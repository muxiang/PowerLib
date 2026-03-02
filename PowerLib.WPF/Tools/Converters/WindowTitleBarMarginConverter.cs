using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using Window = PowerLib.WPF.Controls.Window;

namespace PowerLib.WPF.Tools.Converters;

/// <summary>
/// 将 <see cref="Window"/> 的 <see cref="Window.ShadowWidth"/> 和 <see cref="Window.BorderWidth"/> 转换为标题栏的 Margin，以确保标题栏不会被阴影或边框遮挡。
/// </summary>
public class WindowTitleBarMarginConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 2)
            return new Thickness(0);

        double shadowWidth = values[0] is double d0 ? d0 : 0;
        double borderWidth = values[1] is double d1 ? d1 : 0;

        double shadowBorderOffset = shadowWidth + borderWidth;

        return new Thickness(shadowBorderOffset);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}