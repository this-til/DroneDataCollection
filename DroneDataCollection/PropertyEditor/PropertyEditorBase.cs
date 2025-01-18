using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using MySqlX.XDevAPI;

namespace DroneDataCollection;

public abstract class PropertyEditorBase : DependencyObject {

    public abstract DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value);

    public virtual void Stowage(DepthPropertyItem depthPropertyItem, PropertyDescriptor propertyDescriptor) {
        depthPropertyItem.PropertyName = propertyDescriptor.Name;
        depthPropertyItem.DisplayName = propertyDescriptor.DisplayName;
        depthPropertyItem.PropertyDescriptor = propertyDescriptor;
    }

    public virtual BindingMode GetBindingMode(PropertyDescriptor propertyDescriptor) => propertyDescriptor.IsReadOnly
        ? BindingMode.OneWay
        : BindingMode.TwoWay;

    public virtual UpdateSourceTrigger GetUpdateSourceTrigger(PropertyDescriptor propertyDescriptor) => UpdateSourceTrigger.PropertyChanged;

    protected virtual IValueConverter? GetConverter(PropertyDescriptor propertyDescriptor) => null;

}
