using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace DroneDataCollection;

public partial class DevicePanel {

    public DeviceService deviceService {
        get => App.instance.deviceService;
    }

    public DevicePanel() {
        InitializeComponent();
        this.DataContext = this;
    }

    protected Task<DeviceService.RunTimeDevice>? cacheAddTask;

    protected void onClickAddDevice(object sender, RoutedEventArgs e) {
        if (cacheAddTask is not null && !cacheAddTask.IsCompleted) {
            return;
        }
        cacheAddTask = deviceService.addDevice();
    }

    protected Task? cacheDeleteTask;

    private void onClickDeleteDevice(object sender, RoutedEventArgs e) {
        if (cacheAddTask is not null && !cacheAddTask.IsCompleted) {
            return;
        }
        cacheDeleteTask = deviceService.deleteDevice
        (
            deviceGrid.SelectedCells
                .Select(info => info.Item as DeviceService.RunTimeDevice)
                .Where(d => d is not null)
                .ToArray()!
        );

    }

}
