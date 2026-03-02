using System.Windows;
using System.Windows.Media;

namespace PowerLib.WPF.Tools;

public static class DpiHelper
{
    public static double GetDpiScaleX(Window window)
    {
        PresentationSource source = PresentationSource.FromVisual(window);
        if (source?.CompositionTarget == null)
            return 1.0; // 默认 96 DPI（100% 缩放）

        Matrix m = source.CompositionTarget.TransformToDevice;
        return m.M11; // 水平缩放因子（通常等于 M22）
    }

    public static double GetDpiScaleY(Window window)
    {
        PresentationSource source = PresentationSource.FromVisual(window);
        if (source?.CompositionTarget == null)
            return 1.0;

        Matrix m = source.CompositionTarget.TransformToDevice;
        return m.M22;
    }
}