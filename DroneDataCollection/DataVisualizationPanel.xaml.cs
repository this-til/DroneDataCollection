using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using HandyControl.Controls;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

[ObservableObject]
public partial class DataVisualizationPanel {

    [ObservableProperty]
    public partial DataExtractor dataExtractor { get; set; } = new DataExtractor();

    public DataVisualizationPanel() {
        InitializeComponent();
        this.DataContext = this;
    }

}

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

    [ObservableProperty]
    [PropertyEditor(typeof(DateTimePropertyEditor))]
    public partial DateTime minTime { get; set; } = DateTime.MinValue;

    [ObservableProperty]
    [PropertyEditor(typeof(DateTimePropertyEditor))]
    public partial DateTime maxTime { get; set; } = DateTime.MaxValue;

    [ObservableProperty]
    [PropertyEditor(typeof(CheckComboBoxPropertyEditor))]
    public partial CheckComboBoxProperty<string> device { get; set; } = new CheckComboBoxProperty<string>();

    [ObservableProperty]
    [PropertyEditor(typeof(CheckComboBoxPropertyEditor))]
    public partial CheckComboBoxProperty<string> attentionField { get; set; } = new CheckComboBoxProperty<string>();

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    public partial string sql { get; set; } = string.Empty;

    public DataExtractor() {
        sql = generateSql();
        PropertyChanged += OnPropertyChanged;
        foreach (string fieldName in fieldNames) {
            attentionField.itemsSource.Add(fieldName);
            attentionField.selectedItems.Add(fieldName);
        }
    }

    public string generateSql() {
        StringBuilder stringBuilder = new StringBuilder();

        stringBuilder.Append($"SELECT ({string.Join(',', attentionField.selectedItems)})\nFROM data");

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

}

public partial class DataModificationBase : ObservableObject {

}

public partial class TimeSplitting : DataModificationBase {

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    public partial string columnName { get; set; } = "time";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    public partial string dataColumnName { get; set; } = "time";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    public partial string timeColumnName { get; set; } = "time";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    public partial string dataFormat { get; set; } = "yyyyMMdd";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    public partial string timeFormat { get; set; } = "HHmmss";

    
    
}
