using System.Collections.ObjectModel;
using System.Windows.Controls;
using UserControl = System.Windows.Controls.UserControl;

namespace DroneDataCollection;

public partial class ConsolePanel {

    public ObservableCollection<string> logMessage { get; } = new ObservableCollection<string>();

    public ConsolePanel() {
        InitializeComponent();
        this.DataContext = this;
    }

}
