﻿using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace DroneDataCollection;

public class ListBoxHelper {

    // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty SelectedItemsProperty =
        DependencyProperty.RegisterAttached
        (
            "SelectedItems",
            typeof(IList),
            typeof(ListBoxHelper),
            new FrameworkPropertyMetadata
            (
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnSelectedItemsChanged
            )
        );

    public static IList GetSelectedItems(DependencyObject obj) {
        return (IList)obj.GetValue(SelectedItemsProperty);
    }

    public static void SetSelectedItems(DependencyObject obj, IList value) {
        obj.SetValue(SelectedItemsProperty, value);
    }

    private static void OnlistBoxSelectionChanged(object sender, SelectionChangedEventArgs e) {
        IList dataSource = GetSelectedItems(sender as DependencyObject);
        //添加用户选中的当前项.
        foreach (var item in e.AddedItems) {
            dataSource.Add(item);
        }
        //删除用户取消选中的当前项
        foreach (var item in e.RemovedItems) {
            dataSource.Remove(item);
        }
    }

    private static void OnSelectedItemsChanged(DependencyObject target, DependencyPropertyChangedEventArgs e) {
        if (target is not ListBox listBox || listBox.SelectionMode != SelectionMode.Multiple) {
            return;
        }
        if (e.OldValue != null) {
            listBox.SelectionChanged -= OnlistBoxSelectionChanged;
        }
        IList collection = e.NewValue as IList;
        listBox.SelectedItems.Clear();
        if (collection == null) {
            return;
        }
        foreach (var item in collection) {
            listBox.SelectedItems.Add(item);
        }
        listBox.SelectionChanged += OnlistBoxSelectionChanged;
    }

}
