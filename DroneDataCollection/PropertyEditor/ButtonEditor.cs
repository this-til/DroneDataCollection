using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using HandyControl.Controls;

namespace DroneDataCollection;

public class ButtonEditor : PropertyEditorBase { 

    public override DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        RowDepthPropertyItem rowDepthPropertyItem = new RowDepthPropertyItem();
        TitleElement.SetTitleWidth(rowDepthPropertyItem.groupBox, new GridLength(0));
        Button button = new Button();

        button.HorizontalAlignment = HorizontalAlignment.Stretch;
        button.Width = double.NaN; 

        button.Content = propertyDescriptor.DisplayName;
        
        BindingOperations.SetBinding
        (
            button,
            ButtonHelper.ClickCommandProperty,
            new Binding(propertyDescriptor.Name) {
                Source = value,
                Mode = this.GetBindingMode(propertyDescriptor),
                UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyDescriptor),
                Converter = this.GetConverter(propertyDescriptor)
            }
        );

        rowDepthPropertyItem.EditorElement = button;
        return rowDepthPropertyItem;
    }

}
