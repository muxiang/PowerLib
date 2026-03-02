using System.Globalization;

namespace PowerLib.Primitives;

/// <summary>
/// 表示二维坐标系中的一个点，由 X 和 Y 坐标定义。
/// 该结构体是平台无关的，可用于跨 UI 框架共享位置数据。
/// </summary>
public struct Point
{
    /// <summary>
    /// 获取或设置点的 X 坐标。
    /// </summary>
    public double X { get; private set; }

    /// <summary>
    /// 获取或设置点的 Y 坐标。
    /// </summary>
    public double Y { get; private set; }

    /// <summary>
    /// 初始化 <see cref="Point"/> 结构的新实例。
    /// </summary>
    /// <param name="x">X 坐标。</param>
    /// <param name="y">Y 坐标。</param>
    public Point(double x, double y)
    {
        X = x;
        Y = y;
    }

    /// <summary>
    /// 确定指定的对象是否等于当前 <see cref="Point"/>。
    /// </summary>
    /// <param name="obj">要比较的对象。</param>
    /// <returns>如果对象是具有相同 X 和 Y 值的 <see cref="Point"/>，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public override bool Equals(object obj)
    {
        if (!(obj is Point)) return false;
        var p = (Point)obj;
        return X == p.X && Y == p.Y;
    }

    /// <summary>
    /// 返回该 <see cref="Point"/> 的哈希代码。
    /// </summary>
    /// <returns>32 位有符号整数哈希代码。</returns>
    public override int GetHashCode()
    {
        return X.GetHashCode() ^ Y.GetHashCode();
    }

    /// <summary>
    /// 返回表示当前 <see cref="Point"/> 的字符串。
    /// </summary>
    /// <returns>格式为 "{X=[x], Y=[y]}" 的字符串。</returns>
    public override string ToString()
    {
        return string.Format(
            CultureInfo.CurrentCulture,
            "{{X={0}, Y={1}}}",
            X, Y);
    }
}