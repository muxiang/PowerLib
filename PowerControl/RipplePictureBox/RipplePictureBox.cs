using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

using static PowerControl.NativeConstants;
using static PowerControl.NativeStructures;
using static PowerControl.NativeMethods;

namespace PowerControl
{
    /// <summary>
    /// 水波特效图片框
    /// </summary>
    public class RipplePictureBox : PictureBox
    {
        #region 字段

        //特效实例
        private RippleEffect _effect;
        //渲染定时器
        private readonly Timer _timer;
        //是否正在拖动
        private bool _dragging;
        //上一次单击点
        private Point? _lastClick;
        //单击计时
        private Stopwatch _swClick;
        //未缩放的原始底图
        private Bitmap _bmpOrigin;

        #endregion 字段

        #region 常量

        //核定每秒帧数
        private const int FPS = 60;
        //单击延迟
        private const int ClickDelay = 1000;

        #endregion 常量

        #region 构造器

        /// <summary>
        /// 初始化水波特效图片框
        /// </summary>
        public RipplePictureBox()
        {
            //缺省水波半径
            ClickSplashRadius = 12;
            DragSplashRadius = 7;

            _dragging = false;

            _timer = new Timer { Interval = 1000 / FPS };
            _timer.Tick += (s1, e1) => UpdateFrame();

            MouseDown += (s1, e1) =>
            {
                //由于算法缺陷,避免短时间内在同一点生成大量水波
                if (ComparePoint(_lastClick, e1.Location, 10) && _swClick != null && _swClick.ElapsedMilliseconds < ClickDelay) return;

                _swClick = Stopwatch.StartNew();
                _lastClick = new Point(e1.X, e1.Y);
                Splash(e1.Location.X, e1.Location.Y, ClickSplashRadius);
                _dragging = true;
            };

            MouseUp += (s1, e1) => { _dragging = false; };

            MouseMove += (s1, e1) =>
            {
                if (_dragging)
                    Splash(e1.Location.X, e1.Location.Y, DragSplashRadius);
            };

            _timer.Start();
        }

        #endregion 构造器

        #region 属性

        /// <summary>
        /// 设置水波单击时半径
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(12)]
        public int ClickSplashRadius { get; set; }

        /// <summary>
        /// 设置水波拖动时半径
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(7)]
        public int DragSplashRadius { get; set; }

        /// <summary>
        /// 设置水波背景图
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue(null)]
        public new Image Image
        {
            get => _effect?.Texture;
            set
            {
                if (value == null) return;
                _bmpOrigin = new Bitmap(value);
                //Bitmap texture = Utilities.StretchBitmap(_bmpOrigin, Size);
                _effect?.Dispose();
                _effect = new RippleEffect(_bmpOrigin);
            }
        }

        /// <summary>
        /// 启用或禁用动画
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [DefaultValue(true)]
        public bool AnimationEnabled
        {
            get => _timer.Enabled;
            set
            {
                _timer.Enabled = value;
                if (!value) Clear();
            }
        }

        #endregion 属性

        #region 公开方法

        /// <summary>
        /// 在指定区域创建水波
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="radius">半径</param>
        public void Splash(int x, int y, int radius)
        {
            Point lc = new Point(x, y);
            lc = Translate(lc);
            _effect?.Splash(lc.X, lc.Y, radius);
        }

        /// <summary>
        /// 清除浪高数据
        /// </summary>
        public void Clear()
        {
            _effect?.Clear();
        }

        #endregion 公开方法

        #region 重写

        /// <summary>
        /// 窗口函数
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //禁止擦除背景
                case WM_ERASEBKGND:
                    return;
                //绘图
                case WM_PAINT:

                    PAINTSTRUCT paintStruct = new PAINTSTRUCT();
                    //开始绘图,获取设备上下文(dc)指针
                    IntPtr wndHdc = BeginPaint(m.HWnd, ref paintStruct);

                    if (_effect?.Texture != null)
                    {
                        //生成一帧动画
                        Bitmap f = _effect.Render();
                        using (Graphics gWnd = Graphics.FromHdc(wndHdc))
                        {
                            //渲染到窗口
                            gWnd.DrawImage(f, ClientRectangle);
                        }
                    }

                    //结束绘图
                    EndPaint(m.HWnd, ref paintStruct);
                    return;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        #endregion 重写

        #region 私有方法

        //更新一帧
        private void UpdateFrame()
        {
            if (DesignMode) return;
            if (_effect == null) return;
            //更新特效缓冲区浪高数据
            _effect.Update();
            //强制重绘
            Invalidate();
        }

        /// <summary>
        /// 判断两个点的X相差或Y相差是否大于指定值
        /// </summary>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        /// <param name="diff"></param>
        /// <returns></returns>
        private static bool ComparePoint(Point? pt1, Point? pt2, int diff)
        {
            if (pt1 == null || pt2 == null) return false;
            int xDiff = pt1.Value.X - pt2.Value.X;
            int yDiff = pt1.Value.Y - pt2.Value.Y;

            return Math.Abs(xDiff) < diff && Math.Abs(yDiff) < diff;
        }

        // Handles different control and image sizes
        private Point Translate(Point point)
        {
            Double cx = (double)this.Image.Width / this.Width;
            Double cy = (double)this.Image.Height / this.Height;
            return new Point((int)(cx * point.X), (int)(cy * point.Y));
        }

        #endregion 私有方法
    }
}
