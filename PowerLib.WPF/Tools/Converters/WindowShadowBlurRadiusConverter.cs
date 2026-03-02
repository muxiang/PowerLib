using System;
using System.Globalization;
using System.Windows.Data;

namespace PowerLib.WPF.Tools.Converters;

/// <summary>
/// 将 Window 的 ShadowWidth 转换为 DropShadowEffect 的 BlurRadius，以确保阴影的模糊程度与边框宽度成正比。
/// </summary>
public class WindowShadowBlurRadiusConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value is double d ? d * 1.5 : 0;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}