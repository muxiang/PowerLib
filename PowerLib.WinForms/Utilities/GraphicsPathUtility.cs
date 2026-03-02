using System.Drawing;
using System.Drawing.Drawing2D;

namespace PowerLib.WinForms.Utilities;

internal class GraphicsPathUtility
{
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
}