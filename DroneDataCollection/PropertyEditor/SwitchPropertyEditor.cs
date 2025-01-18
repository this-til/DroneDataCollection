using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using HandyControl.Controls;
using HandyControl.Tools;

namespace DroneDataCollection;

public class SwitchPropertyEditor : PropertyEditorBase {

    public override DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        RowDepthPropertyItem rowDepthPropertyItem = new RowDepthPropertyItem();
        FrameworkElement frameworkElement = new ToggleButton {
            Style = App.instance.FindResource("ToggleButtonSwitch") as Style,
            HorizontalAlignment = HorizontalAlignment.Left,
            IsEnabled = !propertyDescriptor.IsReadOnly
        };
        BindingOperations.SetBinding
        (
            frameworkElement,
            ToggleButton.IsCheckedProperty,
            new Binding(propertyDescriptor.Name) {
                Source = value,
                Mode = this.GetBindingMode(propertyDescriptor),
                UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyDescriptor),
                Converter = this.GetConverter(propertyDescriptor)
            }
        );

        rowDepthPropertyItem.EditorElement = frameworkElement;
        return rowDepthPropertyItem;
    }

}
