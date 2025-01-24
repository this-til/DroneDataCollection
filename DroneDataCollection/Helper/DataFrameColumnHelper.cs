using Microsoft.Data.Analysis;

namespace DroneDataCollection;

public static class DataFrameColumnHelper {

    public static IEnumerable<object?> encasement(this DataFrameColumn field) {
        object[] array = new Object[field.Length];
        for (int i = 0; i < field.Length; i++) {
            array[i] = field[i];
        }
        return array;
    }



}
