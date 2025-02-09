using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using FluentFTP;
using HandyControl.Controls;
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
        timer.Interval = TimeSpan.FromSeconds(5);
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
                            () => {
                                string? error = t.Exception?.InnerException?.Message ?? t.Exception?.Message;
                                if (error is not null) {
                                    runTimeDevice.error = error;
                                    runTimeDevice.state = "出错了...";
                                }
                                if (t.Exception?.InnerException != null) {
                                    log.Error(t.Exception.InnerException.Message, t.Exception.InnerException);
                                }
                            }
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

    private void sqlServiceOncloseConnectionDatabaseEvent() {
        foreach (RunTimeDevice runTimeDevice in runTimeDeviceCollection) {
            runTimeDevice.cancellationTokenSource.Cancel();
        }
        runTimeDeviceCollection.Clear();
    }

    private async Task monitoringDevice(RunTimeDevice device) {
        App.instance.Dispatcher.Invoke
        (
            () => { device.state = "连接中..."; }
        );
        IntPtr vk7016N_createNetworkContext = libvk7016n.VK7016N_CreateNetworkContext(Marshal.StringToHGlobalAnsi(device.ip));
        if (vk7016N_createNetworkContext == IntPtr.Zero) {
            App.instance.Dispatcher.Invoke
            (
                () => {
                    device.error = "无法连接到设备";
                    device.state = "出错了...";
                }
            );
            return;
        }
        device.VK7016N = vk7016N_createNetworkContext;
        try {
            string hostName = Marshal.PtrToStringUTF8(libvk7016n.VK7016N_GetHostname(vk7016N_createNetworkContext)) ?? string.Empty;
            if (string.IsNullOrWhiteSpace(hostName)) {
                throw new Exception("获取设备名失败");
            }
            if (!Equals(hostName, device.hostName)) {
                throw new Exception($"设备名称'{hostName}'与数据库中预留名称'{device.hostName}'不一致");
            }

            device.saveDir = Marshal.PtrToStringUTF8(libvk7016n.VK7016N_GetSaveDir(vk7016N_createNetworkContext)) ?? string.Empty;
            if (string.IsNullOrWhiteSpace(device.saveDir)) {
                throw new Exception($"设备'{hostName}'获取离线保存目录失败");
            }

            await using AsyncFtpClient asyncFtpClient = new AsyncFtpClient(device.ip);
            device.asyncFtpClient = asyncFtpClient;
            asyncFtpClient.Credentials = new NetworkCredential("root", "root");
            await asyncFtpClient.AutoConnect();

            while (device.cancellationTokenSource.Token.IsCancellationRequested == false) {

                if (App.instance.sqlService.sqlConnection?.State != ConnectionState.Open) {
                    throw new Exception("数据库连接异常");
                }

                FtpListItem[] ftpListItems = await asyncFtpClient.GetListing(device.saveDir, device.cancellationTokenSource.Token);

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
                    .OrderBy(c => c.time)
                    .ToList();

                bool headerFile = valueTuples.Count == 1;

                if (valueTuples.Count == 0) {
                    App.instance.Dispatcher.Invoke(() => { device.state = "最新状态..."; });
                    goto end;
                }

                App.instance.Dispatcher.Invoke(() => { device.state = "同步中..."; });

                if (headerFile) {
                    int result = libvk7016n.VK7016N_SetOfflineSave(vk7016N_createNetworkContext, 0);
                    if (result < 0) {
                        throw new Exception($"设备'{device.hostName}'关闭自动保存失败");
                    }
                }

                try {

                    (FtpListItem firstOrDefault, DateTime dateTime)? firstOrDefault = valueTuples.FirstOrDefault();
                    if (firstOrDefault is null) {
                        continue;
                    }

                    FtpListItem ftpListItem = firstOrDefault.Value.firstOrDefault;
                    DateTime dateTime = firstOrDefault.Value.dateTime;

                    byte[] downloadBytes = await asyncFtpClient.DownloadBytes(ftpListItem.FullName, device.cancellationTokenSource.Token);

                    DataFrame dataFrame = DataFrame.LoadCsv
                    (
                        Encoding.UTF8.GetString(downloadBytes),
                        ',',
                        false,
                        Presets.dataField.ToArray()
                    );

                    dataFrame = await new TimeMerging().modifiedDataFrame(dataFrame);

                    await using MySqlTransaction transaction = await App.instance.sqlService.sqlConnection.BeginTransactionAsync();

                    try {

                        long rowsCount = dataFrame.Rows.Count;
                        for (int i = 0; i < rowsCount; i++) {
                            await using MySqlCommand cmd = new MySqlCommand
                            (
                                $"""
                                 INSERT INTO data (device_id, {string.Join(',', Presets.insertDataField)}) 
                                 VALUES ({device.id}, {string.Join(',', dataFrame.Rows[i].Select(o => $"'{o}'"))})
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
                             SET synchronization_time = '{dateTime + TimeSpan.FromSeconds(1)}'
                             WHERE id = {device.id}
                             """,
                            App.instance.sqlService.sqlConnection,
                            transaction
                        );

                        await command.ExecuteNonQueryAsync();
                        await transaction.CommitAsync();

                        App.instance.Dispatcher.Invoke
                        (
                            () => { device.synchronizationTime = dateTime + TimeSpan.FromSeconds(1); }
                        );
                    }
                    catch (Exception) {
                        await transaction.RollbackAsync();
                        throw;
                    }

                }
                finally {
                    if (headerFile) {
                        int result = libvk7016n.VK7016N_SetOfflineSave(vk7016N_createNetworkContext, 1);
                        if (result < 0) {
                            throw new Exception($"设备'{device.hostName}'开启自动保存失败");
                        }
                    }
                }

                App.instance.Dispatcher.Invoke(() => { device.state = "休息中..."; });
                end:
                await Task.Delay
                (
                    headerFile
                        ? 100
                        : 600000
                );

            }
        }
        finally {
            libvk7016n.VK7016N_ContextDestroy(ref vk7016N_createNetworkContext);
            device.VK7016N = vk7016N_createNetworkContext;
        }

    }

    private void mainWindowOnClosed(object? sender, EventArgs e) {
        sqlServiceOncloseConnectionDatabaseEvent();
    }

    public Task<RunTimeDevice> addDevice() {
        string name = generateConflictFreeHostName("DAQ-VK7016n-081125");
        string ip = generateConflictFreeIp("192.168.0.100");

        RunTimeDevice runTimeDevice = new RunTimeDevice() {
            hostName = name,
            ip = ip,
        };

        if (App.instance.sqlService.sqlConnection is null) {
            return Task.FromException<RunTimeDevice>(new Exception("Could not connect to SQL Server"));
        }

        return Task.Run
        (
            async () => {
                await using (MySqlCommand command = new MySqlCommand
                             (
                                 $"""
                                  INSERT INTO device (host_name, ip) 
                                  VALUES('{name}', '{ip}')
                                  """,
                                 App.instance.sqlService.sqlConnection
                             )
                            ) {
                    await command.ExecuteNonQueryAsync();
                }

                await using (MySqlCommand command = new MySqlCommand("SELECT LAST_INSERT_ID()", App.instance.sqlService.sqlConnection)) {
                    object? obj = await command.ExecuteScalarAsync();
                    int id = int.Parse(obj?.ToString() ?? string.Empty);
                    App.instance.Dispatcher.Invoke
                    (
                        () => {
                            runTimeDevice.id = id;
                            runTimeDeviceCollection.Add(runTimeDevice);
                        }
                    );
                }

                return runTimeDevice;
            }
        );

    }

    public Task deleteDevice(params RunTimeDevice[] device) {
        if (App.instance.sqlService.sqlConnection is null) {
            return Task.FromException<RunTimeDevice>(new Exception("Could not connect to SQL Server"));
        }
        RunTimeDevice[] runTimeDevices = device.Where(runTimeDeviceCollection.Contains).ToArray();
        if (runTimeDevices.Length == 0) {
            return Task.CompletedTask;
        }
        foreach (RunTimeDevice runTimeDevice in runTimeDevices) {
            runTimeDevice.cancellationTokenSource.Cancel();
            runTimeDeviceCollection.Remove(runTimeDevice);
        }
        return Task.Run
        (
            async () => {
                await using MySqlCommand mySqlCommand = new MySqlCommand
                (
                    $"""
                     UPDATE device
                     SET deleted = true
                     WHERE id IN  ({string.Join(',', runTimeDevices.Select(d => $"'{d.id}'"))})
                     """,
                    App.instance.sqlService.sqlConnection
                );
                await mySqlCommand.ExecuteNonQueryAsync();
            }
        );

    }

    public string generateConflictFreeHostName(string name) {
        int polling = 0;
        string outName = name;
        while (runTimeDeviceCollection.Select(d => d.hostName).Contains(outName)) {
            polling++;
            outName = $"{name}({polling})";
        }
        return outName;
    }

    public string generateConflictFreeIp(string ip) {
        int lastIndexOf = ip.LastIndexOf('.');
        string networkSegment = ip.Substring(0, lastIndexOf);
        int polling = int.Parse(ip.Substring(lastIndexOf + 1));
        string outIp = ip;
        while (runTimeDeviceCollection.Select(d => d.ip).Contains(outIp) && polling < 255) {
            polling++;
            outIp = $"{networkSegment}.{polling}";
        }
        return outIp;
    }

    public partial class RunTimeDevice : ObservableObject {

        [ObservableProperty]
        public partial int id { get; set; }

        [ObservableProperty]
        public partial string hostName { get; set; } = String.Empty;

        [ObservableProperty]
        public partial DateTime synchronizationTime { get; set; } = DateTime.MinValue;

        [ObservableProperty]
        public partial string ip { get; set; } = "0.0.0.0";

        [ObservableProperty]
        public partial string state { get; set; } = string.Empty;

        [ObservableProperty]
        public partial string error { get; set; } = string.Empty;

        public CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public Task? runningTask;

        public IntPtr VK7016N;

        public string saveDir = String.Empty;

        public AsyncFtpClient asyncFtpClient = null!;

        partial void OnhostNameChanging(string value) {
            if (!App.instance.deviceService.runTimeDeviceCollection.Contains(this)) {
                return;
            }

            foreach (RunTimeDevice runTimeDevice in App.instance.deviceService.runTimeDeviceCollection) {
                if (runTimeDevice == this) {
                    continue;
                }
                if (runTimeDevice.hostName.Equals(value)) {

                    string _hostName = hostName;
                    Task.Run
                    (
                        () => {
                            Task.FromResult(App.instance.Dispatcher.Invoke(() => hostName = _hostName));
                            MessageBox.Show("无法重命名，名称重复", "ERROR");
                        }
                    );

                    return;
                }
            }

            if (App.instance.sqlService.sqlConnection is null) {
                return;
            }

            Task.Run
            (
                async () => {
                    await using MySqlCommand command = new MySqlCommand
                    (
                        $"""
                         UPDATE device 
                         SET host_name = '{value}'
                         WHERE id = {id}
                         """,
                        App.instance.sqlService.sqlConnection
                    );
                    await command.ExecuteNonQueryAsync();
                }
            );
        }

        partial void OnipChanging(string value) {
            if (!App.instance.deviceService.runTimeDeviceCollection.Contains(this)) {
                return;
            }
            foreach (RunTimeDevice runTimeDevice in App.instance.deviceService.runTimeDeviceCollection) {
                if (runTimeDevice == this) {
                    continue;
                }
                if (runTimeDevice.ip.Equals(value)) {
                    string _ip = ip;
                    Task.Run
                    (
                        () => {
                            Task.FromResult(App.instance.Dispatcher.Invoke(() => ip = _ip));
                            MessageBox.Show("无法重命名，ip重复", "ERROR");
                        }
                    );

                    return;
                }
            }
            if (App.instance.sqlService.sqlConnection is null) {
                return;
            }
            Task.Run
            (
                async () => {
                    await using MySqlCommand command = new MySqlCommand
                    (
                        $"""
                         UPDATE device 
                         SET ip = '{value}'
                         WHERE id = {id}
                         """,
                        App.instance.sqlService.sqlConnection
                    );
                    await command.ExecuteNonQueryAsync();
                }
            );

            cancellationTokenSource?.Cancel();
        }

    }

}
