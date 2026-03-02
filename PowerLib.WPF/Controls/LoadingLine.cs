using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PowerLib.WPF.Controls;

/// <summary>
/// 表示一个Win8Metro风格的加载横线动画
/// </summary>
public class LoadingLine : ContentControl
{
    /// <summary>
    /// 表示动画中的一个点，负责逻辑位置计算
    /// </summary>
    private sealed class Dot
    {
        #region 常量

        // 最小速度
        private const double MIN_SPEED = .003;
        // 最大速度
        private const double MAX_SPEED = .018;
        // 出现时比例（基于容器宽度，视为1）
        private const double APPEAR_RATIO = 0.1;
        // 减速区比例
        private const double SLOW_RATIO = 0.3;
        // 加速区比例
        private const double QUICK_RATIO = 0.55;
        // 末尾比例
        private const double END_RATIO = 0.9;
        // 淡出负增量
        private const int ALPHA_SUB = 15;

        #endregion 常量

        #region 字段

        // 容器大小
        private Size _szContainer;
        // 位置基于容器宽度的比例 0~1
        private double _ratio;
        // 透明度 0~255
        private int _opacity;
        // 状态机进度 0~3
        private int _progress;
        // 速度
        private double _speed;

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
        /// <param name="szContainer">容器大小</param>
        public Dot(Size szContainer)
        {
            UpdateLayout(szContainer);
            Reset();
        }

        #region 公开方法

        /// <summary>
        /// 更新容器大小以重新计算点位置
        /// </summary>
        /// <param name="szContainer">容器大小</param>
        public void UpdateLayout(Size szContainer)
        {
            _szContainer = szContainer;
            ReCalcLocation();
        }

        /// <summary>
        /// 重置点的状态
        /// </summary>
        public void Reset()
        {
            _ratio = APPEAR_RATIO;
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
                    if (_ratio + _speed >= SLOW_RATIO && _ratio + _speed < QUICK_RATIO) { _progress = 1; _ratio = SLOW_RATIO - _speed; }
                    break;
                case 1:// 减速区，完全不透明，减速
                    SubSpeed();
                    if (_ratio + _speed >= QUICK_RATIO || _ratio + _speed < SLOW_RATIO) { _progress = 2; _ratio = QUICK_RATIO - _speed; }
                    break;
                case 2:// 加速淡出区，减速，透明度递减
                    AddSpeed();
                    if ((_opacity -= ALPHA_SUB) <= 0) _ratio = APPEAR_RATIO;
                    break;
            }

            // 更新点位置
            _ratio = _ratio >= END_RATIO - _speed ? 0 : _ratio + _speed;
            ReCalcLocation();
        }

        #endregion 公开方法

        #region 私有方法

        /// <summary>
        ///  根据圆心、半径、角度重新计算点位置
        /// </summary>
        private void ReCalcLocation()
        {
            Location = new Point(_szContainer.Width * _ratio, _szContainer.Height / 2);
        }

        private void AddSpeed() { if (++_speed > MAX_SPEED) _speed = MAX_SPEED; }
        private void SubSpeed() { if (--_speed < MIN_SPEED) _speed = MIN_SPEED; }

        #endregion 私有方法
    }

    #region 依赖属性

    public new static readonly DependencyProperty ForegroundProperty = DependencyProperty.Register(
        nameof(Foreground), typeof(Brush), typeof(LoadingLine),
        new PropertyMetadata(Brushes.White, (d, e) => ((LoadingLine)d).UpdateForeground((Brush)e.NewValue)));

    public new static readonly DependencyProperty BackgroundProperty = DependencyProperty.Register(
        nameof(Background), typeof(Brush), typeof(LoadingLine),
        new PropertyMetadata(Brushes.Black, (d, e) => ((LoadingLine)d).UpdateBackground((Brush)e.NewValue)));

    public static readonly DependencyProperty IsActiveProperty = DependencyProperty.Register(
        nameof(IsActive), typeof(bool), typeof(LoadingLine),
        new PropertyMetadata(false, (d, e) =>
        {
            if ((bool)e.NewValue) ((LoadingLine)d).Start();
            else ((LoadingLine)d).Stop();
        }));

    #endregion 依赖属性

    #region 常量

    // 点数量
    private const int DOT_COUNT = 5;
    // 点更新间隔，即第一个点更新逻辑位置后，第二个点需要等待多少次UpdateLogicState()调用后再进行更新
    private const int DOT_UPDATE_OFFSET = 80;

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
    // 逻辑帧间隔
    private const double LOGIC_STEP_MS = 1000 / 60D;

    #endregion 字段

    /// <summary>
    /// 初始化<see cref="LoadingLine"/>的实例
    /// </summary>
    public LoadingLine()
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
            _logicDots[i] = new Dot(Size.Empty);

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
        double size = e.NewSize.Width;
        _dotSize = size / 50.0;
        
        double w = ActualWidth;
        double h = ActualHeight;

        // 更新所有点的物理参数
        foreach (Dot logic in _logicDots)
            logic.UpdateLayout(new Size(w, h));

        // 更新所有点的视觉尺寸
        foreach (Ellipse visual in _visualDots)
        {
            visual.Width = _dotSize;
            visual.Height = _dotSize;
        }
    }

    #endregion 事件处理
}