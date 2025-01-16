using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using HandyControl.Controls;

namespace DroneDataCollection;

[ObservableObject]
public partial class DataVisualizationPanel {

    [ObservableProperty]
    public DataExtractor dataExtractor = new DataExtractor();

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
    [Editor(typeof(DateTimePropertyEditor), typeof(DateTimePropertyEditor))]
    public partial DateTime minTime { get; set; } = DateTime.MinValue;

    [ObservableProperty]
    [Editor(typeof(DateTimePropertyEditor), typeof(DateTimePropertyEditor))]
    public partial DateTime maxTime { get; set; } = DateTime.MaxValue;

    [ObservableProperty]
    [Editor(typeof(CheckComboBoxPropertyEditor), typeof(CheckComboBoxPropertyEditor))]
    public partial CheckComboBoxProperty<string> device { get; set; } = new CheckComboBoxProperty<string>();

    [ObservableProperty]
    [Editor(typeof(CheckComboBoxPropertyEditor), typeof(CheckComboBoxPropertyEditor))]
    public partial CheckComboBoxProperty<string> attentionField { get; set; } = new CheckComboBoxProperty<string>() {
        selectedItems = new ObservableCollection<string>(fieldNames),
        itemsSource = new ObservableCollection<string>(fieldNames)
    };

    public string sql {
        get {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"SELECT ({string.Join(',', attentionField.selectedItems)})");
            stringBuilder.AppendLine("FROM data");

            List<int> deviceId = device.selectedItems
                .Where(s => App.instance.deviceService.deviceIdMap.ContainsKey(s))
                .Select(s => App.instance.deviceService.deviceIdMap[s])
                .ToList();

            //TODO 添加筛选条件 1字段time必须在minTime和maxTime之间 2字段device_id必须在deviceId范围内
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
                stringBuilder.AppendLine("WHERE " + whereClause);
            }

            
            return stringBuilder.ToString();

        }
    }

}
