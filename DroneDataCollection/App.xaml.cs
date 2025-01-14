using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;
using HandyControl.Controls;
using log4net;
using MessageBox = System.Windows.MessageBox;

namespace DroneDataCollection;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {

    public ILog log { get; } = LogManager.GetLogger(typeof(App));

    public static App instance { get; private set; } = null!;

    public LogService logService { get; }

    public YamlService yamlService { get; }

    public ConfigService configService { get; }

    public SqlService sqlService { get; }

    public App() {
        instance = this;
        logService = new LogService();
        yamlService = new YamlService();
        configService = new ConfigService();
        sqlService = new SqlService();
        Startup += onStartupEventHandler;
    }

    private void onStartupEventHandler(object sender, StartupEventArgs args) {
        TaskScheduler.UnobservedTaskException += onTaskSchedulerOnUnobservedTaskException;
        this.DispatcherUnhandledException += onDispatcherUnhandledException;
    }

    private void onDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs args) {
        log.Error("ui has unknown error:", args.Exception);
        args.Handled = true;
    }

    private void onTaskSchedulerOnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs args) {
        log.Error("task has unknown error:", args.Exception);
        args.SetObserved();
    }

    protected override void OnExit(ExitEventArgs e) {
        base.OnExit(e);
        sqlService.closeDatabase();
    }

}
