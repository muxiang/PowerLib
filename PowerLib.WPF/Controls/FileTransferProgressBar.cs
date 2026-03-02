using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using PowerLib.Utilities;

using ThreadingTimer = System.Threading.Timer;

namespace PowerLib.WPF.Controls
{
    /// <summary>
    /// 表示支持速度变化的文件传输进度条
    /// </summary>
    public class FileTransferProgressBar : Control
    {
        #region 常量

        // 默认宽
        private const double DEFAULT_WIDTH = 400;
        // 默认高
        private const double DEFAULT_HEIGHT = 80;
        // 内边距
        private const double PADDING = 2;
        // 内宽
        private const double INNER_WIDTH = DEFAULT_WIDTH - PADDING * 2;
        // 内高
        private const double INNER_HEIGHT = DEFAULT_HEIGHT - PADDING * 2;
        // 速度记录数组长度
        private const int SPEED_RECORD_COUNT = (int)INNER_WIDTH;

        #endregion 常量

        #region 静态字段

        // 速度文本格式化
        private static readonly string SpeedStringFormat;

        #endregion 静态字段

        #region 字段

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

        #endregion 字段

        #region 依赖属性

        /// <summary>
        /// 标识<see cref="TotalSizeInBytes"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty TotalSizeInBytesProperty =
            DependencyProperty.Register(nameof(TotalSizeInBytes), typeof(long), typeof(FileTransferProgressBar),
                new FrameworkPropertyMetadata(0L, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="GridBrush"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty GridBrushProperty =
            DependencyProperty.Register(nameof(GridBrush), typeof(Brush), typeof(FileTransferProgressBar),
                new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(205, 239, 211)), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="SpeedForeground"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty SpeedForegroundProperty =
            DependencyProperty.Register(nameof(SpeedForeground), typeof(Brush), typeof(FileTransferProgressBar),
                new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(6, 176, 37)), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="SpeedBackground"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty SpeedBackgroundProperty =
            DependencyProperty.Register(nameof(SpeedBackground), typeof(Brush), typeof(FileTransferProgressBar),
                new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(167, 229, 145)), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="Foreground"/>依赖属性，覆盖 Control.Foreground 以提供默认值和 AffectsRender 元数据
        /// </summary>
        public new static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(FileTransferProgressBar),
                new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(15, 14, 15)), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="Background"/>依赖属性，覆盖 Control.Background 以提供默认值和 AffectsRender 元数据
        /// </summary>
        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(FileTransferProgressBar),
                new FrameworkPropertyMetadata(Brushes.White, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="BorderBrush"/>依赖属性，覆盖 Control.BorderBrush 以提供默认值和 AffectsRender 元数据
        /// </summary>
        public new static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(FileTransferProgressBar),
                new FrameworkPropertyMetadata(new SolidColorBrush(Color.FromRgb(188, 188, 188)), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="SpeedTextBrush"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty SpeedTextBrushProperty =
            DependencyProperty.Register(nameof(SpeedTextBrush), typeof(Brush), typeof(FileTransferProgressBar),
                new FrameworkPropertyMetadata(Brushes.Black, FrameworkPropertyMetadataOptions.AffectsRender));

        #endregion 依赖属性

        #region 属性

        /// <summary>
        /// 获取或设置文件总大小(字节)
        /// </summary>
        public long TotalSizeInBytes
        {
            get => (long)GetValue(TotalSizeInBytesProperty);
            set => SetValue(TotalSizeInBytesProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的网格画刷
        /// </summary>
        public Brush GridBrush
        {
            get => (Brush)GetValue(GridBrushProperty);
            set => SetValue(GridBrushProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的速度前景画刷
        /// </summary>
        public Brush SpeedForeground
        {
            get => (Brush)GetValue(SpeedForegroundProperty);
            set => SetValue(SpeedForegroundProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的速度背景画刷
        /// </summary>
        public Brush SpeedBackground
        {
            get => (Brush)GetValue(SpeedBackgroundProperty);
            set => SetValue(SpeedBackgroundProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的前景画刷（参考线颜色）
        /// </summary>
        public new Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的背景画刷
        /// </summary>
        public new Brush Background
        {
            get => (Brush)GetValue(BackgroundProperty);
            set => SetValue(BackgroundProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的边框画刷
        /// </summary>
        public new Brush BorderBrush
        {
            get => (Brush)GetValue(BorderBrushProperty);
            set => SetValue(BorderBrushProperty, value);
        }

        /// <summary>
        /// 获取或设置速度文字画刷
        /// </summary>
        public Brush SpeedTextBrush
        {
            get => (Brush)GetValue(SpeedTextBrushProperty);
            set => SetValue(SpeedTextBrushProperty, value);
        }

        /// <summary>
        /// 获取进度条的百分比
        /// </summary>
        public double Percentage
        {
            get
            {
                long total = TotalSizeInBytes;
                if (total == 0)
                    return 0;

                return _valueInBytes / (double)total * 100;
            }
        }

        #endregion 属性

        #region 构造器

        static FileTransferProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileTransferProgressBar), new FrameworkPropertyMetadata(typeof(FileTransferProgressBar)));

            SpeedStringFormat = CultureInfo.CurrentUICulture.Name == "zh-CN"
                ? "速度: {0}/秒"
                : "Speed: {0}/s";
        }

        /// <summary>
        /// 初始化<see cref="FileTransferProgressBar"/>的实例
        /// </summary>
        public FileTransferProgressBar()
        {
            Width = DEFAULT_WIDTH;
            Height = DEFAULT_HEIGHT;

            // 实时速度记录(每像素，跨多像素时，中间像素位置记录-1)
            _arrSpeedRecords = new long[SPEED_RECORD_COUNT];
            for (int i = 0; i < _arrSpeedRecords.Length; i++)
                _arrSpeedRecords[i] = -1;

            _queueSpeedsInSecond = new Queue<Tuple<long, long>>(1000);

            Loaded += OnLoaded;
        }

        #endregion 构造器

        #region 事件处理

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _tmrUpdateDisplaySpeed = new ThreadingTimer(o =>
            {
                Interlocked.Exchange(ref _displaySpeed, _realtimeSpeed);
            }, null, 0, 250);
        }

        #endregion 事件处理

        #region 方法

        /// <summary>
        /// 向进度条添加字节
        /// </summary>
        /// <param name="incrInBytes">字节数</param>
        public void AddValue(long incrInBytes)
        {
            if (incrInBytes < 1)
                return;

            _valueInBytes += incrInBytes;

            long total = TotalSizeInBytes;
            if (_valueInBytes > total)
                _valueInBytes = total;

            long ticks = DateTime.Now.Ticks;
            _queueSpeedsInSecond.Enqueue(new Tuple<long, long>(ticks, incrInBytes));

            // 移除一秒前速度记录
            while (_queueSpeedsInSecond.Count > 0 && ticks - _queueSpeedsInSecond.Peek().Item1 > TimeSpan.TicksPerSecond)
                _queueSpeedsInSecond.Dequeue();

            Interlocked.Exchange(ref _realtimeSpeed, _queueSpeedsInSecond.Sum(tuple => tuple.Item2));

            // 记录速度
            int x = CoerceValue((int)Math.Round(Percentage / 100 * INNER_WIDTH), 0, SPEED_RECORD_COUNT - 1);
            _arrSpeedRecords[x] = _realtimeSpeed;

            // 首次记录
            if (_arrSpeedRecords[0] == -1)
                _arrSpeedRecords[0] = _realtimeSpeed;

            if (_realtimeSpeed > _speedTop * .75)
            {
                // 更新峰值
                Interlocked.Exchange(ref _speedTop, _realtimeSpeed * 2);
            }

            _prevDrawingIndex = x;

            InvalidateVisual();
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
            _prevDrawingIndex = 0;

            InvalidateVisual();
        }

        /// <summary>
        /// 确保值不越界
        /// </summary>
        private static int CoerceValue(int raw, int lo, int hi)
        {
            if (raw <= lo) return lo;
            if (raw >= hi) return hi;
            return raw;
        }

        #endregion 方法

        #region 渲染

        protected override void OnRender(DrawingContext dc)
        {
            double width = ActualWidth;
            double height = ActualHeight;

            if (width <= 0 || height <= 0)
                return;

            double innerWidth = width - PADDING * 2;
            double innerHeight = height - PADDING * 2;

            // 背景
            if (Background != null)
                dc.DrawRectangle(Background, null, new Rect(0, 0, width, height));

            // 边框
            if (BorderBrush != null)
            {
                Pen borderPen = new Pen(BorderBrush, 1);
                dc.DrawRectangle(null, borderPen, new Rect(0.5, 0.5, width - 1, height - 1));
            }

            // 网格
            DrawGrids(dc, innerWidth, innerHeight);

            // 速度波形区域——裁剪到内边距区域
            dc.PushClip(new RectangleGeometry(new Rect(PADDING, PADDING, innerWidth, innerHeight)));

            // 速度波形背景
            if (SpeedBackground != null)
            {
                double waveWidth = (double)_prevDrawingIndex / SPEED_RECORD_COUNT * innerWidth;
                dc.DrawRectangle(SpeedBackground, null, new Rect(PADDING, PADDING, waveWidth, innerHeight));
            }

            // 在速度波形背景上再画一次网格
            DrawGridsInner(dc, innerWidth, innerHeight);

            // 绘制速度波形多边形
            DrawSpeedWave(dc, innerWidth, innerHeight);

            dc.Pop(); // 弹出裁剪

            if (_speedTop <= 0)
                return;

            // 参考线
            double topSpeed = _speedTop;
            double lineY = PADDING + innerHeight - Math.Round(_realtimeSpeed / topSpeed * innerHeight);

            if (Foreground != null)
            {
                Pen forePen = new Pen(Foreground, 1);
                dc.DrawLine(forePen, new Point(PADDING, lineY), new Point(width - PADDING, lineY));
            }

            // 速度文字
            string strSpeed = string.Format(SpeedStringFormat, CommonUtility.GetSizeSuffix(_displaySpeed));
            Typeface typeface = new Typeface(FontFamily, FontStyle, FontWeight, FontStretch);
            FormattedText ft = new FormattedText(
                strSpeed,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                typeface,
                FontSize,
                SpeedTextBrush ?? Brushes.Black);

            double textY = lineY - ft.Height;
            if (textY < PADDING)
                textY = lineY;

            double textX = width - PADDING - ft.Width;
            dc.DrawText(ft, new Point(textX, textY));
        }

        /// <summary>
        /// 在控件整体区域上绘制网格（带内边距偏移）
        /// </summary>
        private void DrawGrids(DrawingContext dc, double innerWidth, double innerHeight)
        {
            if (GridBrush == null)
                return;

            Pen gridPen = new Pen(GridBrush, 1);
            gridPen.Freeze();

            double gridWidth = innerWidth / 10.0;
            double gridHeight = innerHeight / 5.0;

            // 横线
            for (int i = 0; i < 4; i++)
            {
                double y = PADDING + gridHeight * (i + 1);
                dc.DrawLine(gridPen, new Point(PADDING, y), new Point(PADDING + innerWidth, y));
            }

            // 竖线
            for (int i = 0; i < 9; i++)
            {
                double x = PADDING + gridWidth * (i + 1);
                dc.DrawLine(gridPen, new Point(x, PADDING), new Point(x, PADDING + innerHeight));
            }
        }

        /// <summary>
        /// 在速度波形内部区域绘制网格（无偏移，直接在裁剪区域内）
        /// </summary>
        private void DrawGridsInner(DrawingContext dc, double innerWidth, double innerHeight)
        {
            if (GridBrush == null)
                return;

            Pen gridPen = new Pen(GridBrush, 1);
            gridPen.Freeze();

            double gridWidth = innerWidth / 10.0;
            double gridHeight = innerHeight / 5.0;

            for (int i = 0; i < 4; i++)
            {
                double y = PADDING + gridHeight * (i + 1);
                dc.DrawLine(gridPen, new Point(PADDING, y), new Point(PADDING + innerWidth, y));
            }

            for (int i = 0; i < 9; i++)
            {
                double x = PADDING + gridWidth * (i + 1);
                dc.DrawLine(gridPen, new Point(x, PADDING), new Point(x, PADDING + innerHeight));
            }
        }

        /// <summary>
        /// 绘制速度波形多边形
        /// </summary>
        private void DrawSpeedWave(DrawingContext dc, double innerWidth, double innerHeight)
        {
            if (_speedTop <= 0)
                return;

            double topSpeed = _speedTop;
            List<Point> points = new();

            for (int i = 0; i < _arrSpeedRecords.Length; i++)
            {
                if (_arrSpeedRecords[i] == -1)
                    continue;

                double x = PADDING + (double)i / SPEED_RECORD_COUNT * innerWidth;
                double y = PADDING + innerHeight - Math.Round(_arrSpeedRecords[i] / topSpeed * innerHeight);

                points.Add(new Point(x, y));
            }

            if (points.Count < 2)
                return;

            // 构建闭合多边形：顶点 → 右下 → 左下
            StreamGeometry geometry = new();
            using (StreamGeometryContext ctx = geometry.Open())
            {
                ctx.BeginFigure(new Point(points[0].X, PADDING + innerHeight), true, true);

                // 左边第一个点
                ctx.LineTo(points[0], false, false);

                // 沿速度曲线
                for (int i = 1; i < points.Count; i++)
                    ctx.LineTo(points[i], true, false);

                // 右下角
                Point lastPt = points[points.Count - 1];
                ctx.LineTo(new Point(lastPt.X, PADDING + innerHeight), false, false);
            }

            geometry.Freeze();

            if (SpeedForeground != null)
                dc.DrawGeometry(SpeedForeground, null, geometry);
        }

        #endregion 渲染
    }
}
