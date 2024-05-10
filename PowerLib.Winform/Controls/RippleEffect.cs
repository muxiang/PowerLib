using System;
using System.Drawing;
using System.Drawing.Imaging;

using PowerLib.Utilities;

namespace PowerLib.Winform.Controls
{
    /// <summary>
    /// 表示一个2D水波特效
    /// </summary>
    public class RippleEffect : IDisposable
    {
        #region 字段

        // 后台缓冲区
        private int[] _backBuffer;// 浪高
        private readonly Bitmap _bmpFrame;// 动画帧

        // 前台缓冲区
        private int[] _frontBuffer;// 浪高
        private readonly Bitmap _bmpTexture;// 原始材质

        #endregion 字段

        #region 属性

        /// <summary>
        /// 材质
        /// </summary>
        public Bitmap Texture
        {
            get { return _bmpTexture; }
        }

        /// <summary>
        /// 当前动画帧
        /// </summary>
        public Bitmap Frame
        {
            get { return _bmpFrame; }
        }

        #endregion 属性

        #region 构造器

        /// <summary>
        /// 从位图创建水波特效
        /// </summary>
        /// <param name="texture">材质位图</param>
        public RippleEffect(Bitmap texture)
        {
            _bmpTexture = texture ?? throw new ArgumentNullException(nameof(texture));

            // 分配缓冲区
            _frontBuffer = new int[_bmpTexture.Width * _bmpTexture.Height];
            _backBuffer = new int[_frontBuffer.Length];

            _bmpFrame = new Bitmap(_bmpTexture);
        }

        #endregion 构造器

        #region 公开方法

        /// <summary>
        /// 更新缓冲区数据到下一帧动画,随后调用Render渲染生效
        /// </summary>
        public unsafe void Update()
        {
            int w = _bmpTexture.Width;
            int h = _bmpTexture.Height;

            // 固定缓冲区
            fixed (int* pF = _frontBuffer, pB = _backBuffer) 
            {
                // 遍历缓冲区,首尾两行除外
                for (int i = w; i < w * h - w; i++)
                {
                    // 左右两列除外
                    if (i % w == 1 || i % w == w - 1) continue;

                    // 计算浪高
                    pB[i] = ((
                                 pF[i - 1] +
                                 pF[i + 1] +
                                 pF[i - w] +
                                 pF[i + w]) >> 1) - pB[i];

                    // 通过阻力递减
                    pB[i] -= pB[i] >> 5;
                }
            }

            // 交换缓冲区
            int[] tmp = _frontBuffer;
            _frontBuffer = _backBuffer;
            _backBuffer = tmp;
        }

        /// <summary>
        /// 渲染
        /// </summary>
        /// <returns>Bitmap:渲染完成的位图</returns>
        public unsafe Bitmap Render()
        {
            int w = _bmpTexture.Width;
            int h = _bmpTexture.Height;

            // 锁定位图数据
            BitmapData fdat = _bmpFrame.LockBits(new Rectangle(0, 0, _bmpTexture.Width, _bmpTexture.Height),
                                             ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData tdat = _bmpTexture.LockBits(new Rectangle(0, 0, _bmpTexture.Width, _bmpTexture.Height),
                                               ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            // 固定当前帧浪高缓冲区
            fixed (int* buffer = _frontBuffer)
            {
                // 获取位图数据指针
                byte* pFront = (byte*)fdat.Scan0;
                byte* pTexture = (byte*)tdat.Scan0;

                for (int i = w; i < w * h - w; i++)
                {
                    // 计算XY偏移(根据浪高把偏移位置的像素设置到当前位置)
                    int xo = buffer[i - 1] - buffer[i + 1];
                    int yo = buffer[i - w] - buffer[i + w];

                    // 根据偏移生成阴影着色
                    int shade = (xo - yo) / 4;

                    // 获取基准像素的坐标
                    int pxi = i * 3 + (tdat.Stride - w * 3) * (i / (tdat.Stride / 3));
                    int fxi = pxi + xo * 3 + yo * tdat.Stride;
                    if (fxi < 0) fxi = pxi;
                    if (fxi >= w * h * 3) fxi = pxi;

                    // 从材质获取基准像素
                    byte b = pTexture[fxi];
                    byte g = pTexture[fxi + 1];
                    byte r = pTexture[fxi + 2];

                    // 着色
                    b = (byte)CommonUtility.CoerceValue(b + shade, 0, 255);
                    g = (byte)CommonUtility.CoerceValue(g + shade, 0, 255);
                    r = (byte)CommonUtility.CoerceValue(r + shade, 0, 255);

                    // 生成水波
                    pFront[pxi] = b;
                    pFront[pxi + 1] = g;
                    pFront[pxi + 2] = r;
                }

                // 复制首尾行像素
                for (int i = 0; i < w * 3; i++)
                {
                    pFront[w * h * 3 - i - 1] = pTexture[w * h * 3 - i - 1];
                    pFront[i] = pTexture[i];
                }
            }

            // 解锁位图数据
            _bmpFrame.UnlockBits(fdat);
            _bmpTexture.UnlockBits(tdat);

            return _bmpFrame;
        }

        /// <summary>
        /// 在目标区域创建一个水波
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="r">以像素为单位的水波初始半径</param>
        public void Splash(int x, int y, int r)
        {
            // 越界
            if (x < 0 || x >= _bmpTexture.Width || y < 0 || y >= _bmpTexture.Height) return;
            // Temp solution
            if (x < r || x >= _bmpTexture.Width - r || y < r || y >= _bmpTexture.Height - r) return;

            Rectangle effectRect = new Rectangle(0, 0, _bmpTexture.Width, _bmpTexture.Height);

            for (int iy = y - r; iy < y + r; iy++)
            {
                for (int ix = x - r; ix < x + r; ix++)
                {
                    // 根据水波圆心的距离差计算水波浪高
                    double d = Math.Sqrt(Math.Pow(ix - x, 2) + Math.Pow(iy - y, 2));
                    Point p = new Point(ix, iy);
                    if (d < r && effectRect.Contains(p))
                        _frontBuffer[ix + iy * effectRect.Width] = (int)(255 - 256 * 3 * (1 - d / r / 2));
                }
            }
        }

        /// <summary>
        /// 清空缓冲区数据,即平复水面浪高至0
        /// </summary>
        public void Clear()
        {
            if (_bmpTexture == null) return;
            _frontBuffer = new int[_bmpTexture.Width * _bmpTexture.Height];
            _backBuffer = new int[_frontBuffer.Length];
        }

        #endregion 公开方法

        /// <inheritdoc />
        public void Dispose()
        {
            _bmpFrame?.Dispose();
            _bmpTexture?.Dispose();
        }
    }
}
