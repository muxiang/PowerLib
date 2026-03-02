using System;
using System.Globalization;
using System.Windows.Data;

namespace PowerLib.WPF.Tools.Converters;

/// <summary>
/// 将 Window 的 CornerRadius（double）转换为 System.Windows.CornerRadius，以便在 XAML 中直接绑定 CornerRadius 属性。
/// </summary>
public class WindowCornerRadiusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is double d
            ? new System.Windows.CornerRadius(d)
            : new System.Windows.CornerRadius(0);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}