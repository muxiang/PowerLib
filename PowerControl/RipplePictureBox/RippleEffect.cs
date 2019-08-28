using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace PowerControl
{
    /// <summary>
    /// 2D模拟水波特效
    /// </summary>
    public unsafe class RippleEffect : IDisposable
    {
        #region 字段

        // 后台缓冲区
        private int[] _backBuffer;//浪高
        private readonly Bitmap _frame;//动画帧

        // 前台缓冲区
        private int[] _frontBuffer;//浪高
        private readonly Bitmap _texture;//原始材质

        #endregion 字段

        #region 属性

        /// <summary>
        /// 材质
        /// </summary>
        public Bitmap Texture
        {
            get { return _texture; }
        }

        /// <summary>
        /// 当前动画帧
        /// </summary>
        public Bitmap Frame
        {
            get { return _frame; }
        }

        #endregion 属性

        #region 构造器

        /// <summary>
        /// 从位图创建水波特效
        /// </summary>
        /// <param name="texture">材质位图</param>
        public RippleEffect(Bitmap texture)
        {
            _texture = texture ?? throw new ArgumentNullException(nameof(texture));

            //分配缓冲区
            _frontBuffer = new int[_texture.Width * _texture.Height];
            _backBuffer = new int[_frontBuffer.Length];

            _frame = new Bitmap(_texture);
        }

        #endregion 构造器

        #region 公开方法

        /// <summary>
        /// 更新缓冲区数据到下一帧动画,随后调用Render渲染生效
        /// </summary>
        public void Update()
        {
            int w = _texture.Width;
            int h = _texture.Height;

            //固定缓冲区
            fixed (int* pF = _frontBuffer, pB = _backBuffer)
            {
                //遍历缓冲区,首尾两行除外
                for (int i = w; i < w * h - w; i++)
                {
                    //左右两列除外
                    if (i % w == 1 || i % w == w - 1) continue;

                    //计算浪高
                    pB[i] = ((
                                 pF[i - 1] +
                                 pF[i + 1] +
                                 pF[i - w] +
                                 pF[i + w]) >> 1) - pB[i];

                    //通过阻力递减
                    pB[i] -= pB[i] >> 5;
                }
            }

            //交换缓冲区
            int[] tmp = _frontBuffer;
            _frontBuffer = _backBuffer;
            _backBuffer = tmp;
        }

        /// <summary>
        /// 渲染
        /// </summary>
        /// <returns>Bitmap:渲染完成的位图</returns>
        public Bitmap Render()
        {
            int w = _texture.Width;
            int h = _texture.Height;

            //锁定位图数据
            BitmapData fdat = _frame.LockBits(new Rectangle(0, 0, _texture.Width, _texture.Height),
                                             ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            BitmapData tdat = _texture.LockBits(new Rectangle(0, 0, _texture.Width, _texture.Height),
                                               ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            //固定当前帧浪高缓冲区
            fixed (int* buffer = _frontBuffer)
            {
                //获取位图数据指针
                byte* faddr = (byte*)fdat.Scan0;
                byte* taddr = (byte*)tdat.Scan0;

                for (int i = w; i < w * h - w; i++)
                {
                    //计算XY偏移(根据浪高把偏移位置的像素设置到当前位置)
                    int xo = buffer[i - 1] - buffer[i + 1];
                    int yo = buffer[i - w] - buffer[i + w];

                    //根据偏移生成阴影着色
                    int shade = (xo - yo) / 4;

                    //获取基准像素的坐标
                    int pxi = i * 3 + (tdat.Stride - w * 3) * (i / (tdat.Stride / 3));
                    int fxi = pxi + xo * 3 + yo * tdat.Stride;
                    if (fxi < 0) fxi = pxi;
                    if (fxi >= w * h * 3) fxi = pxi;

                    //从材质获取基准像素
                    byte b = taddr[fxi];
                    byte g = taddr[fxi + 1];
                    byte r = taddr[fxi + 2];

                    //着色
                    b = (byte)Utilities.CoerceValue(b + shade, 0, 255);
                    g = (byte)Utilities.CoerceValue(g + shade, 0, 255);
                    r = (byte)Utilities.CoerceValue(r + shade, 0, 255);

                    //生成水波
                    faddr[pxi] = b;
                    faddr[pxi + 1] = g;
                    faddr[pxi + 2] = r;
                }

                //复制首尾行像素
                for (int i = 0; i < w * 3; i++)
                {
                    faddr[w * h * 3 - i - 1] = taddr[w * h * 3 - i - 1];
                    faddr[i] = taddr[i];
                }
            }

            //解锁位图数据
            _frame.UnlockBits(fdat);
            _texture.UnlockBits(tdat);

            return _frame;
        }

        /// <summary>
        /// 在目标区域创建一个水波
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="r">以像素为单位的水波初始半径</param>
        public void Splash(int x, int y, int r)
        {
            //越界
            if (x < 0 || x >= _texture.Width || y < 0 || y >= _texture.Height) return;
            //Temp solution
            if (x < r || x >= _texture.Width - r || y < r || y >= _texture.Height - r) return;

            Rectangle effectRect = new Rectangle(0, 0, _texture.Width, _texture.Height);

            for (int iy = y - r; iy < y + r; iy++)
            {
                for (int ix = x - r; ix < x + r; ix++)
                {
                    //根据水波圆心的距离差计算水波浪高
                    double d = Math.Sqrt(Math.Pow(ix - x, 2) + Math.Pow(iy - y, 2));
                    Point p = new Point(ix, iy);
                    if (d < r && effectRect.Contains(p))
                        _frontBuffer[ix + iy * effectRect.Width] = (int)(255 - (256 * 3 * (1 - d / r / 2)));
                }
            }
        }

        /// <summary>
        /// 清空缓冲区数据,即平复水面浪高至0
        /// </summary>
        public void Clear()
        {
            if (_texture == null) return;
            _frontBuffer = new int[_texture.Width * _texture.Height];
            _backBuffer = new int[_frontBuffer.Length];
        }

        #endregion 公开方法

        public void Dispose()
        {
            _frame?.Dispose();
            _texture?.Dispose();
        }
    }
}
