using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

using Window = PowerLib.WPF.Controls.Window;

namespace PowerLib.WPF.Tools.Converters;

/// <summary>
/// 将 <see cref="Window"/> 的 <see cref="Window.ShadowWidth"/> 、 <see cref="Window.BorderWidth"/> 、<see cref="Window.ShowTitleBar"/> 和 <see cref="Window.TitleBarHeight"/>
/// 转换为 ContentPresenter 的 Margin，以确保内容不会被阴影、边框或标题栏遮挡。
/// </summary>
public class WindowContentPresenterMarginConverter : IMultiValueConverter
{
    public const double ADDITIONAL_OFFSET = 8; // 额外的偏移量，确保内容不会紧贴边框或阴影

    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length != 4)
            return new Thickness(0);

        double shadowWidth = values[0] is double d0 ? d0 : 0;
        double borderWidth = values[1] is double d1 ? d1 : 0;
        bool showTitleBar = values[2] is not bool d2 || d2;
        double titleBarHeight = values[3] is double d3 ? d3 : 0;

        double shadowBorderOffset = shadowWidth + borderWidth + ADDITIONAL_OFFSET;

        return new Thickness(shadowBorderOffset, shadowBorderOffset + (showTitleBar ? titleBarHeight : 0), shadowBorderOffset, shadowBorderOffset);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}