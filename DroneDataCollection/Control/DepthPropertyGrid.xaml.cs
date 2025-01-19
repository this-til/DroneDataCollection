using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using HandyControl.Controls;
using HandyControl.Data;
using HandyControl.Interactivity;
using HandyControl.Tools.Extension;

namespace DroneDataCollection;

public partial class DepthPropertyGrid {

    public DepthPropertyGrid() {
        InitializeComponent();
    }

    public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register
    (
        nameof(SelectedObject),
        typeof(object),
        typeof(DepthPropertyGrid),
        new PropertyMetadata
        (
            null,
            (d, e) => ((DepthPropertyGrid)d).UpdateItems(e.NewValue)
        )
    );

    public object SelectedObject {
        get => GetValue(SelectedObjectProperty);
        set => SetValue(SelectedObjectProperty, value);
    }

    /*public Type BasicsType {
        get => (Type)GetValue(BasicsTypeProperty);
        set => SetValue(BasicsTypeProperty, value);
    }

    public static readonly DependencyProperty BasicsTypeProperty = DependencyProperty.Register
    (
        nameof(BasicsType),
        typeof(Type),
        typeof(DepthPropertyGrid),
        new PropertyMetadata(null)
    );

    public Type SelectedType {
        get => (Type)GetValue(BasicsTypeProperty);
        set => SetValue(BasicsTypeProperty, value);
    }

    public static readonly DependencyProperty SelectedTypeProperty = DependencyProperty.Register
    (
        nameof(SelectedType),
        typeof(Type),
        typeof(DepthPropertyGrid),
        new PropertyMetadata(null)
    );*/

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();
        UpdateItems(SelectedObject);
    }

    private void UpdateItems(object obj) {
        stackPanel.Children.Clear();

        foreach (DepthPropertyItem propertyItem in TypeDescriptor.GetProperties(obj.GetType())
                     .OfType<PropertyDescriptor>()
                     .Where(item => item.IsBrowsable)
                     .Where(item => item.Attributes[typeof(PropertyEditorAttribute)] != null)
                     .Select
                     (
                         item => {
                             PropertyEditorAttribute propertyEditorAttribute = (PropertyEditorAttribute)item.Attributes[typeof(PropertyEditorAttribute)]!;
                             PropertyEditorBase? propertyEditorBase = App.instance.propertyEditorService.getPropertyEditor(propertyEditorAttribute.editorType);
                             if (propertyEditorBase == null) {
                                 return null!;
                             }
                             DepthPropertyItem depthPropertyItem = propertyEditorBase.CreateElement(item, obj);
                             propertyEditorBase.Stowage(depthPropertyItem, item, obj);
                             return depthPropertyItem;
                         }
                     )
                     .Where(e => e != null)) {
            stackPanel.Children.Add(propertyItem);
        }
    }

}
