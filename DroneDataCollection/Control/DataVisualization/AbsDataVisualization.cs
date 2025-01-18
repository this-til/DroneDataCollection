using System.Collections.ObjectModel;
using System.Windows.Controls;
using Microsoft.Data.Analysis;

namespace DroneDataCollection;

public abstract class AbsDataVisualization : UserControl {

    public ObservableCollection<DataFrame> dataFrameList { get; set; } = new ObservableCollection<DataFrame>();


    protected AbsDataVisualization() {

        // 创建一个二维数组作为数据源
        List<List<object>> data = new List<List<object>> {
            {
                [
                    "Alice",
                    30,
                    true
                ]
            }, {
                [
                    "Bob",
                    25,
                    false
                ]
            }, {
                [
                    "Charlie",
                    35,
                    true
                ]
            }
        };

        // 创建 DataFrame
        DataFrame? dataFrame = DataFrame.LoadFrom
        (
            data,
            new List<(string, Type)> {
                ("Name", typeof(string)), ("Age", typeof(int)), ("IsActive", typeof(bool))
            }
        );
        dataFrameList.Add(dataFrame);

        this.DataContext = this;
    }

}
