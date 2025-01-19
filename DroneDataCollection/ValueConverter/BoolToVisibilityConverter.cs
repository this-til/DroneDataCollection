using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DroneDataCollection;

public class BoolToVisibilityConverter : IValueConverter {

    public static BoolToVisibilityConverter instance { get; } = new BoolToVisibilityConverter();

    // 将值从绑定源转换为绑定目标
    public object Convert
    (
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    ) {

        switch (value) {
            // 检查值是否为bool类型，并进行转换
            case bool boolValue:
                return boolValue
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            case Visibility visibility:
                return visibility;
            default:
                return Visibility.Collapsed;
        }

    }

    // 将值从绑定目标转换回绑定源（此场景通常不需要）
    public object ConvertBack
    (
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    ) {
        throw new NotImplementedException();
    }

}
