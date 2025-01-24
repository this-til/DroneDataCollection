using Microsoft.Data.Analysis;

namespace DroneDataCollection;

public static class DataFrameHelper {

    public static DataFrameColumn? getColumn(this DataFrame dataFrame, string columnName) {
        int indexOf = dataFrame.Columns.IndexOf(columnName);
        return indexOf == -1
            ? null
            : dataFrame.Columns[indexOf];
    }

    /*public static Dictionary<string, DataFrame> sliceDataFrameByColumn(DataFrame dataFrame, string columnName) {
        Dictionary<string, DataFrame> dictionary = new Dictionary<string, DataFrame>();
        DataFrameColumn dataFrameColumn = dataFrame.getColumn(columnName) ?? throw new ArgumentException($"Column {columnName} not found");

        List<string?> keyList = dataFrameColumn
            .encasement()
            .Select(o => o?.ToString())
            .ToList()!;
        HashSet<string> keyHasSet = keyList
            .Where(s => s != null)
            .ToHashSet()!;
        Dictionary<string, List<int>> keyMap = keyHasSet.ToDictionary(k => k, v => new List<int>());
        long rowsCount = dataFrame.Rows.Count;
        for (int i = 0; i < rowsCount; i++) {
            string? key = keyList[i];
            if (key is null) {
                continue;
            }
            keyMap[key].Add(i);
        }

        List<DataFrameColumn> dataFrameColumns = dataFrame.Columns
            .Where(column => column.Name.Equals(columnName))
            .ToList();

        foreach (DataFrameColumn column in dataFrameColumns) {
            string dataTypeName = column.Name;
            Type columnDataType = column.DataType;

            int rowCount = keyMap[dataTypeName].Count;
            int columnCount = keyMap.Count;

            List<List<object>> values = new List<List<object>>(rowCount);
            for (int i = 0; i < rowsCount; i++) {
                List<object> objects = new List<object>(columnCount);
                for (int j = 0; j < columnCount; j++) {
                    objects.Add(i);
                }
                values.Add(objects);
            }

            List<(string, Type)> columnDefinition = new List<(string, Type)>(columnCount);
            for (int i = 0; i < columnCount; i++) {
                columnDefinition.Add((,columnDataType));
            }
            
            DataFrame df = DataFrame.LoadFrom
            (
                values,
                new List<(string, Type)>() {
                    ("Name", typeof(string)), ("Age", typeof(int)), ("IsActive", typeof(bool))
                }
            );
        }
    }*/

}
