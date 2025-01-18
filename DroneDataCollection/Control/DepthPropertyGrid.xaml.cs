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

    public static readonly RoutedEvent SelectedObjectChangedEvent =
        EventManager.RegisterRoutedEvent
        (
            "SelectedObjectChanged",
            RoutingStrategy.Bubble,
            typeof(RoutedPropertyChangedEventHandler<object>),
            typeof(PropertyGrid)
        );

    public event RoutedPropertyChangedEventHandler<object> SelectedObjectChanged {
        add => AddHandler(SelectedObjectChangedEvent, value);
        remove => RemoveHandler(SelectedObjectChangedEvent, value);
    }

    public static readonly DependencyProperty SelectedObjectProperty = DependencyProperty.Register
    (
        nameof(SelectedObject),
        typeof(object),
        typeof(DepthPropertyGrid),
        new PropertyMetadata(null, OnSelectedObjectChanged)
    );

    private static void OnSelectedObjectChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        var ctl = (DepthPropertyGrid)d;
        ctl.OnSelectedObjectChanged(e.OldValue, e.NewValue);
    }

    public object SelectedObject {
        get => GetValue(SelectedObjectProperty);
        set => SetValue(SelectedObjectProperty, value);
    }

    protected virtual void OnSelectedObjectChanged(object oldValue, object newValue) {
        UpdateItems(newValue);
        RaiseEvent(new RoutedPropertyChangedEventArgs<object>(oldValue, newValue, SelectedObjectChangedEvent));
    }

    public static readonly DependencyProperty DescriptionProperty = DependencyProperty.Register
    (
        nameof(Description),
        typeof(string),
        typeof(DepthPropertyGrid),
        new PropertyMetadata(default(string))
    );

    public string Description {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public static readonly DependencyProperty MaxTitleWidthProperty = DependencyProperty.Register
    (
        nameof(MaxTitleWidth),
        typeof(double),
        typeof(DepthPropertyGrid),
        new PropertyMetadata(0d)
    );

    public double MaxTitleWidth {
        get => (double)GetValue(MaxTitleWidthProperty);
        set => SetValue(MaxTitleWidthProperty, value);
    }

    public static readonly DependencyProperty MinTitleWidthProperty = DependencyProperty.Register
    (
        nameof(MinTitleWidth),
        typeof(double),
        typeof(DepthPropertyGrid),
        new PropertyMetadata(0d)
    );

    public double MinTitleWidth {
        get => (double)GetValue(MinTitleWidthProperty);
        set => SetValue(MinTitleWidthProperty, value);
    }

    public override void OnApplyTemplate() {
        base.OnApplyTemplate();
        UpdateItems(SelectedObject);
    }

    private void UpdateItems(object obj) {
        if (obj == null) {
            return;
        }
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
                             propertyEditorBase.Stowage(depthPropertyItem, item);
                             return depthPropertyItem;
                         }
                     )
                     .Where(e => e != null)) {
            stackPanel.Children.Add(propertyItem);
        }
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo) {
        base.OnRenderSizeChanged(sizeInfo);
        TitleElement.SetTitleWidth(this, new GridLength(Math.Max(MinTitleWidth, Math.Min(MaxTitleWidth, ActualWidth / 3))));
    }

}

public static class PropertyItemExtend {

    public static readonly DependencyProperty TitlePlacementProperty = DependencyProperty.Register("TitlePlacement", typeof(TitlePlacementType), typeof(PropertyItem), new PropertyMetadata(TitlePlacementType.Left));

    public static void SetTitlePlacementType(DependencyObject element, TitlePlacementType value) {
        element.SetValue(TitlePlacementProperty, value);
    }

    public static TitlePlacementType GetTitlePlacementType(DependencyObject element) {
        return (TitlePlacementType)element.GetValue(TitlePlacementProperty);
    }

}
