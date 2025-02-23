using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Security.Policy;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.Analysis;
using Microsoft.Win32;

namespace DroneDataCollection;

[ObservableObject]
public partial class DataVisualizationPanel {

    [ObservableProperty]
    public partial DataVisualization dataVisualization { get; set; } = new DataVisualization();

    public DataVisualizationPanel() {
        InitializeComponent();
        this.DataContext = this;

        App.instance.sqlService.closeConnectionDatabaseEvent += onSqlServiceOncloseConnectionDatabaseEvent;
        App.instance.deviceService.loadDeviceComplete += onDeviceServiceOnloadDeviceComplete;
    }

    private void onDeviceServiceOnloadDeviceComplete() {
        ObservableCollection<string> deviceItemsSource = dataVisualization.dataExtractor.device.itemsSource;
        ObservableCollection<string> deviceSelectedItems = dataVisualization.dataExtractor.device.selectedItems;

        deviceItemsSource.Clear();
        foreach (string key in App.instance.deviceService.deviceIdMap.Keys) {
            deviceItemsSource.Add(key);
        }
        if (deviceSelectedItems.Count <= 0 && deviceItemsSource.Count > 0) {
            deviceSelectedItems.Add(deviceItemsSource[0]);
        }
    }

    private void onSqlServiceOncloseConnectionDatabaseEvent() {
        dataVisualization.dataExtractor.device.itemsSource.Clear();
    }

    /*private void onClickRender(object sender, RoutedEventArgs e) {
        Task.Run
        (
            async () => {
                DataFrame dataFrame = await dataVisualization.createDataFrame();

                Dispatcher.Invoke
                (
                    () => {
                        DataWindow dataWindow = new DataWindow();
                        dataWindow.dataFrame = dataFrame;
                        dataWindow.dataElement = dataVisualization.dataRender.create();
                        dataWindow.Show();
                    }
                );
            }
        );
    }

    private void onClickExportCSV(object sender, RoutedEventArgs e) {

        Task.Run
        (
            async () => {
                OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "导出CSV";
                dlg.FileName = "Data.csv";
                dlg.DefaultExt = ".csv";
                dlg.Filter = "CSV|*.csv|All files (*.*)|*.*";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() ?? false) {

                }
            }
        );

    }*/

}

public partial class DataVisualization : ObservableObject {

    [ObservableProperty]
    [PropertyEditor(typeof(ObjectPropertyEditor))]
    [DisplayName("数据渲染")]
    public partial DataRender dataRender { get; set; } = new DataRender();

    [ObservableProperty]
    [PropertyEditor(typeof(ObjectPropertyEditor))]
    [DisplayName("导出为CSV")]
    public partial DataSaveAs dataSaveAs { get; set; } = new DataSaveAs();

    [ObservableProperty]
    [PropertyEditor(typeof(ObjectPropertyEditor))]
    [DisplayName("数据导出")]
    public partial DataExtractor dataExtractor { get; set; } = new DataExtractor();

    [ObservableProperty]
    [PropertyEditor(typeof(ObjectPropertyEditor))]
    [DisplayName("数据修饰")]
    public partial DataModification dataModification { get; set; } = new DataModification();

    public async Task<DataFrame> createDataFrame() {
        DataFrame dataFrame = await dataExtractor.extractor();
        dataFrame = await dataModification.modifiedDataFrame(dataFrame);
        return dataFrame;
    }

}

public partial class DataSaveAs : ObservableObject {

    [ObservableProperty]
    [PropertyEditor(typeof(ButtonEditor))]
    [DisplayName("导出")]
    public partial RoutedEventHandler onClickSaveAs { get; set; }

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("保存目录")]
    public partial string saveDirectory { get; set; }

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("文件后缀")]

    public partial string fileSuffix { get; set; } = "csv";

    public DataSaveAs() {
        saveDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\DroneDataCollection\Data\";

        onClickSaveAs = (sender, args) => Task.Run
        (
            async () => {
                /*OpenFileDialog dlg = new OpenFileDialog();
                dlg.Title = "导出CSV";
                dlg.FileName = "Data.csv";
                dlg.DefaultExt = ".csv";
                dlg.Filter = "CSV|*.csv|All files (*.*)|*.*";
                dlg.InitialDirectory = "D:\\Downloads";

                if (dlg.ShowDialog() ?? false) {

                }*/

                DataVisualizationPanel dataVisualization = MainWindow.mainWindow.dataVisualizationPanel;
                DataFrame dataFrame = await dataVisualization.dataVisualization.createDataFrame();
                Directory.CreateDirectory(saveDirectory);
                string filePath = Path.Combine(saveDirectory, DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss-ffff") + "." + fileSuffix);
                DataFrame.SaveCsv(dataFrame, filePath);
                Process.Start("explorer.exe", $"/select, \"{filePath}\"");
            }
        );
    }

}

public partial class DataRender : ObservableObject {

    [ObservableProperty]
    [PropertyEditor(typeof(ButtonEditor))]
    [DisplayName("渲染")]
    public partial RoutedEventHandler onClickRender { get; set; }

    [ObservableProperty]
    [PropertyEditor(typeof(EnumPropertyEditor))]
    [DisplayName("渲染类型")]
    public partial DataRenderType dataRenderType { get; set; } = DataRenderType.chartData;

    public DataRender() {
        onClickRender = (s, e) => Task.Run
        (
            async () => {
                DataVisualizationPanel dataVisualization = MainWindow.mainWindow.dataVisualizationPanel;
                DataFrame dataFrame = await dataVisualization.dataVisualization.createDataFrame();
                dataVisualization.Dispatcher.Invoke
                (
                    () => {
                        DataWindow dataWindow = new DataWindow();
                        dataWindow.dataFrame = dataFrame;
                        dataWindow.dataElement = dataVisualization.dataVisualization.dataRender.create();
                        dataWindow.Show();
                    }
                );
            }
        );
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e) {
        base.OnPropertyChanged(e);
        switch (e.PropertyName) {
            case "dataRenderType":
                lineConfigVisibility = false;
                switch (dataRenderType) {
                    case DataRenderType.chartData:
                        break;
                    case DataRenderType.lineChart:
                        lineConfigVisibility = true;
                        break;
                }
                break;
        }
    }

    [ObservableProperty]
    public partial bool lineConfigVisibility { get; set; } = false;

    [ObservableProperty]
    [PropertyEditor(typeof(ObjectPropertyEditor))]
    [DisplayName("折线图渲染配置")]
    [Visibility]
    public partial LineChartConfig lineConfig { get; set; } = new LineChartConfig();

    public DataFrameGrid create() {
        switch (dataRenderType) {
            case DataRenderType.chartData:
                return new ChartData();
            case DataRenderType.lineChart:
                LineChartData lineChartData = new LineChartData();
                lineChartData.xAxleColumnName = lineConfig.xAxleColumnName;
                lineChartData.deviceColumnName = lineConfig.deviceColumnName;
                return lineChartData;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

}

public partial class LineChartConfig : ObservableObject {

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("x轴列名")]
    public partial string xAxleColumnName { get; set; } = "time";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("设备列")]
    public partial string deviceColumnName { get; set; } = "device";

}

public enum DataRenderType {

    chartData,

    lineChart

}
