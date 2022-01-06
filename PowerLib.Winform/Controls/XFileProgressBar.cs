using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using PowerLib.Winform.Design;

using ThreadingTimer = System.Threading.Timer;

using static PowerLib.Utilities.CommonUtility;

namespace PowerLib.Winform.Controls
{
    /// <summary>
    /// 表示支持速度变化的文件传输进度条
    /// </summary>
    [Designer(typeof(FixedSizeDesigner))]
    public sealed partial class XFileProgressBar : Control
    {
        #region 常量

        // 宽
        private const int WIDTH = 400;
        // 高
        private const int HEIGHT = 80;
        // 内边距
        private const int PADDING = 2;
        // 内宽
        private const int INNER_WIDTH = WIDTH - PADDING * 2;
        // 内高
        private const int INNER_HEIGHT = HEIGHT - PADDING * 2;

        #endregion 常量

        #region 字段

        // 速度文本格式化
        private static readonly string SpeedStringFormat;

        // 文件总大小(字节)
        private long _totalSizeInBytes;
        // 已传输(字节)
        private long _valueInBytes;

        // 1秒内速度记录，每元素表示为<计时周期数,速度>，元素入列时，根据入列时的计时周期数，对超过一秒的队列元素进行出列
        private readonly Queue<Tuple<long, long>> _queueSpeedsInSecond;

        // 实时速度记录(每像素，跨多像素时，中间像素位置记录-1)
        private readonly long[] _arrSpeedRecords;
        // 当前控件最高点表示的速度边界，即界面能表示的最高速度，随速度峰值更新而倍增
        private long _speedTop;
        // 上一个绘制完成的索引，对应_arrSpeedRecords中索引
        private int _prevDrawingIndex;
        // 实时速度
        private long _realtimeSpeed;
        // 显示速度，每250ms从_realtimeSpeed同步
        private long _displaySpeed;
        // 用于同步实时速度的定时器
        private ThreadingTimer _tmrUpdateDisplaySpeed;

        // 边框颜色
        private Color _clrBorder;
        // 网格颜色
        private Color _clrGrid;
        // 前景色
        private Color _clrFore;
        // 背景色
        private Color _clrBack;
        // 速度前景色
        private Color _clrSpeedFore;
        // 速度背景色
        private Color _clrSpeedBack;

        private Pen _penFore;

        private Brush _brsSpeedFore;

        private Pen _penBorder;
        private Pen _penGrid;

        // 速度变化缓冲位图
        private readonly Bitmap _bmpSpeedWaveBuffer;
        // 速度变化缓冲位图图形
        private readonly Graphics _gBmpSpeedWaveBuffer;

        #endregion 字段

        #region 属性

        #region 设计器可见

        /// <summary>
        /// 获取或设置进度条的边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置进度条的边框颜色")]
        [DefaultValue(typeof(Color), "188, 188, 188")]
        public Color BorderColor
        {
            get => _clrBorder;
            set
            {
                _clrBorder = value;
                _penBorder = new Pen(_clrBorder);
                Refresh();
            }
        }

        /// <summary>
        /// 获取或设置进度条的网格颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置进度条的网格颜色")]
        [DefaultValue(typeof(Color), "205, 239, 211")]
        public Color GridColor
        {
            get => _clrGrid;
            set
            {
                _clrGrid = value;
                _penGrid = new Pen(_clrGrid);
                Refresh();
            }
        }

        /// <summary>
        /// 获取或设置进度条的前景颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置进度条的前景颜色")]
        [DefaultValue(typeof(Color), "15, 14, 15")]
        public new Color ForeColor
        {
            get => _clrFore;
            set
            {
                _clrFore = value;
                _penFore = new Pen(_clrFore);
                Refresh();
            }
        }

        /// <summary>
        /// 获取或设置进度条的背景颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置进度条的背景颜色")]
        [DefaultValue(typeof(Color), "255, 255, 255")]
        public new Color BackColor
        {
            get => _clrBack;
            set
            {
                _clrBack = value;
                Refresh();
            }
        }

        /// <summary>
        /// 获取或设置进度条的速度前景颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置进度条的速度前景颜色")]
        [DefaultValue(typeof(Color), "6, 176, 37")]
        public Color SpeedForeColor
        {
            get => _clrSpeedFore;
            set
            {
                _clrSpeedFore = value;
                _brsSpeedFore = new SolidBrush(_clrSpeedFore);
                Refresh();
            }
        }

        /// <summary>
        /// 获取或设置进度条的速度背景颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("获取或设置进度条的速度背景颜色")]
        [DefaultValue(typeof(Color), "167, 229, 145")]
        public Color SpeedBackColor
        {
            get => _clrSpeedBack;
            set
            {
                _clrSpeedBack = value;
                Refresh();
            }
        }

        #endregion 设计器可见

        /// <summary>
        /// 获取或设置文件总大小(字节)
        /// </summary>
        [Browsable(false)]
        public long TotalSizeInBytes
        {
            get => _totalSizeInBytes;
            set
            {
                _totalSizeInBytes = value;
                Refresh();
            }
        }

        /// <summary>
        /// 获取进度条的百分比
        /// </summary>
        [Browsable(false)]
        public double Percentage
        {
            get
            {
                if (_totalSizeInBytes == 0)
                    return 0;

                return _valueInBytes / (double)_totalSizeInBytes * 100;
            }
        }

        #endregion 属性

        #region 构造器

        static XFileProgressBar()
        {
            SpeedStringFormat = CultureInfo.CurrentUICulture.Name == "zh-CN"
                ? "速度: {0}/秒"
                : "Speed: {0}/s";
        }

        /// <summary>
        /// 初始化<see cref="XFileProgressBar"/>的实例
        /// </summary>
        public XFileProgressBar()
        {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            Width = WIDTH;
            Height = HEIGHT;

            BorderColor = Color.FromArgb(188, 188, 188);
            GridColor = Color.FromArgb(205, 239, 211);
            ForeColor = Color.FromArgb(15, 14, 15);
            BackColor = Color.FromArgb(255, 255, 255);
            SpeedForeColor = Color.FromArgb(6, 176, 37);
            SpeedBackColor = Color.FromArgb(167, 229, 145);

            // 初始化速度变化缓冲位图
            _bmpSpeedWaveBuffer = new Bitmap(INNER_WIDTH, INNER_HEIGHT);
            _gBmpSpeedWaveBuffer = Graphics.FromImage(_bmpSpeedWaveBuffer);
            _gBmpSpeedWaveBuffer.SmoothingMode = SmoothingMode.HighQuality;
            _gBmpSpeedWaveBuffer.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // 实时速度记录(每像素，跨多像素时，中间像素位置记录-1)
            _arrSpeedRecords = new long[INNER_WIDTH];
            for (int i = 0; i < _arrSpeedRecords.Length; i++)
                _arrSpeedRecords[i] = -1;

            _queueSpeedsInSecond = new Queue<Tuple<long, long>>(1000);

            ClearSpeedBuffer();

            Font = new Font("微软雅黑", 4, FontStyle.Regular);
        }

        #endregion 构造器

        #region 方法

        /// <summary>
        /// 绘制网格
        /// </summary>
        /// <param name="g">图形</param>
        /// <param name="pen">画笔</param>
        /// <param name="size">尺寸</param>
        /// <param name="padding">内边距</param>
        private static void DrawGrids(Graphics g, Pen pen, Size size, int padding)
        {
            float gridWidth = (size.Width - padding * 2) / 10F;
            float gridHeight = (size.Height - padding * 2) / 5F;

            float x = padding, y;

            // 网格
            // 横
            for (int i = 0; i < 4; i++)
            {
                y = padding + gridHeight * (i + 1);
                g.DrawLine(pen, x, y, size.Width - padding, y);
            }

            y = padding;

            // 竖
            for (int i = 0; i < 9; i++)
            {
                x = padding + gridWidth * (i + 1);
                g.DrawLine(pen, x, y, x, size.Height - padding);
            }
        }

        /// <summary>
        /// 清除速度变化缓冲位图内容，重新填充背景及网格，并绘制现有速度记录
        /// </summary>
        private void ClearSpeedBuffer()
        {
            // 背景
            _gBmpSpeedWaveBuffer.Clear(SpeedBackColor);
            // 网格
            DrawGrids(_gBmpSpeedWaveBuffer, _penGrid, _bmpSpeedWaveBuffer.Size, 0);

            List<PointF> lst = new List<PointF>();

            double topSpeed = _speedTop;

            // 重置绘图索引
            _prevDrawingIndex = 0;

            // 速度记录顶点
            for (int i = 0; i < _arrSpeedRecords.Length; i++)
            {
                if (_arrSpeedRecords[i] == -1)
                    continue;

                float x = i;
                float y = INNER_HEIGHT - (float)Math.Round(_arrSpeedRecords[i] / topSpeed * INNER_HEIGHT);

                lst.Add(new PointF(x, y));
            }

            if (lst.Count < 2)
                return;

            // 补全多边形下方顶点
            PointF ptLast = lst[lst.Count - 1];
            lst.Add(new PointF(ptLast.X, INNER_HEIGHT));
            lst.Add(new PointF(0, INNER_HEIGHT));

            _prevDrawingIndex = (int)ptLast.X;

            _gBmpSpeedWaveBuffer.FillPolygon(_brsSpeedFore, lst.ToArray());
        }

        /// <summary>
        /// 向进度条添加字节
        /// </summary>
        /// <param name="incrInBytes">字节数</param>
        public void AddValue(long incrInBytes)
        {
            if (incrInBytes < 1)
                return;

            _valueInBytes += incrInBytes;

            if (_valueInBytes > _totalSizeInBytes)
                _valueInBytes = _totalSizeInBytes;

            long ticks = DateTime.Now.Ticks;
            _queueSpeedsInSecond.Enqueue(new Tuple<long, long>(ticks, incrInBytes));

            // 移除一秒前速度记录
            while (ticks - _queueSpeedsInSecond.Peek().Item1 > TimeSpan.TicksPerSecond)
                _queueSpeedsInSecond.Dequeue();

            Interlocked.Exchange(ref _realtimeSpeed, _queueSpeedsInSecond.Sum(tuple => tuple.Item2));

            // 记录速度
            int x = CoerceValue((int)Math.Round(Percentage / 100 * INNER_WIDTH), 0, INNER_WIDTH - 1);
            _arrSpeedRecords[x] = _realtimeSpeed;

            // 首次记录
            if (_arrSpeedRecords[0] == -1)
                _arrSpeedRecords[0] = _realtimeSpeed;

            if (_realtimeSpeed > _speedTop * .75)
            {
                // 更新峰值
                Interlocked.Exchange(ref _speedTop, _realtimeSpeed * 2);
                ClearSpeedBuffer();
            }
            else
            {
                // 连接前一速度记录
                double topSpeed = _speedTop;
                long prevSpeed = _arrSpeedRecords[_prevDrawingIndex];

                PointF[] polygon =
                {
                    new PointF(_prevDrawingIndex, INNER_HEIGHT),// 左下
                    new PointF(_prevDrawingIndex, INNER_HEIGHT - (float)Math.Round(prevSpeed / topSpeed * INNER_HEIGHT)),// 前一速度记录，左上
                    new PointF(x, INNER_HEIGHT - (float)Math.Round(_realtimeSpeed / topSpeed * INNER_HEIGHT)),// 当前速度记录，右上
                    new PointF(x, INNER_HEIGHT)// 右下
                };

                _prevDrawingIndex = x;
                _gBmpSpeedWaveBuffer.FillPolygon(_brsSpeedFore, polygon);
            }

            Refresh();
        }

        /// <summary>
        /// 通过清除数据将进度条还原成初始状态
        /// </summary>
        public void ClearData()
        {
            for (int i = 0; i < _arrSpeedRecords.Length; i++)
                _arrSpeedRecords[i] = -1;

            _queueSpeedsInSecond.Clear();

            _valueInBytes = 0;
            _speedTop = 0;

            ClearSpeedBuffer();

            Refresh();
        }

        #endregion 方法

        #region 重写Control

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            _tmrUpdateDisplaySpeed = new ThreadingTimer(o =>
            {
                Interlocked.Exchange(ref _displaySpeed, _realtimeSpeed);
            }, null, 0, 250);
        }

        protected override void OnResize(EventArgs e) { }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            Graphics g = pe.Graphics;

            g.SmoothingMode = SmoothingMode.HighQuality;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            g.Clear(BackColor);

            // 边框
            g.DrawRectangle(_penBorder, ClientRectangle);

            // 网格
            DrawGrids(g, _penGrid, ClientSize, PADDING);

            // 速度
            g.DrawImageUnscaledAndClipped(_bmpSpeedWaveBuffer, new Rectangle(PADDING, PADDING, _prevDrawingIndex, INNER_HEIGHT));

            if (_speedTop <= 0)
                return;

            // 参考线
            double topSpeed = _speedTop;
            float lineY = INNER_HEIGHT - (float)Math.Round(_realtimeSpeed / topSpeed * INNER_HEIGHT);
            g.DrawLine(_penFore, PADDING, lineY, WIDTH - PADDING, lineY);

            // 文字
            string strSpeed = string.Format(SpeedStringFormat, GetSizeSuffix(_displaySpeed));
            SizeF szText = g.MeasureString(strSpeed, Font);

            float textY = lineY - szText.Height;
            if (textY < PADDING)
                textY += szText.Height;

            g.DrawString(strSpeed, Font, Brushes.Black,
                new RectangleF(WIDTH - PADDING - szText.Width, textY, szText.Width, szText.Height));
        }

        #endregion 重写Control
    }
}
