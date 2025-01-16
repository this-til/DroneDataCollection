using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Controls;
using MessageBox = System.Windows.MessageBox;

namespace DroneDataCollection;

public class CheckComboBoxPropertyEditor : PropertyEditorBase {

    public override FrameworkElement CreateElement(PropertyItem propertyItem) {
        CheckComboBox checkbox = new CheckComboBox {
            IsEnabled = propertyItem.IsEnabled,
            ShowSelectAllButton = true
        };
        return checkbox;
    }

    public override void CreateBinding(PropertyItem propertyItem, DependencyObject element) {

        BindingOperations.SetBinding
        (
            element,
            ItemsControl.ItemsSourceProperty,
            new Binding($"{propertyItem.PropertyName}.itemsSource") {
                Source = propertyItem.Value,
                Mode = this.GetBindingMode(propertyItem),
                UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyItem),
                Converter = this.GetConverter(propertyItem)
            }
        );

        BindingOperations.SetBinding
        (
            element,
            ListBoxHelper.SelectedItemsProperty,
            new Binding($"{propertyItem.PropertyName}.selectedItems") {
                Source = propertyItem.Value,
                Mode = this.GetBindingMode(propertyItem),
                UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyItem),
                Converter = this.GetConverter(propertyItem)
            }
        );
        if (element is UIElement uiElement) {
            uiElement.UpdateLayout();
        }

    }

    public override DependencyProperty GetDependencyProperty() {
        return ListBox.SelectedItemsProperty;
    }

}

public partial class CheckComboBoxProperty<V> : ObservableObject {

    [ObservableProperty]
    public partial ObservableCollection<V> itemsSource { get; set; } = new ObservableCollection<V>();

    [ObservableProperty]
    public partial ObservableCollection<V> selectedItems { get; set; } = new ObservableCollection<V>();

}
