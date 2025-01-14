using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts.Defaults;

namespace DroneDataCollection;

public partial class DevicePanel {

    public ObservableCollection<RunTimeDevice> runTimeDeviceCollection { get; } = new ObservableCollection<RunTimeDevice>();

    public DevicePanel() {
        InitializeComponent();
        this.DataContext = this;
        App.instance.sqlService.linkedDatabaseEvent += sqlServiceOnlinkedDatabaseEvent;
        App.instance.sqlService.closeConnectionDatabaseEvent += sqlServiceOncloseConnectionDatabaseEvent;

        AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(onButton_Click));

        App.instance.Exit += mainWindowOnClosed;
    }

    private void onButton_Click(object sender, RoutedEventArgs e) {
        Button? button = e.OriginalSource as Button;
        if (button is null) {
            return;
        }
        switch (button.Name) {
            case "rename":
                break;
            case "delete":
                break;
        }
    }

    private void sqlServiceOnlinkedDatabaseEvent() {
        runTimeDeviceCollection.Clear();
        Task.Run
        (
            async () => {
                List<Device> devices = await App.instance.sqlService.query<Device>("SELECT * FROM device");
                Dispatcher.Invoke
                (
                    () => {
                        foreach (Device device in devices) {
                            if (device.deleted) {
                                continue;
                            }
                            RunTimeDevice runTimeDevice = new RunTimeDevice() {
                                Id = device.id,
                                HostName = device.host_name,
                                SynchronizationTime = device.synchronization_time,
                                state = DeviceState.offline,
                                Ip = "null"
                            };
                            runTimeDeviceCollection.Add
                            (
                                runTimeDevice
                            );
                            Task.Run(() => monitoringDevice(runTimeDevice));
                        }
                    }
                );
            }
        );

    }

    private async Task monitoringDevice(RunTimeDevice device) {

        while (!device.needToDestroy) {
            await Task.Delay(1000);
            switch (device.state) {

                case DeviceState.offline:
                    Dispatcher.Invoke(() => device.state = DeviceState.tryConnection);
                    IntPtr vk7016N_createNetworkContext = libvk7016n.VK7016N_CreateNetworkContext(Marshal.StringToHGlobalAnsi(device.HostName));
                    device.VK7016N = vk7016N_createNetworkContext;
                    Dispatcher.Invoke
                    (
                        () => device.state = device.VK7016N == IntPtr.Zero
                            ? DeviceState.offline
                            : DeviceState.online
                    );
                    break;
                case DeviceState.online:
                    break;
                case DeviceState.sync:
                    break;
            }
        }

        if (device.state != DeviceState.offline) {
            libvk7016n.VK7016N_ContextDestroy(ref device.VK7016N);
        }

    }

    private void sqlServiceOncloseConnectionDatabaseEvent() {
        foreach (RunTimeDevice runTimeDevice in runTimeDeviceCollection) {
            runTimeDevice.needToDestroy = true;
        }
        runTimeDeviceCollection.Clear();
    }

    private void mainWindowOnClosed(object? sender, EventArgs e) {
        sqlServiceOncloseConnectionDatabaseEvent();
    }

}

public partial class RunTimeDevice : ObservableObject {

    [ObservableProperty]
    public int id;

    [ObservableProperty]
    public string hostName = String.Empty;

    [ObservableProperty]
    public DateTime synchronizationTime;

    [ObservableProperty]
    public string stateText = string.Empty;

    [ObservableProperty]
    public string ip = string.Empty;

    public DeviceState state {
        get;
        set {
            field = value;
            StateText = field switch {
                DeviceState.online => "在线", DeviceState.offline => "离线", DeviceState.tryConnection => "尝试连接", DeviceState.sync => "正在同步", _ => String.Empty
            };
        }
    }

    public IntPtr VK7016N;

    public bool needToDestroy;

}

public enum DeviceState {

    online,

    offline,

    tryConnection,

    sync

}
