using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HandyControl.Controls;
using Window = System.Windows.Window;

namespace DroneDataCollection;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

    public static MainWindow mainWindow { get; private set; } = null!;

    public LogService logService { get; }

    public YamlService yamlService { get; }

    public ConfigService configService { get; }

    public SqlService sqlService { get; }

    public MainWindow() {
        mainWindow = this;
        InitializeComponent();
        logService = new LogService();
        yamlService = new YamlService();
        configService = new ConfigService();
        sqlService = new SqlService();
        this.DataContext = this;
        this.Closing += MainWindow_Closed;
    }


    private void MainWindow_Closed(object? sender, CancelEventArgs cancelEventArgs) {
        sqlService.closeDatabase();
    }



}
