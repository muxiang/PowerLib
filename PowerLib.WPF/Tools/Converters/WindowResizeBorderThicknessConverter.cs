using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PowerLib.WPF.Tools.Converters;

/// <summary>
/// 将窗口阴影宽度转换为调整边框的厚度。默认阴影宽度为 0 时，调整边框厚度为 8；当阴影宽度增加时，调整边框厚度也相应增加，以确保用户仍然可以轻松调整窗口大小。
/// </summary>
public class WindowResizeBorderThicknessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not double shadowWidth)
            return new Thickness(8);

        double size = shadowWidth + 8;
        return new Thickness(size);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}