using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using HandyControl.Controls;
using HandyControl.Tools;

namespace DroneDataCollection;

public class ReadOnlyTextPropertyEditor : PropertyEditorBase {

    public override BindingMode GetBindingMode(PropertyDescriptor propertyItem) => BindingMode.OneWay;

    protected override IValueConverter? GetConverter(PropertyDescriptor propertyItem) => App.instance.FindResource("Object2StringConverter") as IValueConverter;

    public override DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        RowDepthPropertyItem rowDepthPropertyItem = new RowDepthPropertyItem();
        FrameworkElement frameworkElement = new System.Windows.Controls.TextBox {
            IsEnabled = true
        };
        BindingOperations.SetBinding
        (
            frameworkElement,
            System.Windows.Controls.TextBox.TextProperty,
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
