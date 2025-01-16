using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DroneDataCollection;

public partial class DeviceService : ObservableObject {

    [ObservableProperty]
    public partial ObservableCollection<RunTimeDevice> runTimeDeviceCollection { get; set; } = new ObservableCollection<RunTimeDevice>();

    public Dictionary<string, int> deviceIdMap { get; } = new Dictionary<string, int>();

    public DeviceService() {
        App.instance.sqlService.linkedDatabaseEvent += sqlServiceOnlinkedDatabaseEvent;
        App.instance.sqlService.closeConnectionDatabaseEvent += sqlServiceOncloseConnectionDatabaseEvent;
        App.instance.Exit += mainWindowOnClosed;
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
                }
                App.instance.Dispatcher.Invoke
                (
                    () => {
                        foreach (Device device in devices) {
                            if (device.deleted) {
                                continue;
                            }
                            RunTimeDevice runTimeDevice = new RunTimeDevice() {
                                id = device.id,
                                hostName = device.host_name,
                                synchronizationTime = device.synchronization_time,
                                state = DeviceState.offline,
                                ip = "null"
                            };
                            runTimeDeviceCollection.Add(runTimeDevice);
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
                    App.instance.Dispatcher.Invoke(() => device.state = DeviceState.tryConnection);
                    IntPtr vk7016N_createNetworkContext = libvk7016n.VK7016N_CreateNetworkContext(Marshal.StringToHGlobalAnsi(device.hostName));
                    device.VK7016N = vk7016N_createNetworkContext;
                    App.instance.Dispatcher.Invoke
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
    public partial int id { get; set; }

    [ObservableProperty]
    public partial string hostName { get; set; } = String.Empty;

    [ObservableProperty]
    public partial DateTime synchronizationTime { get; set; }

    [ObservableProperty]
    public partial string stateText { get; set; } = string.Empty;

    [ObservableProperty]
    public partial string ip { get; set; } = string.Empty;

    [ObservableProperty]
    public partial DeviceState state {
        get;
        set;
    }

    public IntPtr VK7016N;

    [ObservableProperty]
    public partial bool needToDestroy { get; set; }

    partial void OnstateChanging(DeviceState value) {
        stateText = value switch {
            DeviceState.online => "在线", DeviceState.offline => "离线", DeviceState.tryConnection => "尝试连接", DeviceState.sync => "正在同步", _ => String.Empty
        };
    }

}

public enum DeviceState {

    online,

    offline,

    tryConnection,

    sync

}
