using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PowerLib.WPF.Tools.Converters;

/// <summary>
/// 将 Window 的 CornerRadius 转换为标题栏的 CornerRadius，以确保标题栏的圆角与窗口的圆角一致。
/// </summary>
public class WindowTitleBarCornerRadiusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is double d
            ? new CornerRadius(d, d, 0, 0)
            : new CornerRadius(0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}