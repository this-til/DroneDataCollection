using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts.Defaults;
using MySql.Data.Types;

namespace DroneDataCollection;

public partial class DevicePanel {

    public ObservableCollection<RunTimeDevice> runTimeDeviceCollection { get; } = new ObservableCollection<RunTimeDevice>();

    public DevicePanel() {
        InitializeComponent();
        this.DataContext = this;
        MainWindow.mainWindow.sqlService.linkedDatabaseEvent += sqlServiceOnlinkedDatabaseEvent;
        MainWindow.mainWindow.sqlService.closeConnectionDatabaseEvent += sqlServiceOncloseConnectionDatabaseEvent;

        AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(onButton_Click));

        // 创建一个DispatcherTimer实例
        DispatcherTimer timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += timerCallback;
        timer.Start();

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
                List<Device> devices = await MainWindow.mainWindow.sqlService.query<Device>("SELECT * FROM device");
                Dispatcher.Invoke
                (
                    () => {
                        foreach (Device device in devices) {
                            if (device.deleted) {
                                continue;
                            }
                            runTimeDeviceCollection.Add
                            (
                                new RunTimeDevice() {
                                    Id = device.id,
                                    HostName = device.host_name,
                                    SynchronizationTime = device.synchronization_time,
                                    sync = false,
                                    online = false,
                                    Ip = "null"
                                }
                            );
                        }
                    }
                );
            }
        );

    }

    private void timerCallback(object? sender, EventArgs e) {
        /*IntPtr vk7016N_createNetworkContext = libvk7016n.VK7016N_CreateNetworkContext(Marshal.StringToHGlobalAnsi(HOSTNAME));
        if (vk7016N_createNetworkContext == IntPtr.Zero) {
            
        }*/
    }

    private void sqlServiceOncloseConnectionDatabaseEvent() {
        runTimeDeviceCollection.Clear();
    }

}

public partial class RunTimeDevice : ObservableObject {

    [ObservableProperty]
    public int id;

    [ObservableProperty]
    public string hostName = String.Empty;

    [ObservableProperty]
    public MySqlDateTime synchronizationTime;

    [ObservableProperty]
    public string onlineState = String.Empty;

    [ObservableProperty]
    public string syncState = String.Empty;

    public bool online {
        get;
        set {
            field = value;
            OnlineState = field
                ? "在线"
                : "离线";
        }
    }

    public bool sync {
        get;
        set {
            field = value;
            SyncState = field
                ? "正在同步"
                : "未操作";
        }
    }

    [ObservableProperty]
    public string ip = string.Empty;

}
