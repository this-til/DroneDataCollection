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
        this.Loaded += MainWindow_Loaded;
        this.Closing += MainWindow_Closed;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
        loadDatabaseConfig();
        sqlService.connectDatabase(getConnectionStringFromConfig());
        if (sqlService.sqlConnection is not null) {
            databaseButton.Content = "关闭数据库链接";
        }
    }

    private string getConnectionStringFromConfig() => $"server={configService.userConfig.host};port={configService.userConfig.port};database={configService.userConfig.database};user={configService.userConfig.user};password={configService.userConfig.password};Min Pool Size=5;Max Pool Size=50";

    private void MainWindow_Closed(object? sender, CancelEventArgs cancelEventArgs) {
        sqlService.closeDatabase();
    }

    private void DatabaseButton_OnClick(object sender, RoutedEventArgs e) {

        if (sqlService.sqlConnection is not null) {
            sqlService.closeDatabase();
            databaseButton.Content = "连接数据库";
            return;
        }
        saveDatabaseConfig();
        sqlService.connectDatabase(getConnectionStringFromConfig());
        if (sqlService.sqlConnection is not null) {
            databaseButton.Content = "关闭数据库链接";
        }
    }

    private void saveDatabaseConfig() {
        configService.userConfig.host = hostTextBox.Text;
        configService.userConfig.database = databaseTextBox.Text;
        configService.userConfig.port = portTextBox.Text;
        configService.userConfig.user = userTextBox.Text;
        configService.userConfig.password = passwordTextBox.Text;
        configService.saveConfig();
    }

    private void loadDatabaseConfig() {
        hostTextBox.Text = configService.userConfig.host;
        databaseTextBox.Text = configService.userConfig.database;
        portTextBox.Text = configService.userConfig.port;
        userTextBox.Text = configService.userConfig.user;
        passwordTextBox.Text = configService.userConfig.password;
    }

}
