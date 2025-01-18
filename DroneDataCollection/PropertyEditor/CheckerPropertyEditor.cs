using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using HandyControl.Controls;

namespace DroneDataCollection;

public class CheckComboBoxPropertyEditor : PropertyEditorBase {

    public override DepthPropertyItem CreateElement(PropertyDescriptor propertyDescriptor, object value) {
        RowDepthPropertyItem rowDepthPropertyItem = new RowDepthPropertyItem();
        CheckComboBox checkbox = new CheckComboBox {
            IsEnabled = !propertyDescriptor.IsReadOnly,
            ShowSelectAllButton = true,
            SelectionMode = SelectionMode.Multiple
        };
        BindingOperations.SetBinding
        (
            checkbox,
            ItemsControl.ItemsSourceProperty,
            new Binding($"{propertyDescriptor.Name}.itemsSource") {
                Source = value,
                Mode = this.GetBindingMode(propertyDescriptor),
                UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyDescriptor),
                Converter = this.GetConverter(propertyDescriptor)
            }
        );

        BindingOperations.SetBinding
        (
            checkbox,
            ListBoxHelper.SelectedItemsProperty,
            new Binding($"{propertyDescriptor.Name}.selectedItems") {
                Source = value,
                Mode = this.GetBindingMode(propertyDescriptor),
                UpdateSourceTrigger = this.GetUpdateSourceTrigger(propertyDescriptor),
                Converter = this.GetConverter(propertyDescriptor)
            }
        );

        rowDepthPropertyItem.EditorElement = checkbox;
        return rowDepthPropertyItem;
    }
    

}

public partial class CheckComboBoxProperty<V> : ObservableObject {

    [ObservableProperty]
    public partial ObservableCollection<V> itemsSource { get; set; } = new ObservableCollection<V>();

    [ObservableProperty]
    public partial ObservableCollection<V> selectedItems { get; set; } = new ObservableCollection<V>();

}
