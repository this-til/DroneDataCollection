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
using Microsoft.Data.SqlClient;
using Window = System.Windows.Window;

namespace DroneDataCollection;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window {

    public LogService logService { get; } = new LogService();

    public YamlService yamlService { get; } = new YamlService();

    public ConfigService configService { get; } = new ConfigService();

    public SqlService sqlService { get; } = new SqlService();

    public MainWindow() {
        InitializeComponent();
        this.DataContext = this;
    }

    private void ConnectDatabaseButton_OnClick(object sender, RoutedEventArgs e) {
        string host = hostTextBox.Text;
        string database = databaseTextBox.Text;
        string user = userTextBox.Text;
        string password = passwordTextBox.Text;

        string connectionString = $"Server={host};Database={database};User Id={user};Password={password};";

        sqlService.connectDatabase(connectionString);
    }

    private void CloseDatabaseButton_OnClick(object sender, RoutedEventArgs e) {
    }

}
