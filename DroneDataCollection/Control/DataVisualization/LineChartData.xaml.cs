using System.Windows.Controls;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

public partial class LineChartData  {

    public LineChartData() {
        InitializeComponent();
    }

    protected override void OnDataFrameChanging(DataFrame? value) {
        
    }

}

