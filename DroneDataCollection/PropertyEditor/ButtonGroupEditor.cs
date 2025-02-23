using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;

namespace DroneDataCollection;

public class ButtonGroupEditor : PropertyEditorBase {

    public override DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        RowDepthPropertyItem rowDepthPropertyItem = new RowDepthPropertyItem();
        
        
        TitleElement.SetTitleWidth(rowDepthPropertyItem.groupBox, new GridLength(0));
        StackPanel stackPanel = new StackPanel();

        stackPanel.Orientation = Orientation.Horizontal;
        
        

        return rowDepthPropertyItem;

    }

}
