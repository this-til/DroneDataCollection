using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using HandyControl.Controls;

namespace DroneDataCollection;

public class NumberPropertyEditor : PropertyEditorBase {

    public override RowDepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        RowDepthPropertyItem rowDepthPropertyItem = new RowDepthPropertyItem();
        NumberRangeAttribute? numberRangeAttribute = propertyDescriptor.Attributes[typeof(NumberRangeAttribute)] as NumberRangeAttribute;

        NumericUpDown frameworkElement = new NumericUpDown {
            IsEnabled = !propertyDescriptor.IsReadOnly,
            Maximum = numberRangeAttribute?.max ?? Double.MaxValue,
            Minimum = numberRangeAttribute?.min ?? Double.MinValue,
        };

        BindingOperations.SetBinding
        (
            frameworkElement,
            NumericUpDown.ValueProperty,
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
