using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

[ObservableObject]
public partial class DataFrameGrid {

    [ObservableProperty]
    public DataFrame dataFrame = new DataFrame();

    public DataFrameGrid() {
        InitializeComponent();
        this.DataContextChanged += onDataContextChanged;
    }

    private void onDataContextChanged(object sender, DependencyPropertyChangedEventArgs e) {
        if (e.NewValue is not DataFrame dataFrame) {
            return;
        }
        DataFrame = dataFrame;
    }

    partial void OnDataFrameChanging(DataFrame value) {
        Columns.Clear();

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

            Columns.Add(column);
        }

        ItemsSource = value.Rows;
    }

}

