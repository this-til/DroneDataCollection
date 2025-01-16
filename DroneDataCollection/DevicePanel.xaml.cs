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

    public DeviceService deviceService {
        get => App.instance.deviceService;
        private set { }
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

}
