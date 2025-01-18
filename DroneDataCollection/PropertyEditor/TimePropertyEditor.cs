using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using HandyControl.Controls;

namespace DroneDataCollection;

public class TimePropertyEditor : PropertyEditorBase {

    public override DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        RowDepthPropertyItem rowDepthPropertyItem = new RowDepthPropertyItem();
        FrameworkElement frameworkElement = new DateTimePicker {
            IsEnabled = !propertyDescriptor.IsReadOnly,
        };
        BindingOperations.SetBinding
        (
            frameworkElement,
            TimePicker.SelectedTimeProperty,
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
