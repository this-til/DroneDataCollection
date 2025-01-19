using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using HandyControl.Controls;
using MySqlX.XDevAPI;

namespace DroneDataCollection;

public abstract class PropertyEditorBase : DependencyObject {

    public abstract DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value);

    public virtual void Stowage(DepthPropertyItem depthPropertyItem, PropertyDescriptor propertyDescriptor, object value) {
        depthPropertyItem.PropertyName = propertyDescriptor.Name;
        depthPropertyItem.DisplayName = propertyDescriptor.DisplayName;
        depthPropertyItem.PropertyDescriptor = propertyDescriptor;

        VisibilityAttribute? visibilityAttribute = propertyDescriptor.Attributes.OfType<VisibilityAttribute>().FirstOrDefault();
        if (visibilityAttribute != null) {
            string propertyName = visibilityAttribute.visibilityBinding;
            propertyName = string.IsNullOrWhiteSpace(propertyName)
                ? propertyDescriptor.Name + "Visibility"
                : propertyName;

            BindingOperations.SetBinding
            (
                depthPropertyItem,
                UIElement.VisibilityProperty,
                new Binding(propertyName) {
                    Source = value,
                    Mode = BindingMode.OneWay,
                    UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyDescriptor),
                    Converter = BoolToVisibilityConverter.instance
                }
            );

        }
    }

    public virtual BindingMode GetBindingMode(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsReadOnly
        ? BindingMode.OneWay
        : BindingMode.TwoWay;

    public virtual UpdateSourceTrigger GetUpdateSourceTrigger(PropertyDescriptor propertyDescriptor) => UpdateSourceTrigger.PropertyChanged;

    protected virtual IValueConverter? GetConverter(PropertyDescriptor propertyDescriptor) => null;

}
