using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

public partial class ChartData {

    public ChartData() {
        InitializeComponent();
    }

    protected override void OnDataFrameChanging(DataFrame? value) {
        dataGrid.Columns.Clear();

        if (value is null) {
            return;
        }

        foreach (DataFrameColumn dataFrameColumn in value.Columns) {

            DataGridColumn column;

            Binding binding = new Binding($"[{dataFrameColumn.Name}]") {
                /*ConverterParameter = dataFrameColumn.Name,
                Converter = new DataFrameRowValueConverter()*/
            };

            if (dataFrameColumn.DataType.IsEnum) {
                column = new DataGridComboBoxColumn {
                    ItemsSource = Enum.GetValues(dataFrameColumn.DataType).Cast<object>(),
                    SelectedValueBinding = binding
                };
            }

            else if (dataFrameColumn.DataType == typeof(bool)) {
                column = new DataGridCheckBoxColumn {
                    Binding = binding
                };
            }
            else {
                column = new DataGridTextColumn {
                    Binding = binding,
                };
            }
            column.Header = dataFrameColumn.Name;
            column.IsReadOnly = true;

            dataGrid.Columns.Add(column);
        }

        dataGrid.ItemsSource = value.Rows;
    }

}
