using System.Net.Mime;
using System.Windows;
using System.Windows.Data;
using log4net;
using Microsoft.Data.Analysis;
using Microsoft.Identity.Client;
using Microsoft.Win32;

namespace DroneDataCollection;

public partial class DataWindow {

    public ILog log = LogManager.GetLogger(typeof(DataWindow));

    public static readonly DependencyProperty dataFrameProperty = DependencyProperty.Register
    (
        nameof(dataFrame),
        typeof(DataFrame),
        typeof(DataWindow),
        new PropertyMetadata(new DataFrame())
    );

    public DataFrame dataFrame {
        get => (DataFrame)GetValue(dataFrameProperty);
        set => SetValue(dataFrameProperty, value);
    }

    public static readonly DependencyProperty dataElementProperty = DependencyProperty.Register
    (
        nameof(dataElement),
        typeof(DataFrameGrid),
        typeof(DataWindow),
        new PropertyMetadata(null, (o, args) => ((DataWindow)o).OnDataElementChanging(args.NewValue as DataFrameGrid))
    );

    public DataFrameGrid dataElement {
        get => (DataFrameGrid)GetValue(dataElementProperty);
        set => SetValue(dataElementProperty, value);
    }

    protected void OnDataElementChanging(DataFrameGrid? dataFrameGrid) {

        if (dataFrameGrid == null) {
            return;
        }
        BindingOperations.SetBinding
        (
            dataFrameGrid,
            DataFrameGrid.dataFrameProperty,
            new Binding(nameof(dataFrame)) {
                Source = this,
                Mode = BindingMode.TwoWay,
            }
        );

    }

    public DataWindow() {
        InitializeComponent();
        this.DataContext = this;
    }

    private void onClickOutCsv(object sender, RoutedEventArgs e) {

        /*Task.Run
        (
            async () => {
                SaveFileDialog dlg = new SaveFileDialog();
                dlg.OverwritePrompt = true;
                dlg.Title = "保存数据";
                if (dlg.ShowDialog(MainWindow.mainWindow) == true) {
                    string dlgFileName = dlg.FileName;
                    log.Debug(dlgFileName);
                }

            }
        );*/

        /*Thread thread = new Thread
        (
            () => {
                Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
                dlg.OverwritePrompt = true;
                dlg.Title = "保存数据";
                dlg.DefaultExt = ".csv";
                dlg.Filter = "txt files(*.txt)|*.txt|csv files(*.csv)|*.csv|All files(*.*)|*.*";
                dlg.RestoreDirectory = true;
                dlg.InitialDirectory = "C:\\";
                if (dlg.ShowDialog() == true) {
                    string dlgFileName = dlg.FileName;
                    log.Debug(dlgFileName);
                }
            }
        );
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
        thread.Join();*/
    }

}
