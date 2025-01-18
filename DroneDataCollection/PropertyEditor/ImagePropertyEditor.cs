using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HandyControl.Controls;

namespace DroneDataCollection;

public class ImagePropertyEditor : PropertyEditorBase {

    public override DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        RowDepthPropertyItem rowDepthPropertyItem = new RowDepthPropertyItem();
        ImageSelector frameworkElement = new ImageSelector {
            IsEnabled = !propertyDescriptor.IsReadOnly,
            Width = 50,
            Height = 50,
            HorizontalAlignment = HorizontalAlignment.Left
        };
        BindingOperations.SetBinding
        (
            frameworkElement,
            SourceProperty,
            new Binding($"({propertyDescriptor.Name})") {
                Source = value,
                Mode = this.GetBindingMode(propertyDescriptor),
                UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyDescriptor),
                Converter = this.GetConverter(propertyDescriptor)
            }
        );

        BindingOperations.SetBinding
        (
            this,
            UriProperty,
            new Binding(ImageSelector.UriProperty.Name) {
                Source = frameworkElement,
                Mode = BindingMode.OneWay
            }
        );

        rowDepthPropertyItem.EditorElement = frameworkElement;
        return rowDepthPropertyItem;
    }

    internal static readonly DependencyProperty UriProperty = DependencyProperty.Register
    (
        nameof(Uri),
        typeof(Uri),
        typeof(ImagePropertyEditor),
        new PropertyMetadata(null, OnUriChangedCallback)
    );

    private static void OnUriChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e) =>
        ((ImagePropertyEditor)d).Source = e.NewValue is Uri uri
            ? BitmapFrame.Create(uri)
            : null;

    internal Uri Uri {
        get => (Uri)GetValue(UriProperty);
        set => SetValue(UriProperty, value);
    }

    public static readonly DependencyProperty SourceProperty = DependencyProperty.Register
    (
        nameof(Source),
        typeof(ImageSource),
        typeof(ImagePropertyEditor),
        new PropertyMetadata(default(ImageSource))
    );

    public ImageSource? Source {
        get => (ImageSource)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

}
