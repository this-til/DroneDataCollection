using System.Windows;
using System.Windows.Data;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

public partial class DataWindow {

    public static readonly DependencyProperty dataFrameProperty = DependencyProperty.Register
    (
        nameof(dataFrame),
        typeof(DataFrame),
        typeof(DataWindow),
        new PropertyMetadata(new DataFrame())
    );

    public DataFrame dataFrame {
        get => (DataFrame)GetValue(dataFrameProperty);
        set => SetValue(dataFrameProperty, value);
    }

    public static readonly DependencyProperty dataElementProperty = DependencyProperty.Register
    (
        nameof(dataElement),
        typeof(DataFrameGrid),
        typeof(DataWindow),
        new PropertyMetadata(null, (o, args) => ((DataWindow)o).OnDataElementChanging(args.NewValue as DataFrameGrid))
    );

    public DataFrameGrid dataElement {
        get => (DataFrameGrid)GetValue(dataElementProperty);
        set => SetValue(dataElementProperty, value);
    }

    protected void OnDataElementChanging(DataFrameGrid? dataFrameGrid) {

        if (dataFrameGrid == null) {
            return;
        }
        BindingOperations.SetBinding
        (
            dataFrameGrid,
            DataFrameGrid.dataFrameProperty,
            new Binding(nameof(dataFrame)) {
                Source = this,
                Mode = BindingMode.TwoWay,
            }
        );

    }

    public DataWindow() {
        InitializeComponent();
        this.DataContext = this;
    }

}
