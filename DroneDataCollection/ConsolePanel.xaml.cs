using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace DroneDataCollection;

public partial class ConsolePanel : UserControl {

    public ObservableCollection<string> logMessage { get; } = new ObservableCollection<string>();

    public ConsolePanel() {
        InitializeComponent();
        this.DataContext = this;
    }

}

