using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PowerLib.WPF.Controls;

/// <summary>
/// 表示一个2D水波特效
/// </summary>
internal class RippleEffect
{
    #region 字段

    private int[] _backBuffer;                  // 浪高（后台缓冲区）
    private readonly WriteableBitmap _frame;    // 动画帧（可写）
    private readonly int[] _framePixels;         // 在未锁定位图时生成的动画帧

    private int[] _frontBuffer;                 // 浪高（前台缓冲区）
    private readonly BitmapSource _texture;      // 原始材质（只读）
    private readonly int[] _texturePixels;       // 材质的托管内存副本

    private readonly Int32Rect _bitmapRect;
    private readonly int _width;
    private readonly int _height;
    private readonly int _stride;

    private bool _isActive;
    private bool _isFramePending;
    private bool _isFrameRenderRequired;

    #endregion

    #region 属性

    /// <summary>
    /// 材质位图（只读）
    /// </summary>
    public BitmapSource Texture => _texture;

    /// <summary>
    /// 当前动画帧
    /// </summary>
    public WriteableBitmap Frame => _frame;

    /// <summary>
    /// 材质像素高度
    /// </summary>
    public int PixelHeight => _height;

    /// <summary>
    /// 材质像素宽度
    /// </summary>
    public int PixelWidth => _width;

    #endregion

    #region 构造器

    /// <summary>
    /// 从 BitmapSource 创建水波特效
    /// </summary>
    /// <param name="texture">材质位图（必须为 Pbgra32 格式）</param>
    public RippleEffect(BitmapSource texture)
    {
        if (texture == null)
            throw new ArgumentNullException(nameof(texture));

        if (texture.Format != PixelFormats.Pbgra32)
            throw new ArgumentException("仅支持 PixelFormats.Pbgra32 格式的位图。", nameof(texture));

        _texture = texture;
        _width = texture.PixelWidth;
        _height = texture.PixelHeight;
        _stride = _width * 4;
        _bitmapRect = new Int32Rect(0, 0, _width, _height);

        int pixelCount = _width * _height;
        _frontBuffer = new int[pixelCount];
        _backBuffer = new int[pixelCount];
        _texturePixels = new int[pixelCount];
        _framePixels = new int[pixelCount];

        texture.CopyPixels(_bitmapRect, _texturePixels, _stride, 0);
        Array.Copy(_texturePixels, _framePixels, pixelCount);

        _frame = new WriteableBitmap(_width, _height,
            texture.DpiX, texture.DpiY,
            PixelFormats.Pbgra32,
            null);
        _frame.WritePixels(_bitmapRect, _framePixels, _stride, 0);
    }

    #endregion

    #region 公开方法

    /// <summary>
    /// 更新水波物理模拟到下一帧
    /// </summary>
    /// <returns>是否需要渲染或提交动画帧</returns>
    public unsafe bool Update()
    {
        if (!_isActive)
            return _isFramePending;

        bool remainsActive = false;

        fixed (int* pFront = _frontBuffer, pBack = _backBuffer)
        {
            // 按行遍历可以消除热循环中的取模运算。
            for (int y = 1; y < _height - 1; y++)
            {
                int rowStart = y * _width;
                int rowEnd = rowStart + _width - 1;

                for (int i = rowStart + 1; i < rowEnd; i++)
                {
                    int height = ((pFront[i - 1] + pFront[i + 1] + pFront[i - _width] + pFront[i + _width]) >> 1) - pBack[i];
                    height -= height >> 5;
                    pBack[i] = height;
                    remainsActive |= height != 0;
                }
            }
        }

        int[] temp = _frontBuffer;
        _frontBuffer = _backBuffer;
        _backBuffer = temp;
        _isActive = remainsActive;
        _isFrameRenderRequired = true;

        return true;
    }

    /// <summary>
    /// 渲染当前水波效果到 Frame
    /// </summary>
    public unsafe void Render()
    {
        if (_isFrameRenderRequired)
        {
            fixed (int* buffer = _frontBuffer, pFrame = _framePixels, pTexture = _texturePixels)
            {
                // 边框像素始终保持构造时复制的原图，只生成内部区域。
                for (int y = 1; y < _height - 1; y++)
                {
                    int rowStart = y * _width;
                    int rowEnd = rowStart + _width - 1;

                    for (int x = 1, i = rowStart + 1; i < rowEnd; x++, i++)
                    {
                        int offsetX = buffer[i - 1] - buffer[i + 1];
                        int offsetY = buffer[i - _width] - buffer[i + _width];
                        int shade = (offsetX - offsetY) / 4;

                        int sampleX = x + offsetX;
                        int sampleY = y + offsetY;

                        if (sampleX < 0)
                            sampleX = 0;
                        else if (sampleX >= _width)
                            sampleX = _width - 1;

                        if (sampleY < 0)
                            sampleY = 0;
                        else if (sampleY >= _height)
                            sampleY = _height - 1;

                        int color = pTexture[sampleY * _width + sampleX];
                        int alpha = (int)((uint)color >> 24);
                        int blue = (color & 0xFF) + shade;
                        int green = ((color >> 8) & 0xFF) + shade;
                        int red = ((color >> 16) & 0xFF) + shade;

                        // Pbgra32 的颜色分量不能大于 Alpha。
                        if (blue < 0)
                            blue = 0;
                        else if (blue > alpha)
                            blue = alpha;

                        if (green < 0)
                            green = 0;
                        else if (green > alpha)
                            green = alpha;

                        if (red < 0)
                            red = 0;
                        else if (red > alpha)
                            red = alpha;

                        pFrame[i] = (alpha << 24) | (red << 16) | (green << 8) | blue;
                    }
                }
            }

            _isFrameRenderRequired = false;
            _isFramePending = true;
        }

        // Lock 可能等待 WPF 渲染线程。拿不到锁时丢弃本次提交，优先保证 UI 响应，
        // 下一个合成帧会继续尝试提交托管缓冲区中的最新结果。
        if (!_isFramePending || !_frame.TryLock(new Duration(TimeSpan.Zero))) 
            return;

        try
        {
            Marshal.Copy(_framePixels, 0, _frame.BackBuffer, _framePixels.Length);
            _frame.AddDirtyRect(_bitmapRect);
            _isFramePending = false;
        }
        finally
        {
            _frame.Unlock();
        }
    }

    /// <summary>
    /// 在指定位置产生水波
    /// </summary>
    /// <param name="x">X坐标</param>
    /// <param name="y">Y坐标</param>
    /// <param name="radius">水波半径（像素）</param>
    public void Splash(int x, int y, int radius)
    {
        if (radius <= 0 || x < radius || x >= _width - radius || y < radius || y >= _height - radius)
            return;

        int radiusSquared = radius * radius;

        for (int iy = y - radius; iy <= y + radius; iy++)
        {
            int offsetY = iy - y;
            int rowStart = iy * _width;

            for (int ix = x - radius; ix <= x + radius; ix++)
            {
                int offsetX = ix - x;
                int distanceSquared = offsetX * offsetX + offsetY * offsetY;

                if (distanceSquared < radiusSquared)
                {
                    double distance = Math.Sqrt(distanceSquared);
                    _frontBuffer[rowStart + ix] = (int)(255 - 256 * 3 * (1 - distance / radius / 2));
                    _isActive = true;
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
        Array.Copy(_texturePixels, _framePixels, _texturePixels.Length);

        _isActive = false;
        _isFramePending = false;
        _isFrameRenderRequired = false;
        _frame.WritePixels(_bitmapRect, _framePixels, _stride, 0);
    }

    #endregion
}
