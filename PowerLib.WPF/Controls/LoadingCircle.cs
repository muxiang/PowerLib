using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using PowerLib.Utilities;

namespace PowerLib.WPF.Controls;

/// <summary>
/// 表示一个Win8Metro风格的加载圆圈动画
/// </summary>
public class LoadingCircle : ContentControl,IDisposable
{
    /// <summary>
    /// 表示动画中的一个点，负责逻辑位置计算
    /// </summary>
    private sealed class Dot
    {
        #region 常量

        // 最小速度
        private const double MIN_SPEED = 1.5D;
        // 最大速度
        private const double MAX_SPEED = 6.5;
        // 出现时角度
        private const int APPEAR_ANGLE = 90;
        // 减速区角度
        private const int SLOW_ANGLE = 225;
        // 加速区角度
        private const int QUICK_ANGLE = 315;
        // 最大角度
        private const int MAX_ANGLE = 360;
        // 淡出负增量
        private const int ALPHA_SUB = 12;

        #endregion 常量

        #region 字段

        // 角度 0~360 不含360
        private double _angle;
        // 透明度 0~255
        private int _opacity;
        // 状态机进度 0~5
        private int _progress;
        // 速度
        private double _speed;
        // 圆心
        private Point _center;
        // 半径
        private double _radius;

        #endregion 字段

        #region 属性

        // 点位置
        public Point Location { get; private set; }

        // 透明度映射 (0-255)
        public int Opacity => _opacity < 0 ? 0 : _opacity > 255 ? 255 : _opacity;

        #endregion 属性

        /// <summary>
        /// 初始化<see cref="Dot"/>的实例
        /// </summary>
        /// <param name="center">圆心</param>
        /// <param name="radius">半径</param>
        public Dot(Point center, double radius)
        {
            UpdateLayout(center, radius);
            Reset();
        }

        #region 公开方法

        /// <summary>
        /// 更新圆心及半径以重新计算点位置
        /// </summary>
        /// <param name="center">圆心</param>
        /// <param name="radius">半径</param>
        public void UpdateLayout(Point center, double radius)
        {
            _center = center;
            _radius = radius;

            ReCalcLocation();
        }

        /// <summary>
        /// 重置点的状态
        /// </summary>
        public void Reset()
        {
            _angle = APPEAR_ANGLE;
            _speed = MIN_SPEED;
            _progress = 0;
            _opacity = 1;

            ReCalcLocation();
        }

        /// <summary>
        /// 触发一次点的动作，使之更新状态以匹配动画帧
        /// </summary>
        public void Action()
        {
            // 状态机逻辑
            switch (_progress)
            {
                case 0:// 初始加速区，完全不透明，加速
                    _opacity = 255;
                    AddSpeed();
                    if (_angle + _speed >= SLOW_ANGLE && _angle + _speed < QUICK_ANGLE)
                    {
                        _progress = 1;
                        _angle = SLOW_ANGLE - _speed;
                    }
                    break;
                case 1:// 减速区，完全不透明，减速
                    SubSpeed();
                    if (_angle + _speed >= QUICK_ANGLE || _angle + _speed < SLOW_ANGLE) { _progress = 2; _angle = QUICK_ANGLE - _speed; }
                    break;
                case 2:// 加速区2，完全不透明，加速
                    AddSpeed();
                    if (_angle + _speed >= SLOW_ANGLE && _angle + _speed < QUICK_ANGLE) { _progress = 3; _angle = SLOW_ANGLE - _speed; }
                    break;
                case 3:// 减速区2，完全不透明，减速
                    SubSpeed();
                    if (_angle + _speed >= QUICK_ANGLE && _angle + _speed < MAX_ANGLE) { _progress = 4; _angle = QUICK_ANGLE - _speed; }
                    break;
                case 4:// 减速区3，完全不透明，减速
                    SubSpeed();
                    if (_angle + _speed >= 0 && _angle + _speed < APPEAR_ANGLE) { _progress = 5; _angle = 0; }
                    break;
                case 5:// 加速淡出区，加速，透明度递减
                    AddSpeed();
                    if ((_opacity -= ALPHA_SUB) <= 0) _angle = APPEAR_ANGLE;
                    break;
            }

            // 更新点位置
            _angle = _angle >= MAX_ANGLE - _speed ? 0 : _angle + _speed;
            ReCalcLocation();
        }

        #endregion 公开方法

        #region 私有方法

        /// <summary>
        ///  根据圆心、半径、角度重新计算点位置
        /// </summary>
        private void ReCalcLocation()
        {
            Location = GeometryUtility.GetLocationByAngle(_center.ToSharedPoint(), _radius, _angle).ToWpfPoint();
        }

        private void AddSpeed() { if (++_speed > MAX_SPEED) _speed = MAX_SPEED; }
        private void SubSpeed() { if (--_speed < MIN_SPEED) _speed = MIN_SPEED; }

        #endregion 私有方法
    }

    #region 依赖属性

    public new static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
        nameof(Foreground), typeof(Brush), typeof(LoadingCircle),
        new PropertyMetadata(Brushes.White, (d, e) => ((LoadingCircle)d).UpdateForeground((Brush)e.NewValue)));

    public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
        nameof(Background), typeof(Brush), typeof(LoadingCircle),
        new PropertyMetadata(Brushes.Black, (d, e) => ((LoadingCircle)d).UpdateBackground((Brush)e.NewValue)));

    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
        nameof(IsActive), typeof(bool), typeof(LoadingCircle),
        new PropertyMetadata(false, (d, e) =>
        {
            if ((bool)e.NewValue) ((LoadingCircle)d).Start();
            else ((LoadingCircle)d).Stop();
        }));

    #endregion 依赖属性

    #region 常量

    // 点数量
    private const int DOT_COUNT = 5;
    // 点更新间隔，即第一个点更新逻辑位置后，第二个点需要等待多少次UpdateLogicState()调用后再进行更新
    private const int DOT_UPDATE_OFFSET = 80;

    // 逻辑帧间隔
    private const double LOGIC_STEP_MS = 1000 / 60D;

    #endregion 常量

    #region 字段

    private readonly Canvas _canvas;
    private readonly Dot[] _logicDots;
    private readonly Ellipse[] _visualDots;

    // 点宽高
    private double _dotSize;
    // 每次调用UpdateLogicState()时累加
    private int _updateCount;

    // 渲染时间控制
    private TimeSpan _lastTime = TimeSpan.Zero;
    private TimeSpan _accumulator = TimeSpan.Zero;

    private bool _disposed;

    #endregion 字段

    /// <summary>
    /// 初始化<see cref="LoadingCircle"/>的实例
    /// </summary>
    public LoadingCircle()
    {
        _canvas = new Canvas
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            VerticalAlignment = VerticalAlignment.Stretch,
            Background = Brushes.Transparent
        };

        Content = _canvas;

        _logicDots = new Dot[DOT_COUNT];
        _visualDots = new Ellipse[DOT_COUNT];

        for (int i = 0; i < DOT_COUNT; i++)
        {
            // 创建逻辑点，等待Resize回调设置
            _logicDots[i] = new Dot(new Point(0, 0), 0);

            // 创建视觉点
            Ellipse ellipse = new()
            {
                Width = _dotSize,
                Height = _dotSize,
                Opacity = 0,
                Fill = Foreground,
                IsHitTestVisible = false,
                RenderTransform = new TranslateTransform()
            };

            Canvas.SetLeft(ellipse, 0);
            Canvas.SetTop(ellipse, 0);

            _visualDots[i] = ellipse;
            _canvas.Children.Add(ellipse);
        }

        SizeChanged += OnSizeChanged;
        Loaded += (_, _) => Start();
        Unloaded += (_, _) => Stop();
    }

    ~LoadingCircle()
    {
        Dispose(false);
    }

    #region 属性

    /// <summary>
    /// 获取或设置前景画刷
    /// </summary>
    public new Brush Foreground
    {
        get => (Brush)GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    /// <summary>
    /// 获取或设置背景画刷
    /// </summary>
    public new Brush Background
    {
        get => (Brush)GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
    }

    /// <summary>
    /// 获取或设置是否激活动画
    /// </summary>
    public bool IsActive
    {
        get => (bool)GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }

    #endregion 属性

    #region 私有方法

    private void OnRendering(object sender, EventArgs e)
    {
        if (DesignerProperties.GetIsInDesignMode(this))
            return;

        RenderingEventArgs args = (RenderingEventArgs)e;

        if (_lastTime == TimeSpan.Zero)
        {
            _lastTime = args.RenderingTime;
            return;
        }

        TimeSpan dt = args.RenderingTime - _lastTime;
        _lastTime = args.RenderingTime;

        _accumulator += dt;

        bool isDirty = false;

        // 对齐渲染
        while (_accumulator.TotalMilliseconds >= LOGIC_STEP_MS)
        {
            // 更新逻辑点
            UpdateLogicState();
            _accumulator -= TimeSpan.FromMilliseconds(LOGIC_STEP_MS);
            isDirty = true;
        }

        // 更新动画帧
        if (isDirty)
            UpdateVisuals();
    }

    /// <summary>
    /// 逻辑点更新
    /// </summary>
    private void UpdateLogicState()
    {
        // 重置检查
        if (_logicDots.All(d => d.Opacity <= 0))
        {
            _updateCount = 0;

            foreach (Dot dot in _logicDots)
                dot.Reset();
        }

        // 逻辑点更新
        for (int i = 0; i < DOT_COUNT; i++)
        {
            if (_updateCount++ <= i * DOT_UPDATE_OFFSET)
                continue;

            _logicDots[i].Action();
        }
    }

    /// <summary>
    /// 渲染帧更新
    /// </summary>
    private void UpdateVisuals()
    {
        for (int i = 0; i < DOT_COUNT; i++)
        {
            Dot dot = _logicDots[i];
            Ellipse visual = _visualDots[i];

            visual.Opacity = dot.Opacity / 255.0;

            if (visual.RenderTransform is not TranslateTransform transform)
                continue;

            transform.X = dot.Location.X - _dotSize / 2;
            transform.Y = dot.Location.Y - _dotSize / 2;
        }
    }

    private void Start()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(LoadingCircle));

        _lastTime = TimeSpan.Zero;
        _accumulator = TimeSpan.Zero;

        _updateCount = 0;

        foreach (Dot dot in _logicDots)
            dot.Reset();

        CompositionTarget.Rendering += OnRendering;
    }

    private void Stop()
    {
        CompositionTarget.Rendering -= OnRendering;

        foreach (Ellipse v in _visualDots)
            v.Opacity = 0;
    }

    private void UpdateForeground(Brush brs)
    {
        if (_visualDots == null)
            return;

        foreach (Ellipse v in _visualDots)
            v.Fill = brs;
    }

    private void UpdateBackground(Brush brs)
    {
        _canvas.Background = brs;
    }

    #endregion 私有方法

    #region 事件处理

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        // 保持正圆布局
        double size = Math.Min(e.NewSize.Width, e.NewSize.Height);
        _dotSize = size / 10.0;

        Point center = new(e.NewSize.Width / 2, e.NewSize.Height / 2);
        double radius = size / 2 - _dotSize;

        if (radius <= 0)
            return;

        // 更新所有点的物理参数
        foreach (Dot logic in _logicDots)
            logic.UpdateLayout(center, radius);

        // 更新所有点的视觉尺寸
        foreach (Ellipse visual in _visualDots)
        {
            visual.Width = _dotSize;
            visual.Height = _dotSize;
        }
    }

    #endregion 事件处理

    #region 实现IDisposable

    /// <summary>
    /// 释放资源
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 释放资源，取消事件订阅
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
            Stop();

        _disposed = true;
    }

    #endregion 实现IDisposable
}
