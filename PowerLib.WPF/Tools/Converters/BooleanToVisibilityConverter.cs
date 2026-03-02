using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PowerLib.WPF.Tools.Converters;

/// <summary>
/// 将布尔值转换为可见性。true 显示，false 隐藏。
/// </summary>
public class BooleanToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value != null && (bool)value ? Visibility.Visible : Visibility.Collapsed;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}