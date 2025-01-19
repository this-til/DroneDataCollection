namespace DroneDataCollection;

using System;
using System.Windows;
using System.Windows.Controls;

public static class ButtonHelper {

    // 定义一个标识符字段（必须是静态的，只读的）
    public static readonly DependencyProperty ClickCommandProperty =
        DependencyProperty.RegisterAttached
        (
            "ClickCommand",
            typeof(RoutedEventHandler),
            typeof(ButtonHelper),
            new PropertyMetadata(OnClickCommandChanged)
        );

    // 获取依赖属性的值
    public static RoutedEventHandler GetClickCommand(DependencyObject obj) {
        return (RoutedEventHandler)obj.GetValue(ClickCommandProperty);
    }

    // 设置依赖属性的值
    public static void SetClickCommand(DependencyObject obj, RoutedEventHandler value) {
        obj.SetValue(ClickCommandProperty, value);
    }

    // 当ClickCommand属性的值更改时的回调方法
    private static void OnClickCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
        if (d is not Button button) {
            return;
        }
        if (e.OldValue is RoutedEventHandler oldCommand) {
            button.Click -= oldCommand;
        }
        if (e.NewValue is RoutedEventHandler newCommand) {
            button.Click += newCommand;
        }

    }

}
