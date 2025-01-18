using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using HandyControl.Controls;

namespace DroneDataCollection;

public class ObjectPropertyEditor : PropertyEditorBase {

    public override DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        DepthPropertyGroupItem propertyItem = new DepthPropertyGroupItem();
        FrameworkElement frameworkElement = new DepthPropertyGrid() {
            IsEnabled = !propertyDescriptor.IsReadOnly,
        };
        BindingOperations.SetBinding
        (
            frameworkElement,
            DepthPropertyGrid.SelectedObjectProperty,
            new Binding(propertyDescriptor.Name) {
                Source = value,
                Mode = this.GetBindingMode(propertyDescriptor),
                UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyDescriptor),
                Converter = this.GetConverter(propertyDescriptor)
            }
        );

        propertyItem.EditorElement = frameworkElement;
        return propertyItem;
    }

}
