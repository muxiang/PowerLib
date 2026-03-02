using System;
using System.Globalization;

namespace PowerLib.Primitives;

/// <summary>
/// 表示一种 RGBA 颜色，包含 Alpha（透明度）、Red、Green 和 Blue 四个 8 位通道。
/// 该结构体是平台无关的，可用于跨 UI 框架共享颜色数据。
/// </summary>
/// <remarks>
/// 所有通道值范围为 0 到 255，其中 Alpha = 255 表示完全不透明，Alpha = 0 表示完全透明。
/// </remarks>
public struct Color : IEquatable<Color>
{
    /// <summary>
    /// 获取 Alpha 通道值（透明度）。
    /// </summary>
    public byte A { get; private set; }

    /// <summary>
    /// 获取 Red 通道值。
    /// </summary>
    public byte R { get; private set; }

    /// <summary>
    /// 获取 Green 通道值。
    /// </summary>
    public byte G { get; private set; }

    /// <summary>
    /// 获取 Blue 通道值。
    /// </summary>
    public byte B { get; private set; }

    /// <summary>
    /// 初始化 <see cref="Color"/> 结构的新实例。
    /// </summary>
    /// <param name="a">Alpha 通道（0=透明，255=不透明）。</param>
    /// <param name="r">Red 通道（0–255）。</param>
    /// <param name="g">Green 通道（0–255）。</param>
    /// <param name="b">Blue 通道（0–255）。</param>
    public Color(byte a, byte r, byte g, byte b)
    {
        A = a;
        R = r;
        G = g;
        B = b;
    }

    /// <summary>
    /// 初始化不透明的 <see cref="Color"/>（Alpha = 255）。
    /// </summary>
    /// <param name="r">Red 通道。</param>
    /// <param name="g">Green 通道。</param>
    /// <param name="b">Blue 通道。</param>
    public Color(byte r, byte g, byte b) : this(255, r, g, b) { }

    /// <summary>
    /// 从 32 位 ARGB 值创建 <see cref="Color"/>。
    /// </summary>
    /// <param name="argb">32 位整数，格式为 0xAARRGGBB。</param>
    /// <returns>对应的颜色。</returns>
    public static Color FromArgb(int argb)
    {
        return new Color(
            (byte)((argb >> 24) & 0xFF),
            (byte)((argb >> 16) & 0xFF),
            (byte)((argb >> 8) & 0xFF),
            (byte)(argb & 0xFF)
        );
    }

    /// <summary>
    /// 从单独的 Alpha、Red、Green 和 Blue 通道值创建 <see cref="Color"/>。
    /// </summary>
    /// <param name="a">a</param>
    /// <param name="r">r</param>
    /// <param name="g">g</param>
    /// <param name="b">b</param>
    /// <returns></returns>
    public static Color FromArgb(byte a, byte r, byte g, byte b)
    {
        return new Color(a, r, g, b);
    }

    /// <summary>
    /// 从单独的 Alpha、Red、Green 和 Blue 通道值创建 <see cref="Color"/>，并确保通道值在有效范围内（0–255）。
    /// </summary>
    /// <param name="a">a</param>
    /// <param name="r">r</param>
    /// <param name="g">g</param>
    /// <param name="b">b</param>
    /// <returns></returns>
    public static Color FromArgb(int a, int r, int g, int b)
    {
        return new Color(
            (byte)PowerLib.Utilities.CommonUtility.CoerceValue(a, 0, 255),
            (byte)PowerLib.Utilities.CommonUtility.CoerceValue(r, 0, 255),
            (byte)PowerLib.Utilities.CommonUtility.CoerceValue(g, 0, 255),
            (byte)PowerLib.Utilities.CommonUtility.CoerceValue(b, 0, 255)
        );
    }

    /// <summary>
    /// 从单独的 Red、Green 和 Blue 通道值创建不透明的 <see cref="Color"/>（Alpha = 255）。
    /// </summary>
    /// <param name="r">r</param>
    /// <param name="g">g</param>
    /// <param name="b">b</param>
    /// <returns></returns>
    public static Color FromArgb(byte r, byte g, byte b)
    {
        return new Color(255, r, g, b);
    }

    /// <summary>
    /// 从单独的 Red、Green 和 Blue 通道值创建不透明的 <see cref="Color"/>（Alpha = 255），并确保通道值在有效范围内（0–255）。
    /// </summary>
    /// <param name="r">r</param>
    /// <param name="g">g</param>
    /// <param name="b">b</param>
    /// <returns></returns>
    public static Color FromArgb(int r, int g, int b)
    {
        return new Color(
            255,
            (byte)PowerLib.Utilities.CommonUtility.CoerceValue(r, 0, 255),
            (byte)PowerLib.Utilities.CommonUtility.CoerceValue(g, 0, 255),
            (byte)PowerLib.Utilities.CommonUtility.CoerceValue(b, 0, 255)
        );
    }

    /// <summary>
    /// 获取该颜色的 32 位 ARGB 值（格式：0xAARRGGBB）。
    /// </summary>
    /// <returns>表示此颜色的整数。</returns>
    public int ToArgb()
    {
        return (A << 24) | (R << 16) | (G << 8) | B;
    }

    /// <summary>
    /// 确定两个 <see cref="Color"/> 实例是否相等。
    /// </summary>
    /// <param name="left">第一个颜色。</param>
    /// <param name="right">第二个颜色。</param>
    /// <returns>如果所有通道值相等，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool operator ==(Color left, Color right)
    {
        return left.A == right.A && left.R == right.R && left.G == right.G && left.B == right.B;
    }

    /// <summary>
    /// 确定两个 <see cref="Color"/> 实例是否不相等。
    /// </summary>
    /// <param name="left">第一个颜色。</param>
    /// <param name="right">第二个颜色。</param>
    /// <returns>如果任意通道值不等，则为 <see langword="true"/>；否则为 <see langword="false"/>。</returns>
    public static bool operator !=(Color left, Color right)
    {
        return !(left == right);
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return obj is Color other && Equals(other);
    }

    /// <inheritdoc/>
    public bool Equals(Color other)
    {
        return A == other.A && R == other.R && G == other.G && B == other.B;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return ToArgb();
    }

    /// <inheritdoc/>
    public override string ToString()
    {
        return string.Format(
            CultureInfo.InvariantCulture,
            "#{0:X2}{1:X2}{2:X2}{3:X2}",
            A, R, G, B);
    }

    // 预定义常用颜色（可选）
    /// <summary>
    /// 获取黑色（#FF000000）。
    /// </summary>
    public static Color Black => new Color(255, 0, 0, 0);

    /// <summary>
    /// 获取白色（#FFFFFFFF）。
    /// </summary>
    public static Color White => new Color(255, 255, 255, 255);

    /// <summary>
    /// 获取透明色（#00000000）。
    /// </summary>
    public static Color Transparent => new Color(0, 0, 0, 0);
}