using System.Collections.ObjectModel;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentFTP;
using log4net;
using Microsoft.Data.Analysis;
using MySql.Data.MySqlClient;

namespace DroneDataCollection;

public partial class DeviceService : ObservableObject {

    [ObservableProperty]
    public partial ObservableCollection<RunTimeDevice> runTimeDeviceCollection { get; set; } = new ObservableCollection<RunTimeDevice>();

    public Dictionary<string, int> deviceIdMap { get; } = new Dictionary<string, int>();

    public Dictionary<int, string> idMap { get; } = new Dictionary<int, string>();

    private DispatcherTimer timer;

    public ILog log = LogManager.GetLogger(typeof(DeviceService));

    public event Action? loadDeviceComplete;

    public DeviceService() {
        App.instance.sqlService.linkedDatabaseEvent += sqlServiceOnlinkedDatabaseEvent;
        App.instance.sqlService.closeConnectionDatabaseEvent += sqlServiceOncloseConnectionDatabaseEvent;
        App.instance.Exit += mainWindowOnClosed;

        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += timerTick;
        timer.Start();
    }

    private void timerTick(object? sender, EventArgs e) {
        foreach (RunTimeDevice runTimeDevice in runTimeDeviceCollection) {
            if (runTimeDevice.runningTask is null || runTimeDevice.runningTask.IsCompleted) {
                runTimeDevice.runningTask = Task.Run(() => monitoringDevice(runTimeDevice));
                runTimeDevice.runningTask.ContinueWith
                (
                    t => {
                        App.instance.Dispatcher.Invoke
                        (
                            () => { runTimeDevice.error = t.Exception?.InnerException?.Message ?? t.Exception?.Message ?? string.Empty; }
                        );
                    }
                );
            }
        }
    }

    private void sqlServiceOnlinkedDatabaseEvent() {
        runTimeDeviceCollection.Clear();
        deviceIdMap.Clear();
        Task.Run
        (
            async () => {
                List<Device> devices = await App.instance.sqlService.query<Device>("SELECT * FROM device");
                foreach (Device device in devices) {
                    deviceIdMap.TryAdd(device.host_name, device.id);
                    idMap.TryAdd(device.id, device.host_name);
                }
                App.instance.Dispatcher.Invoke
                (
                    () => {
                        foreach (Device device in devices) {
                            if (device.deleted) {
                                continue;
                            }
                            RunTimeDevice runTimeDevice = new RunTimeDevice {
                                id = device.id,
                                hostName = device.host_name,
                                ip = device.ip,
                                synchronizationTime = device.synchronization_time,
                            };
                            runTimeDeviceCollection.Add(runTimeDevice);
                        }
                        loadDeviceComplete?.Invoke();
                    }
                );
            }
        );

    }

    private async Task monitoringDevice(RunTimeDevice device) {

        IntPtr vk7016N_createNetworkContext = libvk7016n.VK7016N_CreateNetworkContext(Marshal.StringToHGlobalAnsi(device.ip));
        if (device.VK7016N == IntPtr.Zero) {
            throw new Exception("Could not create network context");
        }

        device.saveDir = Marshal.PtrToStringUTF8(libvk7016n.VK7016N_GetSaveDir(vk7016N_createNetworkContext)) ?? string.Empty;
        if (string.IsNullOrWhiteSpace(device.saveDir)) {
            throw new Exception("Get save directory failed");
        }

        await using AsyncFtpClient asyncFtpClient = new AsyncFtpClient(device.ip);
        device.asyncFtpClient = asyncFtpClient;
        asyncFtpClient.Credentials = new NetworkCredential("root", "root");
        await asyncFtpClient.AutoConnect();

        while (device.cancellationTokenSource.Token.IsCancellationRequested == false) {

            if (App.instance.sqlService.sqlConnection is null) {
                continue;
            }

            FtpListItem[] ftpListItems = await asyncFtpClient.GetListing(device.saveDir);

            List<(FtpListItem, DateTime)> valueTuples = ftpListItems.Select
                (
                    f => {
                        if (!DateTime.TryParseExact(f.Name, "yyyyMMdd_HHmmss", null, DateTimeStyles.None, out DateTime time)) {
                            time = DateTime.MinValue;
                        }
                        return (f, time);
                    }
                )
                .Where(c => c.time > device.synchronizationTime)
                .ToList();

            if (valueTuples.Count < 2) {
                continue;
            }

            (FtpListItem, DateTime) valueTuple = valueTuples[^1];
            FtpListItem ftpListItem = valueTuple.Item1;
            DateTime dateTime = valueTuple.Item2;

            byte[] downloadBytes = await asyncFtpClient.DownloadBytes(ftpListItem.FullName, CancellationToken.None);

            DataFrame dataFrame = DataFrame.LoadCsv
            (
                Encoding.UTF8.GetString(downloadBytes),
                ',',
                false,
                Presets.dataField.ToArray()
            );

            dataFrame = await new TimeMerging().modifiedDataFrame(dataFrame);

            await using (MySqlTransaction transaction = await App.instance.sqlService.sqlConnection.BeginTransactionAsync()) {

                long rowsCount = dataFrame.Rows.Count;
                for (int i = 0; i < rowsCount; i++) {
                    MySqlCommand cmd = new MySqlCommand
                    (
                        $"""
                         INSERT INTO data (device_id, time, {string.Join(',', Presets.dataField)}) 
                         VALUES ({device.id}, {string.Join(',', dataFrame.Rows[i])})
                         """,
                        App.instance.sqlService.sqlConnection,
                        transaction
                    );
                    await cmd.ExecuteNonQueryAsync();
                }

                MySqlCommand command = new MySqlCommand
                (
                    $"""
                     UPDATE device 
                     SET synchronization_time = {dateTime + TimeSpan.FromSeconds(1)}
                     WHERE id = {device.id}
                     """,
                    App.instance.sqlService.sqlConnection,
                    transaction
                );
                
                await command.ExecuteNonQueryAsync();
                await transaction.CommitAsync();
            }

            Task.Delay(60000).Wait();

        }

    }

    private void sqlServiceOncloseConnectionDatabaseEvent() {
        foreach (RunTimeDevice runTimeDevice in runTimeDeviceCollection) {
            runTimeDevice.cancellationTokenSource.Cancel();
        }
        runTimeDeviceCollection.Clear();
    }

    private void mainWindowOnClosed(object? sender, EventArgs e) {
        sqlServiceOncloseConnectionDatabaseEvent();
    }

}

public partial class RunTimeDevice : ObservableObject {

    [ObservableProperty]
    public partial int id { get; set; }

    [ObservableProperty]
    public partial string hostName { get; set; } = String.Empty;

    [ObservableProperty]
    public partial DateTime synchronizationTime { get; set; }

    [ObservableProperty]
    public partial string ip { get; set; } = "0.0.0.0";

    [ObservableProperty]
    public partial string error { get; set; } = string.Empty;

    public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    public Task? runningTask;

    public IntPtr VK7016N;

    public string saveDir = String.Empty;

    public AsyncFtpClient asyncFtpClient = null!;

}
