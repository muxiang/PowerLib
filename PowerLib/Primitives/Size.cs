using System.Globalization;

namespace PowerLib.Primitives;

/// <summary>
/// 表示由宽度和高度定义的二维尺寸。
/// 该结构体是平台无关的，适用于跨 UI 框架共享大小信息。
/// </summary>
public struct Size
{
    /// <summary>
    /// 获取或设置宽度。
    /// </summary>
    /// <value>宽度，通常应为非负数。</value>
    public double Width { get; private set; }

    /// <summary>
    /// 获取或设置高度。
    /// </summary>
    /// <value>高度，通常应为非负数。</value>
    public double Height { get; private set; }

    /// <summary>
    /// 初始化 <see cref="Size"/> 结构的新实例。
    /// </summary>
    /// <param name="width">宽度。</param>
    /// <param name="height">高度。</param>
    public Size(double width, double height)
    {
        Width = width;
        Height = height;
    }

    /// <summary>
    /// 确定指定的对象是否等于当前 <see cref="Size"/>。
    /// </summary>
    /// <param name="obj">要比较的对象。</param>
    /// <returns>如果对象是具有相同 Width 和 Height 值的 <see cref="Size"/>，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public override bool Equals(object obj)
    {
        if (!(obj is Size)) return false;
        var s = (Size)obj;
        return Width == s.Width && Height == s.Height;
    }

    /// <summary>
    /// 返回该 <see cref="Size"/> 的哈希代码。
    /// </summary>
    /// <returns>32 位有符号整数哈希代码。</returns>
    public override int GetHashCode()
    {
        return Width.GetHashCode() ^ Height.GetHashCode();
    }

    /// <summary>
    /// 返回表示当前 <see cref="Size"/> 的字符串。
    /// </summary>
    /// <returns>格式为 "{Width=[w], Height=[h]}" 的字符串。</returns>
    public override string ToString()
    {
        return string.Format(
            CultureInfo.CurrentCulture,
            "{{Width={0}, Height={1}}}",
            Width, Height);
    }
}