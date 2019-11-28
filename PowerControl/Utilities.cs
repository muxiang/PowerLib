using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace PowerControl
{
    /// <summary>
    /// 实用工具类
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// 递归获取控件所在的父窗口
        /// </summary>
        /// <param name="c">控件</param>
        /// <returns>父窗口引用</returns>
        public static Form GetParentForm(Control c)
        {
            if (c.Parent == null) return null;
            if (c.Parent is Form parent) return parent;

            return GetParentForm(c.Parent);
        }

        /// <summary>
        /// 获取更深的颜色
        /// </summary>
        /// <param name="baseColor">基色</param>
        /// <param name="diff">色差</param>
        /// <returns></returns>
        public static Color GetDeeperColor(Color baseColor, int diff = 32)
        {
            int r, g, b;

            r = baseColor.R - diff;
            g = baseColor.G - diff;
            b = baseColor.B - diff;
            r = r < 0 ? 0 : r;
            g = g < 0 ? 0 : g;
            b = b < 0 ? 0 : b;

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// 获取更浅的颜色
        /// </summary>
        /// <param name="baseColor">基色</param>
        /// <param name="diff">色差</param>
        /// <returns></returns>
        public static Color GetLighterColor(Color baseColor, int diff = 32)
        {
            int r, g, b;

            r = baseColor.R + diff;
            g = baseColor.G + diff;
            b = baseColor.B + diff;
            r = r > 255 ? 255 : r;
            g = g > 255 ? 255 : g;
            b = b > 255 ? 255 : b;

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// 获取圆角矩形路径
        /// </summary>
        /// <param name="rect">参考矩形</param>
        /// <param name="radius">圆角半径</param>
        /// <returns></returns>
        public static GraphicsPath GetRoundedRectPath(Rectangle rect, int radius)
        {
            int diameter = radius * 2;

            Rectangle arcRect = new Rectangle(rect.Location, new Size(diameter, diameter));
            GraphicsPath path = new GraphicsPath();

            //左上角
            path.AddArc(arcRect, 180, 90);

            //右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            //右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            //左下角
            arcRect.X = rect.Left;
            path.AddArc(arcRect, 90, 90);

            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// 根据半径、角度求圆上坐标
        /// </summary>
        /// <param name="center">圆心</param>
        /// <param name="radius">半径</param>
        /// <param name="angle">角度</param>
        /// <returns>坐标</returns>
        public static PointF GetDotLocationByAngle(PointF center, float radius, int angle)
        {
            var x = (float)(center.X + radius * Math.Cos(angle * Math.PI / 180));
            var y = (float)(center.Y + radius * Math.Sin(angle * Math.PI / 180));

            return new PointF(x, y);
        }

        /// <summary>
        /// 确保值不越界
        /// </summary>
        /// <param name="raw">原值</param>
        /// <param name="lo">最低</param>
        /// <param name="hi">最高</param>
        /// <returns></returns>
        public static int CoerceValue(int raw, int lo, int hi)
        {
            if (raw <= lo) return lo;
            if (raw >= hi) return hi;
            return raw;
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
        /// 在指定位置创建正多边形
        /// </summary>
        /// <param name="ptCenter">外切圆圆心</param>
        /// <param name="radius">外切圆半径</param>
        /// <param name="edgeCount">边数</param>
        /// <param name="originDegree">原始角度</param>
        /// <returns>多边形顶点数组</returns>
        /// <exception cref="ArgumentOutOfRangeException">边数小于3</exception>
        public static PointF[] CreateRegularPolygon(PointF ptCenter, float radius, int edgeCount, double originDegree = 0)
        {
            if (edgeCount < 3)
                throw new ArgumentOutOfRangeException(nameof(edgeCount));

            PointF[] polygon = new PointF[edgeCount];

            for (int i = 0; i < edgeCount; i++)
            {
                double x = ptCenter.X + Math.Cos(Math.PI / 180 * (360D / edgeCount * i + originDegree)) * radius;
                double y = ptCenter.Y + Math.Sin(Math.PI / 180 * (360D / edgeCount * i + originDegree)) * radius;

                polygon[i] = new PointF((float)x, (float)y);
            }

            return polygon;
        }

        /// <summary>
        /// 计算一个点绕另一个点旋转后的坐标
        /// </summary>
        /// <param name="pointToRotate">原始点</param>
        /// <param name="centerPoint">中心点</param>
        /// <param name="angleInDegrees">角度</param>
        /// <returns>旋转后的点</returns>
        public static PointF RotateAt(this PointF pointToRotate, PointF centerPoint, double angleInDegrees)
        {
            //弧度
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            //余弦
            double cosTheta = Math.Cos(angleInRadians);
            //正弦
            double sinTheta = Math.Sin(angleInRadians);

            return new PointF(
                (float)(cosTheta * (pointToRotate.X - centerPoint.X) - sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
                (float)(sinTheta * (pointToRotate.X - centerPoint.X) + cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y)
            );
        }

        /// <summary>
        /// 获取给定正方形内最大的六边形
        /// </summary>
        /// <param name="rectSquare">正方形</param>
        /// <param name="ratioFactor">宽高比因子，值越大，六边形越宽</param>
        /// <returns></returns>
        public static PointF[] GetHexagonBySquare(RectangleF rectSquare, float ratioFactor = 4F)
        {
            PointF[] result =
            {
                new PointF(rectSquare.X + rectSquare.Width / ratioFactor, rectSquare.Y),
                new PointF(rectSquare.Right - rectSquare.Width / ratioFactor, rectSquare.Y),
                new PointF(rectSquare.Right, rectSquare.Y + rectSquare.Height / 2),
                new PointF(rectSquare.Right - rectSquare.Width / ratioFactor, rectSquare.Bottom),
                new PointF(rectSquare.X + rectSquare.Width / ratioFactor, rectSquare.Bottom),
                new PointF(rectSquare.X, rectSquare.Y + rectSquare.Height / 2),
            };

            return result;
        }

        /// <summary>
        /// 从位图计算图像路径
        /// </summary>
        /// <param name="bmp">位图</param>
        /// <param name="scanCallback">
        /// 扫描回调，回传当前扫描的像素索引
        /// </param>
        /// <returns></returns>
        public static unsafe GraphicsPath GetGraphicsPathFromBitmap(Bitmap bmp, Action<int> scanCallback = null)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            GraphicsPath path = new GraphicsPath();
            BitmapData bckdata = null;

            try
            {
                bckdata = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                uint* bckInt = (uint*)bckdata.Scan0;
                for (int j = 0; j < h; j++)
                {
                    for (int i = 0; i < w; i++)
                    {
                        if ((*bckInt & 0xFF000000) != 0)
                            path.AddRectangle(new Rectangle(i, j, 1, 1));

                        scanCallback?.Invoke(i + j * w);
                        bckInt++;
                    }
                }
            }
            finally
            {
                if (bckdata != null)
                    bmp.UnlockBits(bckdata);
            }

            return path;
        }

        /// <summary>
        /// Create a translucent bitmap
        /// </summary>
        /// <param name="alpha">Transparency</param>
        /// <param name="baseColor">Base-color</param>
        /// <param name="size">Size</param>
        /// <returns>Reference of bitmap</returns>
        public static Bitmap CreateBitmap(byte alpha, Color baseColor, Size size)
        {
            Bitmap bmp = new Bitmap(size.Width, size.Height);

            Color clr = Color.FromArgb(alpha, baseColor);

            //for (int i = 0; i < bmp.Width; i++)
            //    for (int j = 0; j < bmp.Height; j++)
            //        bmp.SetPixel(i, j, clr);

            using (Graphics g = Graphics.FromImage(bmp))
                g.Clear(clr);

            return bmp;
        }

        /// <summary>
        /// Stretch a bitmap to a new size
        /// </summary>
        /// <param name="original">Original bmp</param>
        /// <param name="stretchSize">Size</param>
        /// <param name="disposeOriginal">Auto dispose the original bitmap</param>
        /// <returns>Reference of new bmp</returns>
        public static Bitmap StretchBitmap(Bitmap original, Size stretchSize, bool disposeOriginal = true)
        {
            Bitmap newBitmap = new Bitmap(stretchSize.Width, stretchSize.Height);

            Graphics g = Graphics.FromImage(newBitmap);
            g.DrawImage(original, new Rectangle(new Point(0, 0), stretchSize));
            g.Dispose();

            if (disposeOriginal) original.Dispose();

            return newBitmap;
        }

        /// <summary>
        /// Cut a bitmap to a new rectangle
        /// </summary>
        /// <param name="original">Original bmp</param>
        /// <param name="cutRect">Rectangle</param>
        /// <param name="disposeOriginal">Auto dispose the original bitmap</param>
        /// <returns>Reference of new bmp</returns>
        public static Bitmap CutBitmap(Bitmap original, Rectangle cutRect, bool disposeOriginal = true)
        {
            Bitmap newBitmap = new Bitmap(cutRect.Width, cutRect.Height);

            Graphics g = Graphics.FromImage(newBitmap);
            g.DrawImage(original, new Rectangle(0, 0, cutRect.Width, cutRect.Height), cutRect, GraphicsUnit.Pixel);
            g.Dispose();

            if (disposeOriginal) original.Dispose();

            return newBitmap;
        }
    }
}
