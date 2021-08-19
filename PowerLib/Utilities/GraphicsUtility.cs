using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace PowerLib.Utilities
{
    /// <summary>
    /// 封装图形处理
    /// </summary>
    public static class GraphicsUtility
    {
        /// <summary>
        /// 计算两点之间线段距离
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <returns></returns>
        public static double GetDistance(PointF pt1, PointF pt2)
        {
            float width = pt2.X - pt1.X;
            float height = pt2.Y - pt1.Y;
            float result = width * width + height * height;
            return Math.Sqrt(result);
        }

        /// <summary>
        /// 计算两点之间线段距离，计算结果以毫米为单位
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="pxWidth">像素宽度</param>
        /// <param name="pxHeight">像素高度</param>
        /// <returns></returns>
        public static double GetDistanceInMm(PointF pt1, PointF pt2, double pxWidth, double pxHeight)
        {
            float width = pt2.X - pt1.X;
            float height = pt2.Y - pt1.Y;
            double result = width * pxWidth * width * pxWidth + height * pxHeight * height * pxHeight;
            return Math.Sqrt(result);
        }

        /// <summary>
        /// 获取指定角度的两条半径所组成的最小圆心角角度
        /// </summary>
        /// <param name="angleA">半径A的角度</param>
        /// <param name="angleB">半径B的角度</param>
        /// <returns></returns>
        public static float GetAngleDiff(float angleA, float angleB)
        {
            float diff = angleB - angleA;
            if (diff > 0)
            {
                if (diff > 180)
                    return -(360 - diff);
                return diff;
            }

            if (diff < -180)
                return -(-360 - diff);
            return diff;
        }

        /// <summary>
        /// 获取指定角度的两条半径的中线角度
        /// </summary>
        /// <param name="angleA"></param>
        /// <param name="angleB"></param>
        /// <returns></returns>
        public static float GetMidAngle(float angleA, float angleB)
        {
            float result = (angleA + angleB) / 2;
            if (Math.Abs(angleA - angleB) > 180)
                return result - 180;

            return result;
        }

        /// <summary>
        /// 获取指定角度的半径的相对半径的角度
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public static float GetOppositeAngle(float angle)
        {
            float result = angle + 180;
            return result >= 360 ? result - 360 : result;
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
            // 弧度
            double angleInRadians = angleInDegrees * (Math.PI / 180);
            // 余弦
            double cosTheta = Math.Cos(angleInRadians);
            // 正弦
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

            // 左上角
            path.AddArc(arcRect, 180, 90);

            // 右上角
            arcRect.X = rect.Right - diameter;
            path.AddArc(arcRect, 270, 90);

            // 右下角
            arcRect.Y = rect.Bottom - diameter;
            path.AddArc(arcRect, 0, 90);

            // 左下角
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
    }
}
