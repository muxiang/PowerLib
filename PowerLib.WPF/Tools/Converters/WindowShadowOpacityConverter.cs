using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

using PowerLib.WPF.Controls;

namespace PowerLib.WPF.Tools.Converters
{
    /// <summary>
    /// 将 <see cref="Window"/> 的 <see cref="Window.ShowShadow"/> 和 <see cref="Window.ShadowOpacity"/> 转换为 DropShadowEffect 的 Opacity，以确保当用户选择不显示阴影时，阴影的透明度为 0，从而隐藏阴影效果；当用户选择显示阴影时，使用用户设置的透明度值来显示阴影效果。
    /// </summary>
    public class WindowShadowOpacityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            const double defaultOpacity = 0.7D;
            double opacity = defaultOpacity;

            if (values.Length != 2)
                return opacity;

            bool showShadow = values[0] is not bool d0 || d0;
            opacity = values[1] is double d1 ? d1 : defaultOpacity;
            return showShadow ? opacity : 0;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
