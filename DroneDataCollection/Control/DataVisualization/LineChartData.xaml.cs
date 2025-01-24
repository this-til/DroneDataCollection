using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

[ObservableObject]
public partial class LineChartData {

    /// <summary>
    /// 时间轴
    /// </summary>
    [ObservableProperty]
    public partial string xAxleColumnName { get; set; } = "time";

    [ObservableProperty]
    public partial string deviceColumnName { get; set; } = "device";

    [ObservableProperty]
    public partial string dataType { get; set; } = "";

    [ObservableProperty]
    public partial ObservableCollection<string> allDataTypeList { get; set; } = new ObservableCollection<string>();

    [ObservableProperty]
    public partial string device { get; set; } = "";

    [ObservableProperty]
    public partial ObservableCollection<string> allDeviceList { get; set; } = new ObservableCollection<string>();

    [ObservableProperty]
    public partial SeriesCollection series { get; set; } = new SeriesCollection();

    [ObservableProperty]
    public partial AxesCollection axisY { get; set; } = new AxesCollection();

    [ObservableProperty]
    public partial AxesCollection axisX { get; set; } = new AxesCollection();

    public LineChartData() {
        InitializeComponent();
    }

    partial void OndeviceChanged(string value) {
        upData();
    }

    partial void OndataTypeChanged(string value) {
        upData();
    }

    protected override void OnDataFrameChanging(DataFrame? value) {
        if (value is null) {
            return;
        }

        allDataTypeList.Clear();
        foreach (string se in dataFrame.Columns
                     .Where(c => !c.Name.Equals(xAxleColumnName))
                     .Where(c => !c.Name.Equals(deviceColumnName))
                     .Select(c => c.Name)) {
            allDataTypeList.Add(se);
        }
        if (allDataTypeList.Count > 0) {
            dataType = allDataTypeList[0];
        }

        allDeviceList.Clear();
        foreach (string se in dataFrame.Columns[deviceColumnName]
                     .encasement()
                     .Select(o => o?.ToString())
                     .Where(o => o != null)
                     .ToHashSet()!) {
            allDeviceList.Add(se);
        }
        if (allDataTypeList.Count > 0) {
            device = allDeviceList[0];
        }

        upData();
    }

    protected virtual void upData() {

        if (dataFrame is null) {
            return;
        }
        if (string.IsNullOrWhiteSpace(dataType)) {
            return;
        }
        if (string.IsNullOrWhiteSpace(device)) {
            return;
        }

        List<(object, object)> valueTuples = dataFrame.Rows
            .Where(r => r[deviceColumnName].Equals(device))
            .Select(r => (r[dataType], r[xAxleColumnName]))
            .Where(a => a.Item1 is not null && a.Item2 is not null)
            .OrderBy(a => a.Item2)
            .ToList();

        axisX = [
            new Axis() {
                Title = xAxleColumnName,
                Labels = valueTuples.Select(r => r.Item2.ToString()).ToList(),
            }
        ];
        axisY = [
            new Axis() {
                Title = device
            }
        ];
        series = [
            new LineSeries() {
                Name = dataType,
                Values = new ChartValues<double>
                (
                    valueTuples.Select(r => r.Item1)
                        .Select
                        (
                            r => r is IConvertible convertible
                                ? convertible.ToDouble(null)
                                : double.NaN
                        )
                )
            }
        ];
    }

}
