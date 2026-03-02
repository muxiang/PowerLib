using System;
using System.Drawing;

namespace PowerLib.WinForms;

/// <summary>
/// 提供 <see cref="PowerLib.Primitives.Point"/> 与 <see cref="System.Drawing.Point"/> / <see cref="PointF"/> 之间的转换扩展方法。
/// </summary>
public static class PointExtensions
{
    /// <summary>
    /// 将平台无关的 <see cref="Primitives.Point"/> 转换为 WinForms 的 <see cref="Point"/>（整数坐标）。
    /// </summary>
    /// <param name="point">要转换的共享点。</param>
    /// <returns>对应的 <see cref="Point"/>，坐标经四舍五入取整。</returns>
    /// <remarks>
    /// 由于 <see cref="Point"/> 使用整数坐标，转换会丢失小数部分。
    /// 使用 <see cref="Math.Round(double)"/> 进行四舍五入以减少偏差。
    /// </remarks>
    public static Point ToDrawingPoint(this Primitives.Point point)
    {
        return new Point(
            (int)Math.Round(point.X),
            (int)Math.Round(point.Y));
    }

    /// <summary>
    /// 将平台无关的 <see cref="Primitives.Point"/> 转换为 WinForms 的 <see cref="PointF"/>（浮点坐标）。
    /// </summary>
    /// <param name="point">要转换的共享点。</param>
    /// <returns>对应的 <see cref="PointF"/>。</returns>
    public static PointF ToDrawingPointF(this Primitives.Point point)
    {
        return new PointF((float)point.X, (float)point.Y);
    }

    /// <summary>
    /// 将 WinForms 的 <see cref="Point"/> 转换为平台无关的 <see cref="Primitives.Point"/>。
    /// </summary>
    /// <param name="point">要转换的 WinForms 点。</param>
    /// <returns>对应的共享点。</returns>
    public static Primitives.Point ToSharedPoint(this Point point)
    {
        return new Primitives.Point(point.X, point.Y);
    }

    /// <summary>
    /// 将 WinForms 的 <see cref="PointF"/> 转换为平台无关的 <see cref="Primitives.Point"/>。
    /// </summary>
    /// <param name="point">要转换的 WinForms 浮点点。</param>
    /// <returns>对应的共享点。</returns>
    public static Primitives.Point ToSharedPoint(this PointF point)
    {
        return new Primitives.Point(point.X, point.Y);
    }
}

/// <summary>
/// 提供 <see cref="PowerLib.Primitives.Size"/> 与 <see cref="System.Drawing.Size"/> / <see cref="SizeF"/> 之间的转换扩展方法。
/// </summary>
public static class SizeExtensions
{
    public static Size ToDrawingSize(this Primitives.Size size)
    {
        return new Size(
            (int)Math.Round(size.Width),
            (int)Math.Round(size.Height));
    }

    public static SizeF ToDrawingSizeF(this Primitives.Size size)
    {
        return new SizeF((float)size.Width, (float)size.Height);
    }

    public static Primitives.Size ToSharedSize(this Size size)
    {
        return new Primitives.Size(size.Width, size.Height);
    }

    public static Primitives.Size ToSharedSize(this SizeF size)
    {
        return new Primitives.Size(size.Width, size.Height);
    }
}

/// <summary>
/// 提供 <see cref="PowerLib.Primitives.Rectangle"/> 与 <see cref="System.Drawing.Rectangle"/> / <see cref="RectangleF"/> 之间的转换扩展方法。
/// </summary>
public static class RectangleExtensions
{
    public static Rectangle ToDrawingRectangle(this Primitives.Rectangle rect)
    {
        return new Rectangle(
            (int)Math.Round(rect.X),
            (int)Math.Round(rect.Y),
            (int)Math.Round(rect.Width),
            (int)Math.Round(rect.Height));
    }

    public static RectangleF ToDrawingRectangleF(this Primitives.Rectangle rect)
    {
        return new RectangleF(
            (float)rect.X,
            (float)rect.Y,
            (float)rect.Width,
            (float)rect.Height);
    }

    public static Primitives.Rectangle ToSharedRectangle(this Rectangle rect)
    {
        return new Primitives.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
    }

    public static Primitives.Rectangle ToSharedRectangle(this RectangleF rect)
    {
        return new Primitives.Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
    }
}

/// <summary>
/// 提供 <see cref="PowerLib.Primitives.Color"/> 与 <see cref="System.Drawing.Color"/> 之间的转换扩展方法。
/// </summary>
public static class ColorExtensions
{
    /// <summary>
    /// 将平台无关的 <see cref="Primitives.Color"/> 转换为 WinForms 的 <see cref="Color"/>。
    /// </summary>
    /// <param name="color">要转换的共享颜色。</param>
    /// <returns>对应的 WinForms 颜色。</returns>
    public static Color ToDrawingColor(this Primitives.Color color)
    {
        return Color.FromArgb(color.A, color.R, color.G, color.B);
    }

    /// <summary>
    /// 将 WinForms 的 <see cref="Color"/> 转换为平台无关的 <see cref="Primitives.Color"/>。
    /// </summary>
    /// <param name="color">要转换的 WinForms 颜色。</param>
    /// <returns>对应的共享颜色。</returns>
    public static Primitives.Color ToSharedColor(this Color color)
    {
        return new Primitives.Color(color.A, color.R, color.G, color.B);
    }
}