using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace PowerLib.Utilities
{
    /// <summary>
    /// 封装位图处理
    /// </summary>
    public static class BitmapUtility
    {
        /// <summary>
        /// 从位图计算图像路径
        /// </summary>
        /// <param name="bmp">位图</param>
        /// <param name="scanCallback">扫描回调，回传当前扫描的像素索引</param>
        /// <returns></returns>
        public static unsafe GraphicsPath GetGraphicsPathFromBitmap(Bitmap bmp, Action<int> scanCallback = null)
        {
            if (bmp.PixelFormat != PixelFormat.Format32bppArgb || bmp.PixelFormat != PixelFormat.Format32bppPArgb)
                throw new InvalidOperationException("位图不是有效的32位ARGB位图");

            int w = bmp.Width;
            int h = bmp.Height;

            GraphicsPath path = new GraphicsPath();
            BitmapData bmpData = null;

            try
            {
                bmpData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                uint* pData = (uint*)bmpData.Scan0;
                int stride = Math.Abs(bmpData.Stride);

                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        if ((*pData & 0xFF000000) != 0)
                            path.AddRectangle(new Rectangle(i, j, 1, 1));

                        scanCallback?.Invoke(i + j * w);
                        pData++;
                    }

                    if (stride > w)
                        pData += stride - w;
                }
            }
            finally
            {
                if (bmpData != null)
                    bmp.UnlockBits(bmpData);
            }

            return path;
        }

        /// <summary>
        /// 按指定大小缩放一个位图
        /// </summary>
        /// <param name="bmpOrigin">原始位图</param>
        /// <param name="size">大小</param>
        /// <returns></returns>
        public static Bitmap StretchBitmap(Bitmap bmpOrigin, Size size)
        {
            Bitmap bmpResult = new Bitmap(size.Width, size.Height);

            using (Graphics g = Graphics.FromImage(bmpResult))
                g.DrawImage(bmpOrigin, new Rectangle(Point.Empty, size));

            return bmpResult;
        }

        /// <summary>
        /// 创建半透明位图
        /// </summary>
        /// <param name="alpha">透明度(0-255)</param>
        /// <param name="baseColor">基础颜色</param>
        /// <param name="size">大小</param>
        /// <returns>结果位图</returns>
        public static Bitmap CreateTranslucentBitmap(byte alpha, Color baseColor, Size size)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);

            Color clr = Color.FromArgb(alpha, baseColor);

            using (Graphics g = Graphics.FromImage(bmp))
                g.Clear(clr);

            return bmp;
        }

        /// <summary>
        /// 缩放位图到指定大小
        /// </summary>
        /// <param name="bmpOrg">原始位图</param>
        /// <param name="sz">大小</param>
        /// <param name="disposeOriginal">是否自动释放原始位图</param>
        /// <returns>结果位图</returns>
        public static Bitmap StretchBitmap(Bitmap bmpOrg, Size sz, bool disposeOriginal)
        {
            Bitmap newBitmap = new Bitmap(sz.Width, sz.Height);

            using (Graphics g = Graphics.FromImage(newBitmap))
                g.DrawImage(bmpOrg, new Rectangle(new Point(0, 0), sz));

            if (disposeOriginal)
                bmpOrg.Dispose();

            return newBitmap;
        }

        /// <summary>
        /// 以指定角度旋转位图
        /// </summary>
        /// <param name="bmpOrg">原始位图</param>
        /// <param name="angle">角度：90/180/270</param>
        /// <param name="disposeOriginal">是否自动释放原始位图</param>
        /// <returns>结果位图</returns>
        public static Bitmap RotateBitmap(Bitmap bmpOrg, int angle, bool disposeOriginal = false)
        {
            if (angle != 90 && angle != 180 && angle != 270)
                throw new ArgumentOutOfRangeException(nameof(angle));

            int width = bmpOrg.Width;
            int height = bmpOrg.Height;
            Size szDraw = new Size(bmpOrg.Width, bmpOrg.Height);

            if (angle == 90 || angle == 270)
            {
                width = bmpOrg.Height;
                height = bmpOrg.Width;
            }

            Bitmap newBitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(newBitmap))
            {
                g.TranslateTransform(width / 2F, height / 2F);
                g.RotateTransform(angle);
                g.TranslateTransform(-bmpOrg.Width / 2F, -bmpOrg.Height / 2F);
                g.DrawImage(bmpOrg, new Rectangle(Point.Empty, szDraw));
            }

            if (disposeOriginal)
                bmpOrg.Dispose();

            return newBitmap;
        }

        /// <summary>
        /// 使用指定矩形剪裁位图
        /// </summary>
        /// <param name="bmpOrg">原始位图</param>
        /// <param name="rect">矩形</param>
        /// <param name="disposeOriginal">是否自动释放原始位图</param>
        /// <returns>结果位图</returns>
        public static Bitmap CutBitmap(Bitmap bmpOrg, Rectangle rect, bool disposeOriginal = true)
        {
            Bitmap newBitmap = new Bitmap(rect.Width, rect.Height);

            using (Graphics g = Graphics.FromImage(newBitmap))
                g.DrawImage(bmpOrg, new Rectangle(0, 0, rect.Width, rect.Height), rect, GraphicsUnit.Pixel);

            if (disposeOriginal)
                bmpOrg.Dispose();

            return newBitmap;
        }

        /// <summary>
        /// 对图像进行马赛克处理
        /// </summary>
        /// <param name="bmpOrigin">图像</param>
        /// <param name="radius">马赛克单位半径，值越大越模糊</param>
        /// <returns></returns>
        public static unsafe Bitmap Mosaic(Bitmap bmpOrigin, int radius)
        {
            if (bmpOrigin == null)
                throw new ArgumentNullException(nameof(bmpOrigin));

            if (radius < 1)
                throw new ArgumentOutOfRangeException(nameof(radius), "模糊半径不能小于1");

            int w = bmpOrigin.Width;
            int h = bmpOrigin.Height;

            Bitmap bmpResult = new Bitmap(w, h);

            BitmapData bmpDataOrg = null;
            BitmapData bmpDataResult = null;

            try
            {
                bmpDataOrg = bmpOrigin.LockBits(new Rectangle(0, 0, w, h),
                    ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                bmpDataResult = bmpResult.LockBits(new Rectangle(0, 0, w, h),
                    ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

                uint* pDataOrg = (uint*)bmpDataOrg.Scan0;
                uint* pDataResult = (uint*)bmpDataResult.Scan0;
                int stride = Math.Abs(bmpDataOrg.Stride);

                //  纵向
                for (int y = radius; y < h + radius; y += radius * 2 + 1)
                {
                    // 横向
                    for (int x = radius; x < w + radius; x += radius * 2 + 1)
                    {
                        ulong sumA = 0;
                        ulong sumR = 0;
                        ulong sumG = 0;
                        ulong sumB = 0;
                        ulong pixelCount = 0;

                        for (int y1 = y - radius; y1 < y + radius + 1; ++y1)
                        {
                            if (y1 >= h)
                                break;

                            for (int x1 = x - radius; x1 < x + radius + 1; ++x1)
                            {
                                if (x1 >= w)
                                    break;

                                uint c = pDataOrg[y1 * stride / 4 + x1];
                                byte a = (byte)(c >> 24);
                                byte r = (byte)(c >> 16 & 0xFF);
                                byte g = (byte)(c >> 8 & 0xFF);
                                byte b = (byte)(c & 0xFF);

                                sumA += a;
                                sumR += r;
                                sumG += g;
                                sumB += b;
                                ++pixelCount;
                            }
                        }

                        // 均值
                        byte avgA = (byte)(sumA / pixelCount);
                        byte avgR = (byte)(sumR / pixelCount);
                        byte avgG = (byte)(sumG / pixelCount);
                        byte avgB = (byte)(sumB / pixelCount);

                        for (int y1 = y - radius; y1 < y + radius + 1; ++y1)
                        {
                            if (y1 >= h)
                                break;

                            for (int x1 = x - radius; x1 < x + radius + 1; ++x1)
                            {
                                if (x1 >= w)
                                    break;

                                uint c = ((uint)avgA << 24) | ((uint)avgR << 16) | ((uint)avgG << 8) | avgB;
                                pDataResult[y1 * stride / 4 + x1] = c;
                            }
                        }
                    }

                    if (stride / 4 <= w)
                        continue;

                    pDataOrg += stride / 4 - w;
                    pDataResult += stride / 4 - w;
                }
            }
            finally
            {
                if (bmpDataOrg != null)
                    bmpOrigin.UnlockBits(bmpDataOrg);
                if (bmpDataResult != null)
                    bmpResult.UnlockBits(bmpDataResult);
            }

            return bmpResult;
        }
    }
}
