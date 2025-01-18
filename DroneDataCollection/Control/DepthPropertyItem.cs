using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;
using Org.BouncyCastle.Asn1.X509;

namespace DroneDataCollection;

public class DepthPropertyItem : UserControl {

    public DepthPropertyItem() {
        this.DataContext = this;
    }

    public static readonly DependencyProperty ValueProperty = DependencyProperty.Register
    (
        nameof(Value),
        typeof(object),
        typeof(DepthPropertyItem),
        new PropertyMetadata(default(object))
    );

    public object Value {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly DependencyProperty DisplayNameProperty = DependencyProperty.Register
    (
        nameof(DisplayName),
        typeof(string),
        typeof(DepthPropertyItem),
        new PropertyMetadata(default(string))
    );

    public string DisplayName {
        get => (string)GetValue(DisplayNameProperty);
        set => SetValue(DisplayNameProperty, value);
    }

    public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register
    (
        nameof(PropertyName),
        typeof(string),
        typeof(DepthPropertyItem),
        new PropertyMetadata(default(string))
    );

    public string PropertyName {
        get => (string)GetValue(PropertyNameProperty);
        set => SetValue(PropertyNameProperty, value);
    }

    public static readonly DependencyProperty PropertyDescriptorProperty = DependencyProperty.Register
    (
        nameof(PropertyDescriptor),
        typeof(PropertyDescriptor),
        typeof(DepthPropertyItem),
        new PropertyMetadata(default(Type))
    );

    public PropertyDescriptor PropertyDescriptor {
        get => (PropertyDescriptor)GetValue(PropertyDescriptorProperty);
        set => SetValue(PropertyDescriptorProperty, value);
    }

    public static readonly DependencyProperty EditorElementProperty = DependencyProperty.Register
    (
        nameof(EditorElement),
        typeof(FrameworkElement),
        typeof(DepthPropertyItem),
        new PropertyMetadata(default(FrameworkElement))
    );

    public FrameworkElement EditorElement {
        get => (FrameworkElement)GetValue(EditorElementProperty);
        set => SetValue(EditorElementProperty, value);
    }

}
