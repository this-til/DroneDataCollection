using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Common;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

public partial class DataExtractor : ObservableObject {

    private static List<string> fieldNames = new List<string> {
        "id",
        "device_id",
        "time",
        "ux",
        "uy",
        "uz",
        "hx",
        "hy",
        "hz",
        "t",
        "x",
        "y",
        "h",
        "x1",
        "x2",
        "la_h",
        "gyro_1",
        "gyro_2",
        "gyro_3",
        "gps_1",
        "gps_2",
        "gps_3"
    };

    private static List<string> outFieldNames = new List<string> {
        "device_id",
        "time",
        "ux",
        "uy",
        "uz",
        "hx",
        "hy",
        "hz",
        "t",
        "x",
        "y",
        "h",
        "x1",
        "x2",
        "la_h",
        "gyro_1",
        "gyro_2",
        "gyro_3",
        "gps_1",
        "gps_2",
        "gps_3"
    };

    [ObservableProperty]
    [PropertyEditor(typeof(DateTimePropertyEditor))]
    [DisplayName("时间起始点")]
    public partial DateTime minTime { get; set; } = DateTime.MinValue;

    [ObservableProperty]
    [PropertyEditor(typeof(DateTimePropertyEditor))]
    [DisplayName("时间结束点")]
    public partial DateTime maxTime { get; set; } = DateTime.MaxValue;

    [ObservableProperty]
    [PropertyEditor(typeof(CheckComboBoxPropertyEditor))]
    [DisplayName("设备")]
    public partial CheckComboBoxProperty<string> device { get; set; } = new CheckComboBoxProperty<string>();

    [ObservableProperty]
    [PropertyEditor(typeof(CheckComboBoxPropertyEditor))]
    [DisplayName("导出列")]
    public partial CheckComboBoxProperty<string> attentionField { get; set; } = new CheckComboBoxProperty<string>();

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("sql语句")]
    public partial string sql { get; set; } = string.Empty;

    public DataExtractor() {
        sql = generateSql();
        PropertyChanged += OnPropertyChanged;
        device.selectedItems.CollectionChanged += OnPropertyChanged;
        device.itemsSource.CollectionChanged += OnPropertyChanged;

        attentionField.selectedItems.CollectionChanged += OnPropertyChanged;
        attentionField.itemsSource.CollectionChanged += OnPropertyChanged;

        foreach (string fieldName in fieldNames) {
            attentionField.itemsSource.Add(fieldName);
        }
        foreach (string outFieldName in outFieldNames) {
            attentionField.selectedItems.Add(outFieldName);
        }
    }

    public string generateSql() {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append($"SELECT {string.Join(',', attentionField.selectedItems)}\nFROM data");

        List<int> deviceId = device.selectedItems
            .Where(s => App.instance.deviceService.deviceIdMap.ContainsKey(s))
            .Select(s => App.instance.deviceService.deviceIdMap[s])
            .ToList();

        string whereClause = string.Join
            (
                " AND ",
                new List<string> {
                    $"time BETWEEN '{minTime:yyyy-MM-dd HH:mm:ss}' AND '{maxTime:yyyy-MM-dd HH:mm:ss}'",
                    $"device_id IN ({string.Join(",", deviceId)})"
                }
            )
            .Trim();

        if (!string.IsNullOrEmpty(whereClause)) {
            stringBuilder.Append("\nWHERE ").Append(whereClause);
        }

        return stringBuilder.ToString();

    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e) {
        if (e.PropertyName == nameof(sql)) {
            return;
        }
        sql = generateSql();
    }

    private void OnPropertyChanged(object? sender, NotifyCollectionChangedEventArgs e) {
        sql = generateSql();
    }

    public async Task<DataFrame> extractor() {
        if (App.instance.sqlService.sqlConnection == null) {
            return new DataFrame();
        }
        DbDataReader dbDataReader = await App.instance.sqlService.query(sql);
        return await DataFrame.LoadFrom(dbDataReader);
    }

}
