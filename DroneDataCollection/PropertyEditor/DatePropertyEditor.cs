using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using HandyControl.Controls;

namespace DroneDataCollection;

public class DatePropertyEditor : PropertyEditorBase {


    public override DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        RowDepthPropertyItem rowDepthPropertyItem = new RowDepthPropertyItem();
        FrameworkElement frameworkElement = new DateTimePicker {
            IsEnabled = !propertyDescriptor.IsReadOnly
        };

        BindingOperations.SetBinding
        (
            frameworkElement,
            System.Windows.Controls.DatePicker.SelectedDateProperty,
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
