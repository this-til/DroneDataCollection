using System.Windows;

namespace DroneDataCollection;

public partial class DatabasePanel {

    public ConfigService configService => App.instance.configService;

    public SqlService sqlService => App.instance.sqlService;

    public DatabasePanel() {
        InitializeComponent();
        this.DataContext = this;
        this.Loaded += MainWindow_Loaded;
        App.instance.sqlService.linkedDatabaseEvent += sqlServiceOnlinkedDatabaseEvent;
        App.instance.sqlService.closeConnectionDatabaseEvent += sqlServiceOncloseConnectionDatabaseEvent;
    }

    private void sqlServiceOncloseConnectionDatabaseEvent() {
        databaseButton.Content = "连接数据库";
    }

    private void sqlServiceOnlinkedDatabaseEvent() {
        databaseButton.Content = "关闭数据库链接";
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

    private void MainWindow_Loaded(object sender, RoutedEventArgs e) {
        if (!string.IsNullOrEmpty(configService.userConfig.host)) {
            loadDatabaseConfig();
        }
        sqlService.connectDatabase(getConnectionStringFromConfig());
    }

    private string getConnectionStringFromConfig() {
        /*SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder();

        sqlConnectionStringBuilder.DataSource = configService.userConfig.host;
        sqlConnectionStringBuilder.InitialCatalog = configService.userConfig.database;
        sqlConnectionStringBuilder.UserID = configService.userConfig.user;
        sqlConnectionStringBuilder.Password = configService.userConfig.password;

        sqlConnectionStringBuilder.Pooling = true;
        sqlConnectionStringBuilder.MaxPoolSize = 50;
        sqlConnectionStringBuilder.MinPoolSize = 5;
        return sqlConnectionStringBuilder.ToString();*/
        return $"server={configService.userConfig.host};port={configService.userConfig.port};database={configService.userConfig.database};user={configService.userConfig.user};password={configService.userConfig.password};Min Pool Size=5;Max Pool Size=50";
    }

    private void DatabaseButton_OnClick(object sender, RoutedEventArgs e) {

        if (sqlService.sqlConnection is not null) {
            sqlService.closeDatabase();
            return;
        }
        saveDatabaseConfig();
        sqlService.connectDatabase(getConnectionStringFromConfig());
    }

}
