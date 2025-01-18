﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using HandyControl.Controls;

namespace DroneDataCollection;

public class EnumPropertyEditor : PropertyEditorBase {

    public override DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        RowDepthPropertyItem rowDepthPropertyItem = new RowDepthPropertyItem();
        FrameworkElement frameworkElement = new System.Windows.Controls.ComboBox {
            IsEnabled = !propertyDescriptor.IsReadOnly,
            ItemsSource = Enum.GetValues(propertyDescriptor.PropertyType)
        };
        BindingOperations.SetBinding
        (
            frameworkElement,
            Selector.SelectedValueProperty,
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
