using System.Globalization;

namespace PowerLib.Primitives;

/// <summary>
/// 表示一个由位置（X, Y）和尺寸（Width, Height）定义的矩形区域。
/// 该结构体是平台无关的，可用于跨 UI 框架（如 WinForms、WPF）共享几何数据。
/// </summary>
public struct Rectangle
{
    /// <summary>
    /// 获取或设置矩形左上角的 X 坐标。
    /// </summary>
    /// <value>矩形左上角的 X 坐标，单位为与设备无关的逻辑单位。</value>
    public double X { get; private set; }

    /// <summary>
    /// 获取或设置矩形左上角的 Y 坐标。
    /// </summary>
    /// <value>矩形左上角的 Y 坐标，单位为与设备无关的逻辑单位。</value>
    public double Y { get; private set; }

    /// <summary>
    /// 获取或设置矩形的宽度。
    /// </summary>
    /// <value>矩形的宽度，必须为非负数。</value>
    /// <remarks>
    /// 虽然本结构体不强制验证宽度非负，但在 UI 渲染上下文中，
    /// 负宽度通常被视为无效或未定义行为。
    /// </remarks>
    public double Width { get; private set; }

    /// <summary>
    /// 获取或设置矩形的高度。
    /// </summary>
    /// <value>矩形的高度，必须为非负数。</value>
    /// <remarks>
    /// 虽然本结构体不强制验证高度非负，但在 UI 渲染上下文中，
    /// 负高度通常被视为无效或未定义行为。
    /// </remarks>
    public double Height { get; private set; }

    /// <summary>
    /// 获取矩形的右边界 X 坐标，即 X + Width。
    /// </summary>
    public double Right => X + Width;

    /// <summary>
    /// 获取矩形的下边界 Y 坐标，即 Y + Height。
    /// </summary>
    public double Bottom => Y + Height;

    /// <summary>
    /// 获取矩形左上角的坐标点，等价于 new Point(X, Y)。
    /// </summary>
    public Point Location => new Point(X, Y);

    /// <summary>
    /// 初始化 <see cref="Rectangle"/> 结构的新实例。
    /// </summary>
    /// <param name="x">矩形左上角的 X 坐标。</param>
    /// <param name="y">矩形左上角的 Y 坐标。</param>
    /// <param name="width">矩形的宽度。</param>
    /// <param name="height">矩形的高度。</param>
    public Rectangle(double x, double y, double width, double height)
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    /// <summary>
    /// 初始化 <see cref="Rectangle"/> 结构的新实例，使用指定的位置和尺寸。
    /// </summary>
    /// <param name="location">位置</param>
    /// <param name="size">尺寸</param>
    public Rectangle(Point location, Size size)
    {
        X = location.X;
        Y = location.Y;
        Width = size.Width;
        Height = size.Height;
    }

    /// <summary>
    /// 确定指定的对象是否等于当前 <see cref="Rectangle"/>。
    /// </summary>
    /// <param name="obj">要与当前矩形比较的对象。</param>
    /// <returns>
    /// 如果 <paramref name="obj"/> 是 <see cref="Rectangle"/> 且其 X、Y、Width 和 Height 属性
    /// 与当前实例完全相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。
    /// </returns>
    public override bool Equals(object obj)
    {
        if (!(obj is Rectangle rect)) return false;
        return X == rect.X && Y == rect.Y && Width == rect.Width && Height == rect.Height;
    }

    /// <summary>
    /// 返回该 <see cref="Rectangle"/> 的哈希代码。
    /// </summary>
    /// <returns>32 位有符号整数哈希代码。</returns>
    public override int GetHashCode()
    {
        // 使用简单的异或组合（适用于 .NET 4.0）
        return X.GetHashCode() ^ Y.GetHashCode() ^ Width.GetHashCode() ^ Height.GetHashCode();
    }

    /// <summary>
    /// 返回表示当前 <see cref="Rectangle"/> 的字符串。
    /// </summary>
    /// <returns>
    /// 一个字符串，格式为 "{X=[x], Y=[y], Width=[w], Height=[h]}"，
    /// 其中数值使用当前文化格式化。
    /// </returns>
    public override string ToString()
    {
        return string.Format(
            CultureInfo.CurrentCulture,
            "{{X={0}, Y={1}, Width={2}, Height={3}}}",
            X, Y, Width, Height);
    }
}