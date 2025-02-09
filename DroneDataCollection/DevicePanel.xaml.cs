using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Button = System.Windows.Controls.Button;
using ButtonBase = System.Windows.Controls.Primitives.ButtonBase;

namespace DroneDataCollection;

public partial class DevicePanel {

    public DeviceService deviceService {
        get => App.instance.deviceService;
    }

    public DevicePanel() {
        InitializeComponent();
        this.DataContext = this;

        AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(onButton_Click));
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

    private void onClickAddDevice(object sender, RoutedEventArgs e) {
    }

}
