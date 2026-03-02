using System.Windows;
using System.Windows.Media;

namespace PowerLib.WPF;

/// <summary>
/// 提供 <see cref="PowerLib.Primitives.Point"/> 与 <see cref="System.Windows.Point"/> 之间的转换扩展方法。
/// </summary>
public static class PointExtensions
{
    /// <summary>
    /// 将平台无关的 <see cref="Primitives.Point"/> 转换为 WPF 的 <see cref="Point"/>。
    /// </summary>
    /// <param name="point">要转换的共享点。</param>
    /// <returns>对应的 WPF 点。</returns>
    public static Point ToWpfPoint(this Primitives.Point point)
    {
        return new Point(point.X, point.Y);
    }

    /// <summary>
    /// 将 WPF 的 <see cref="Point"/> 转换为平台无关的 <see cref="Primitives.Point"/>。
    /// </summary>
    /// <param name="point">要转换的 WPF 点。</param>
    /// <returns>对应的共享点。</returns>
    public static Primitives.Point ToSharedPoint(this Point point)
    {
        return new Primitives.Point(point.X, point.Y);
    }
}

/// <summary>
/// 提供 <see cref="PowerLib.Primitives.Size"/> 与 <see cref="System.Windows.Size"/> 之间的转换扩展方法。
/// </summary>
public static class SizeExtensions
{
    public static Size ToWpfSize(this Primitives.Size size)
    {
        return new Size(size.Width, size.Height);
    }

    public static Primitives.Size ToSharedSize(this Size size)
    {
        return new Primitives.Size(size.Width, size.Height);
    }
}

/// <summary>
/// 提供 <see cref="PowerLib.Primitives.Rectangle"/> 与 <see cref="System.Windows.Rect"/> 之间的转换扩展方法。
/// </summary>
public static class RectangleExtensions
{
    public static Rect ToWpfRect(this Primitives.Rectangle rect)
    {
        return new Rect(rect.X, rect.Y, rect.Width, rect.Height);
    }

    public static Primitives.Rectangle ToSharedRectangle(this Rect rect)
    {
        return new Primitives.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
    }
}

/// <summary>
/// 提供 <see cref="PowerLib.Primitives.Color"/> 与 <see cref="System.Windows.Media.Color"/> 之间的转换扩展方法。
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// 将平台无关的 <see cref="Primitives.Color"/> 转换为 WPF 的 <see cref="Color"/>。
    /// </summary>
    /// <param name="color">要转换的共享颜色。</param>
    /// <returns>对应的 WPF 颜色。</returns>
    public static Color ToWpfColor(this Primitives.Color color)
    {
        return Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    /// <summary>
    /// 将 WPF 的 <see cref="Color"/> 转换为平台无关的 <see cref="Primitives.Color"/>。
    /// </summary>
    /// <param name="color">要转换的 WPF 颜色。</param>
    /// <returns>对应的共享颜色。</returns>
    public static Primitives.Color ToSharedColor(this Color color)
    {
        return new Primitives.Color(color.A, color.R, color.G, color.B);
    }
}