using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using static PowerLib.Utilities.CommonUtility;

namespace PowerLib.WPF.Controls;

/// <summary>
/// 表示一个2D水波特效
/// </summary>
internal class RippleEffect
{
    #region 字段

    private int[] _backBuffer;      // 浪高（后台缓冲区）
    private readonly WriteableBitmap _frame;   // 动画帧（可写）

    private int[] _frontBuffer;     // 浪高（前台缓冲区）
    private readonly WriteableBitmap _texture; // 原始材质（只读）

    #endregion

    #region 属性

    /// <summary>
    /// 材质位图（只读）
    /// </summary>
    public WriteableBitmap Texture => _texture;

    /// <summary>
    /// 当前动画帧
    /// </summary>
    public WriteableBitmap Frame => _frame;

    #endregion

    #region 构造器

    /// <summary>
    /// 从 WriteableBitmap 创建水波特效
    /// </summary>
    /// <param name="texture">材质位图（必须为 Pbgra32 格式）</param>
    public RippleEffect(WriteableBitmap texture)
    {
        if (texture == null)
            throw new ArgumentNullException(nameof(texture));

        if (texture.Format != PixelFormats.Pbgra32)
            throw new ArgumentException("仅支持 PixelFormats.Pbgra32 格式的位图。", nameof(texture));

        _texture = texture;
        int pixelCount = _texture.PixelWidth * _texture.PixelHeight;
        _frontBuffer = new int[pixelCount];
        _backBuffer = new int[pixelCount];

        _frame = new WriteableBitmap(
            _texture.PixelWidth,
            _texture.PixelHeight,
            _texture.DpiX,
            _texture.DpiY,
            PixelFormats.Pbgra32,
            null);
    }

    #endregion

    #region 公开方法

    /// <summary>
    /// 更新水波物理模拟到下一帧
    /// </summary>
    public unsafe void Update()
    {
        int w = _texture.PixelWidth;
        int h = _texture.PixelHeight;
        int total = w * h;

        fixed (int* pF = _frontBuffer, pB = _backBuffer)
        {
            for (int i = w; i < total - w; i++)
            {
                // 跳过左右边界（第一列和最后一列）
                if (i % w == 0 || i % w == w - 1)
                    continue;

                // 水波扩散公式
                pB[i] = ((pF[i - 1] + pF[i + 1] + pF[i - w] + pF[i + w]) >> 1) - pB[i];
                // 阻尼衰减
                pB[i] -= pB[i] >> 5;
            }
        }

        // 交换前后缓冲区（兼容 .NET 4.5）
        int[] tmp = _frontBuffer;
        _frontBuffer = _backBuffer;
        _backBuffer = tmp;
    }

    /// <summary>
    /// 渲染当前水波效果到 Frame
    /// </summary>
    public unsafe WriteableBitmap Render()
    {
        int w = _texture.PixelWidth;
        int h = _texture.PixelHeight;
        int total = w * h;
        int stride = _frame.BackBufferStride; // 每行字节数（通常 = w * 4）
        
        _frame.Lock();

        try
        {
            fixed (int* buffer = _frontBuffer)
            {
                byte* pFrame = (byte*)_frame.BackBuffer.ToPointer();
                byte* pTexture = (byte*)_texture.BackBuffer.ToPointer();

                for (int i = w; i < total - w; i++)
                {
                    if (i % w == 0 || i % w == w - 1)
                        continue;

                    int srcY = i / w;
                    int srcX = i % w;

                    // 计算偏移
                    int xo = buffer[i - 1] - buffer[i + 1];
                    int yo = buffer[i - w] - buffer[i + w];
                    int shade = (xo - yo) / 4;

                    // 目标采样点（带边界保护）
                    int dstX = CoerceValue(srcX + xo, 0, w - 1);
                    int dstY = CoerceValue(srcY + yo, 0, h - 1);

                    int srcIndex = srcY * stride + srcX * 4;
                    int dstIndex = dstY * stride + dstX * 4;

                    // 读取材质像素（Pbgra32: B, G, R, A）
                    byte b = pTexture[dstIndex + 0];
                    byte g = pTexture[dstIndex + 1];
                    byte r = pTexture[dstIndex + 2];
                    byte a = pTexture[dstIndex + 3];

                    // 应用阴影
                    b = (byte)CoerceValue(b + shade, 0, 255);
                    g = (byte)CoerceValue(g + shade, 0, 255);
                    r = (byte)CoerceValue(r + shade, 0, 255);

                    // 写入帧缓冲区
                    pFrame[srcIndex + 0] = b;
                    pFrame[srcIndex + 1] = g;
                    pFrame[srcIndex + 2] = r;
                    pFrame[srcIndex + 3] = a;
                }

                // 复制顶部和底部行（防止边缘闪烁）
                for (int x = 0; x < w; x++)
                {
                    // Top row (y=0)
                    int topSrc = x * 4;
                    int topDst = x * 4;
                    pFrame[topDst + 0] = pTexture[topSrc + 0];
                    pFrame[topDst + 1] = pTexture[topSrc + 1];
                    pFrame[topDst + 2] = pTexture[topSrc + 2];
                    pFrame[topDst + 3] = pTexture[topSrc + 3];

                    // Bottom row (y = h-1)
                    int bottomSrc = (h - 1) * stride + x * 4;
                    int bottomDst = (h - 1) * stride + x * 4;
                    pFrame[bottomDst + 0] = pTexture[bottomSrc + 0];
                    pFrame[bottomDst + 1] = pTexture[bottomSrc + 1];
                    pFrame[bottomDst + 2] = pTexture[bottomSrc + 2];
                    pFrame[bottomDst + 3] = pTexture[bottomSrc + 3];
                }

                // 复制左右列（可选，此处简化：由主循环跳过，边界保持原样即可）
                // 实际上主循环已跳过首尾列，所以无需额外处理
            }
                        

            // 标记整个区域需要重绘
            _frame.AddDirtyRect(new Int32Rect(0, 0, w, h));            
        }
        finally
        {
            _frame.Unlock();
        }
        
        return _frame;
    }

    /// <summary>
    /// 在指定位置产生水波
    /// </summary>
    /// <param name="x">X坐标</param>
    /// <param name="y">Y坐标</param>
    /// <param name="radius">水波半径（像素）</param>
    public void Splash(int x, int y, int radius)
    {
        if (x < 0 || x >= _texture.PixelWidth || y < 0 || y >= _texture.PixelHeight)
            return;

        // 避免靠近边缘（简化处理）
        if (x < radius || x >= _texture.PixelWidth - radius ||
            y < radius || y >= _texture.PixelHeight - radius)
            return;

        int w = _texture.PixelWidth;
        int h = _texture.PixelHeight;

        for (int iy = y - radius; iy <= y + radius; iy++)
        {
            for (int ix = x - radius; ix <= x + radius; ix++)
            {
                if (ix < 0 || ix >= w || iy < 0 || iy >= h)
                    continue;

                double dx = ix - x;
                double dy = iy - y;
                double d = Math.Sqrt(dx * dx + dy * dy);
                if (d < radius)
                {
                    int index = ix + iy * w;
                    _frontBuffer[index] = (int)(255 - 256 * 3 * (1 - d / radius / 2));
                }
            }
        }
    }

    /// <summary>
    /// 清空水波（水面恢复平静）
    /// </summary>
    public void Clear()
    {
        Array.Clear(_frontBuffer, 0, _frontBuffer.Length);
        Array.Clear(_backBuffer, 0, _backBuffer.Length);
    }

    #endregion
}