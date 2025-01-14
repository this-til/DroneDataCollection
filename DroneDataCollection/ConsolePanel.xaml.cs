using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Controls;
using System.Windows.Threading;
using log4net;
using log4net.Appender;
using log4net.Core;
using log4net.Repository;

namespace DroneDataCollection;

public partial class ConsolePanel : UserControl {

    public ObservableCollection<string> logMessage { get; } = new ObservableCollection<string>();

    public ConsolePanel() {
        InitializeComponent();
        this.DataContext = this;
    }

}

