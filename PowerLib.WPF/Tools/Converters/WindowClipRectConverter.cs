using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PowerLib.WPF.Tools.Converters;

/// <summary>
/// 将 Window 的 ActualWidth、ActualHeight、ShadowWidth 和 BorderWidth 转换为一个 Rect，用于设置窗口内容的 Clip 属性，以确保内容不会超过边界。
/// </summary>
public class WindowClipRectConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        double actualWidth = values[0] is double w ? w : 0;
        double actualHeight = values[1] is double h ? h : 0;
        double shadowWidth = values[2] is double sw ? sw : 0;
        double borderWidth = values[3] is double bw ? bw : 0;

        if (actualWidth <= 0 || actualHeight <= 0)
            return Rect.Empty;

        double offset = shadowWidth + borderWidth;
        return new Rect(0, 0, actualWidth - 2 * offset, actualHeight - 2 * offset);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}