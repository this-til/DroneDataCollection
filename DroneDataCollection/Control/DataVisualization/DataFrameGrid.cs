using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

public abstract partial class DataFrameGrid : UserControl {

    public static readonly DependencyProperty dataFrameProperty = DependencyProperty.Register
    (
        nameof(dataFrame),
        typeof(DataFrame),
        typeof(DataFrameGrid),
        new PropertyMetadata(new DataFrame(), (o, args) => ((DataFrameGrid)o).OnDataFrameChanging(args.NewValue as DataFrame))
    );

    protected DataFrameGrid() {
        this.DataContext = this;
    }

    public DataFrame dataFrame {
        get => (DataFrame)GetValue(dataFrameProperty);
        set => SetValue(dataFrameProperty, value);
    }

    protected abstract void OnDataFrameChanging(DataFrame? value);

}
