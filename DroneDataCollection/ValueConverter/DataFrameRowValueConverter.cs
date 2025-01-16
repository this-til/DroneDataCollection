using System.Globalization;
using System.Windows.Data;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

public class DataFrameRowValueConverter : IValueConverter {

    public object? Convert
    (
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    ) {
        if (value is DataFrameRow indexedClass && parameter is string columnName) {
            return indexedClass[columnName];
        }
        return null;
    }

    public object? ConvertBack
    (
        object? value,
        Type targetType,
        object? parameter,
        CultureInfo culture
    ) {
        return null;
    }

}
