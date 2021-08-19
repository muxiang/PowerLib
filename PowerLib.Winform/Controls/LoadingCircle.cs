using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using ThreadingTimer = System.Threading.Timer;

namespace PowerLib.Winform.Controls
{
    /// <summary>
    /// 表示一个加载圆圈动画
    /// </summary>
    [ToolboxBitmap(typeof(LoadingCircle), "LoadingCircleIcon.png")]
    public sealed class LoadingCircle : Control
    {
        #region 构造

        /// <summary>
        /// 初始化加载圆圈动画的实例
        /// </summary>
        public LoadingCircle()
        {
            // 双缓冲，禁擦背景
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer,
                true);

            _dotSize = Width / 10f;

            // 初始化"点"
            _dots = new Dot[5];

            Color = Color.White;
        }

        #endregion 构造

        #region 属性

        /// <summary>
        ///     圆心
        /// </summary>
        [Browsable(false)]
        public PointF CircleCenter
        {
            get { return new PointF(Width / 2f, Height / 2f); }
        }

        /// <summary>
        ///     半径
        /// </summary>
        [Browsable(false)]
        public float CircleRadius
        {
            get { return Width / 2f - _dotSize; }
        }

        /// <summary>
        ///     颜色
        /// </summary>
        [Browsable(true), Category("Appearance"), Description("设置\"点\"的前景色")]
        public Color Color { get; set; }

        #endregion 属性

        #region 字段

        // 点数组
        private readonly Dot[] _dots;

        // Timers
        private ThreadingTimer _tmrDraw;
        private ThreadingTimer _tmrAction;

        // 点大小
        private float _dotSize;

        // 是否活动
        private bool _isActived;

        // 是否绘制:用于状态重置时挂起与恢复绘图
        private bool _isDrawing = true;

        // Timer计数:用于延迟启动每个点
        private int _timerCount;

        #endregion 字段

        #region 常量

        // 动作间隔(Timer)
        private const int ACTION_INTERVAL = 30;

        // 计数基数：用于计算每个点启动延迟：index * timerCountRadix
        private const int TIMER_COUNT_RADIX = 45;

        #endregion 常量

        #region 方法

        // 检查是否重置
        private bool CheckToReset()
        {
            return _dots.Count(d => d.Opacity > 0) == 0;
        }

        // 初始化点元素
        private void CreateDots()
        {
            for (int i = 0; i < _dots.Length; ++i)
                _dots[i] = new Dot(CircleCenter, CircleRadius);
        }

        /// <summary>
        ///     开关
        /// </summary>
        public bool Switch()
        {
            if (!_isActived)
                Start();
            else
                Stop();

            return _isActived;
        }

        /// <summary>
        /// 开始
        /// </summary>
        public void Start()
        {
            CreateDots();

            _timerCount = 0;
            foreach (Dot dot in _dots)
                dot.Reset();

            // 初始化绘图timer
            _tmrDraw = new ThreadingTimer(o =>
            {
                if (!IsHandleCreated || Disposing || IsDisposed)
                    return;

                BeginInvoke(new MethodInvoker(Invalidate));
            }, null, 0, 1000 / 60);


            // 初始化动作timer
            _tmrAction = new ThreadingTimer(
                state =>
                {
                    // 动画动作
                    for (int i = 0; i < _dots.Length; i++)
                        if (_timerCount++ > i * TIMER_COUNT_RADIX)
                            _dots[i].DotAction();

                    // 是否重置
                    if (CheckToReset())
                    {
                        // 重置前暂停绘图
                        _isDrawing = false;

                        _timerCount = 0;

                        foreach (Dot dot in _dots)
                            dot.Reset();

                        // 恢复绘图
                        _isDrawing = true;
                    }

                    if (_isActived)
                        _tmrAction.Change(ACTION_INTERVAL, Timeout.Infinite);
                },
                null, ACTION_INTERVAL, Timeout.Infinite);

            _isActived = true;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            _tmrDraw.Dispose();
            _tmrAction.Dispose();
            _isActived = false;
        }

        #endregion 方法

        #region 重写

        protected override void OnPaint(PaintEventArgs e)
        {
            if (_isActived && _isDrawing)
            {
                // 缓冲绘制
                using Bitmap bmp = new Bitmap(Width, Height);
                using Graphics bufferGraphics = Graphics.FromImage(bmp);
                // 抗锯齿
                bufferGraphics.SmoothingMode = SmoothingMode.HighQuality;
                foreach (Dot dot in _dots)
                {
                    RectangleF rect = new RectangleF(
                        new PointF(dot.Location.X - _dotSize / 2, dot.Location.Y - _dotSize / 2),
                        new SizeF(_dotSize, _dotSize));

                    bufferGraphics.FillEllipse(new SolidBrush(Color.FromArgb(dot.Opacity, Color)), rect);
                }

                // 贴图
                e.Graphics.DrawImage(bmp, new PointF(0, 0));
            }

            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            Height = Width;
            _dotSize = Width / 12f;

            base.OnResize(e);
        }

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new MethodInvoker(() => { Dispose(disposing); }));
                return;
            }

            if (disposing) Stop();
            base.Dispose(disposing);
        }

        #endregion 重写
    }
}