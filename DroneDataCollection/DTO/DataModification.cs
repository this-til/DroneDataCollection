using System.ComponentModel;
using System.Globalization;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

public partial class DataModification : ObservableObject {

    [ObservableProperty]
    [PropertyEditor(typeof(SwitchPropertyEditor))]
    [DisplayName("启用日期时间拆分")]
    public partial bool timeSplittingVisibility { get; set; } = true;

    [ObservableProperty]
    [PropertyEditor(typeof(ObjectPropertyEditor))]
    [DisplayName("日期时间拆分")]
    [Visibility]
    public partial TimeSplitting timeSplitting { get; set; } = new TimeSplitting();

    [ObservableProperty]
    [PropertyEditor(typeof(SwitchPropertyEditor))]
    [DisplayName("启用日期时间聚合")]
    public partial bool timeMergingVisibility { get; set; }

    [ObservableProperty]
    [PropertyEditor(typeof(ObjectPropertyEditor))]
    [DisplayName("日期时间聚合")]
    [Visibility]
    public partial TimeMerging timeMerging { get; set; } = new TimeMerging();

    [ObservableProperty]
    [PropertyEditor(typeof(SwitchPropertyEditor))]
    [DisplayName("启用设备Id转为设备名")]
    public partial bool deviceIdToNameVisibility { get; set; } = true;

    [ObservableProperty]
    [PropertyEditor(typeof(ObjectPropertyEditor))]
    [DisplayName("设备Id转为设备名")]
    [Visibility]
    public partial DeviceIdToName deviceIdToName { get; set; } = new DeviceIdToName();

    public async Task<DataFrame> modifiedDataFrame(DataFrame dataFrame) {

        if (timeSplittingVisibility) {
            dataFrame = await timeSplitting.modifiedDataFrame(dataFrame);
        }
        if (timeMergingVisibility) {
            dataFrame = await timeMerging.modifiedDataFrame(dataFrame);
        }
        if (deviceIdToNameVisibility) {
            dataFrame = await deviceIdToName.modifiedDataFrame(dataFrame);
        }

        return dataFrame;
    }

}

public abstract partial class DataModificationBase : ObservableObject {

    public abstract Task<DataFrame> modifiedDataFrame(DataFrame dataFrame);

}

public partial class TimeSplitting : DataModificationBase {

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("拆分时间列")]
    public partial string splitTimeColumnName { get; set; } = "time";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("源日期格式")]
    public partial string sourceDateFormat { get; set; } = "yyyy-MM-dd HH:mm:ss";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("输出日期列名")]
    public partial string dateColumnName { get; set; } = "date";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("日期格式")]
    public partial string dateFormat { get; set; } = "yyyyMMdd";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("输出时间列名")]
    public partial string timeColumnName { get; set; } = "time";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("时间格式")]
    public partial string timeFormat { get; set; } = "HHmmss";

    public override Task<DataFrame> modifiedDataFrame(DataFrame dataFrame) {

        int indexOf = dataFrame.Columns.IndexOf(splitTimeColumnName);
        if (indexOf == -1) {
            throw new ArgumentException($"Column '{splitTimeColumnName}' does not exist in the DataFrame.");
        }

        DataFrameColumn dataFrameColumn = dataFrame.Columns[indexOf];

        StringDataFrameColumn date = new StringDataFrameColumn(dateColumnName, dataFrameColumn.Length);
        StringDataFrameColumn time = new StringDataFrameColumn(timeColumnName, dataFrameColumn.Length);

        for (int i = 0; i < dataFrameColumn.Length; i++) {
            DateTime? dateTime = null;
            switch (dataFrameColumn) {
                case DateTimeDataFrameColumn dateTimeColumn:
                    dateTime = dateTimeColumn[i];
                    break;
                default:
                    if (DateTime.TryParseExact(dataFrameColumn[i].ToString(), sourceDateFormat, null, DateTimeStyles.AllowWhiteSpaces, out DateTime _dateTime)) {
                        dateTime = _dateTime;
                    }
                    break;
            }
            if (dateTime is null) {
                continue;
            }
            date[i] = dateTime.Value.ToString(dateFormat);
            time[i] = dateTime.Value.ToString(timeFormat);
        }

        dataFrame.Columns.RemoveAt(indexOf);

        dataFrame.Columns.Insert(indexOf, time);
        dataFrame.Columns.Insert(indexOf, date);

        return Task.FromResult(dataFrame);
    }

}

public partial class TimeMerging : DataModificationBase {

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("合并后日期时间列名")]
    public partial string resultColumnName { get; set; } = "time";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("日期列名")]
    public partial string dateColumnName { get; set; } = "date";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("时间列名")]
    public partial string timeColumnName { get; set; } = "time";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("日期格式")]
    public partial string dateFormat { get; set; } = "yyyyMMdd";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("时间格式")]
    public partial string timeFormat { get; set; } = "HHmmss";

    public override Task<DataFrame> modifiedDataFrame(DataFrame dataFrame) {
        int dateIndexOf = dataFrame.Columns.IndexOf(dateColumnName);
        int timeIndexOf = dataFrame.Columns.IndexOf(timeColumnName);

        if (dateIndexOf == -1 || timeIndexOf == -1) {
            throw new ArgumentException($"One or both of the columns '{dateColumnName}' and '{timeColumnName}' do not exist in the DataFrame.");
        }

        DataFrameColumn dataColumn = dataFrame.Columns[dateIndexOf];
        DataFrameColumn timeColumn = dataFrame.Columns[timeIndexOf];

        DateTimeDataFrameColumn dateTimeColumn = new DateTimeDataFrameColumn
        (
            resultColumnName,
            dataColumn.Length
        );
        for (int i = 0; i < dataColumn.Length; i++) {
            string? dateStr = dataColumn[i]?.ToString();
            string? timeStr = timeColumn[i]?.ToString();
            if (DateTime.TryParseExact(dateStr, dateFormat, null, DateTimeStyles.None, out DateTime date) &&
                DateTime.TryParseExact(timeStr, timeFormat, null, DateTimeStyles.None, out DateTime time)) {
                DateTime combinedDateTime = date.Date.Add(time.TimeOfDay);
                dateTimeColumn[i] = combinedDateTime;
            }
        }

        int minIndexOf = Math.Min(dateIndexOf, timeIndexOf);
        int maxIndexOf = Math.Max(dateIndexOf, timeIndexOf);
        dataFrame.Columns.RemoveAt(maxIndexOf);
        dataFrame.Columns.RemoveAt(minIndexOf);
        dataFrame.Columns.Insert(minIndexOf, dateTimeColumn);

        return Task.FromResult(dataFrame);
    }

}

public partial class DeviceIdToName : DataModificationBase {

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("设备id列名")]
    public partial string idColumnName { get; set; } = "device_id";

    [ObservableProperty]
    [PropertyEditor(typeof(PlainTextPropertyEditor))]
    [DisplayName("设备id列名")]
    public partial string convertColumnName { get; set; } = "device";

    public override Task<DataFrame> modifiedDataFrame(DataFrame dataFrame) {
        int indexOf = dataFrame.Columns.IndexOf(idColumnName);
        if (indexOf == -1) {
            throw new ArgumentException($"Column '{idColumnName}' does not exist in the DataFrame.");
        }
        DataFrameColumn dataFrameColumn = dataFrame.Columns[indexOf];

        DataFrameColumn nameColumn = new StringDataFrameColumn(convertColumnName, dataFrameColumn.Length);

        for (int i = 0; i < dataFrameColumn.Length; i++) {
            int? id = null;
            switch (dataFrameColumn) {
                case Int32DataFrameColumn dateTimeColumn:
                    id = dateTimeColumn[i];
                    break;
                default:
                    if (int.TryParse(dataFrameColumn[i].ToString(), out var _id)) {
                        id = _id;
                    }
                    break;
            }
            if (id is null) {
                continue;
            }
            if (App.instance.deviceService.idMap.TryGetValue(id.Value, out var name)) {
                nameColumn[i] = name;
            }
        }

        dataFrame.Columns.RemoveAt(indexOf);
        dataFrame.Columns.Insert(indexOf, nameColumn);
        return Task.FromResult(dataFrame);
    }

}
