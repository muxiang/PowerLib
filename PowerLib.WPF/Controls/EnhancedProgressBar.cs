using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using PowerLib.Utilities;

namespace PowerLib.WPF.Controls
{
    /// <summary>
    /// 表示增强进度条控件
    /// </summary>
    public class EnhancedProgressBar : Control
    {
        static EnhancedProgressBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(EnhancedProgressBar), new FrameworkPropertyMetadata(typeof(EnhancedProgressBar)));
        }

        #region 依赖属性

        /// <summary>
        /// 标识<see cref="Minimum"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty MinimumProperty =
            DependencyProperty.Register(nameof(Minimum), typeof(double), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="Maximum"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty MaximumProperty =
            DependencyProperty.Register(nameof(Maximum), typeof(double), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(100D, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="Value"/>依赖属性
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="Foreground"/>依赖属性，覆盖 Control.Foreground 以提供默认值和 AffectsRender 元数据
        /// </summary>
        public new static readonly DependencyProperty ForegroundProperty =
            DependencyProperty.Register(nameof(Foreground), typeof(Brush), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(Brushes.Cyan, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="Background"/>依赖属性，覆盖 Control.Background 以提供默认值和 AffectsRender 元数据
        /// </summary>
        public new static readonly DependencyProperty BackgroundProperty =
            DependencyProperty.Register(nameof(Background), typeof(Brush), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(Brushes.DimGray, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="BorderBrush"/>依赖属性，覆盖 Control.BorderBrush 以提供默认值和 AffectsRender 元数据
        /// </summary>
        public new static readonly DependencyProperty BorderBrushProperty =
            DependencyProperty.Register(nameof(BorderBrush), typeof(Brush), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(Brushes.DimGray, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="BorderThickness"/>依赖属性，覆盖 Control.BorderThickness 以提供默认值和 AffectsRender 元数据
        /// </summary>
        public new static readonly DependencyProperty BorderThicknessProperty =
            DependencyProperty.Register(nameof(BorderThickness), typeof(Thickness), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(new Thickness(1), FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="CornerRadius"/>依赖属性，表示进度条的圆角半径
        /// </summary>
        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register(nameof(CornerRadius), typeof(double), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(0D, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="ShowTextOutline"/>依赖属性，表示是否显示文字的外包围线以避免重影
        /// </summary>
        public static readonly DependencyProperty ShowTextOutlineProperty =
            DependencyProperty.Register(nameof(ShowTextOutline), typeof(bool), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 标识<see cref="Text"/>依赖属性，表示显示在百分比前方的附加文字
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(nameof(Text), typeof(string), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 文字描边的画笔，为 null 时使用 Foreground
        /// </summary>
        public static readonly DependencyProperty TextOutlineStrokeProperty =
            DependencyProperty.Register(nameof(TextOutlineStroke), typeof(Brush), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.AffectsRender));

        /// <summary>
        /// 文字描边的宽度
        /// </summary>
        public static readonly DependencyProperty TextOutlineThicknessProperty =
            DependencyProperty.Register(nameof(TextOutlineThickness), typeof(double), typeof(EnhancedProgressBar),
                new FrameworkPropertyMetadata(1D, FrameworkPropertyMetadataOptions.AffectsRender));
        
        #endregion 依赖属性

        #region 属性

        /// <summary>
        /// 获取或设置进度条的最小值
        /// </summary>
        public double Minimum
        {
            get => (double)GetValue(MinimumProperty);
            set => SetValue(MinimumProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的最大值
        /// </summary>
        public double Maximum
        {
            get => (double)GetValue(MaximumProperty);
            set => SetValue(MaximumProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的当前值
        /// </summary>
        public double Value
        {
            get => (double)GetValue(ValueProperty);
            set => SetValue(ValueProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的前景画刷（进度填充色）
        /// </summary>
        public new Brush Foreground
        {
            get => (Brush)GetValue(ForegroundProperty);
            set => SetValue(ForegroundProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的进度背景画刷
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
        /// 获取或设置进度条的边框厚度，如果四个边框厚度不同，仅取最大边框厚度进行渲染
        /// </summary>
        public new Thickness BorderThickness
        {
            get => (Thickness)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        /// <summary>
        /// 获取或设置进度条的圆角半径
        /// </summary>
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        /// <summary>
        /// 获取或设置一个布尔值，表示是否显示文字的外包围线以避免重影
        /// </summary>
        public bool ShowTextOutline
        {
            get => (bool)GetValue(ShowTextOutlineProperty);
            set => SetValue(ShowTextOutlineProperty, value);
        }

        /// <summary>
        /// 获取或设置显示在百分比前方的附加文字
        /// </summary>
        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// 获取或设置文字描边的画笔颜色，为 null 时使用 Foreground
        /// </summary>
        public Brush TextOutlineStroke
        {
            get => (Brush)GetValue(TextOutlineStrokeProperty);
            set => SetValue(TextOutlineStrokeProperty, value);
        }

        /// <summary>
        /// 获取或设置文字描边的宽度
        /// </summary>
        public double TextOutlineThickness
        {
            get => (double)GetValue(TextOutlineThicknessProperty);
            set => SetValue(TextOutlineThicknessProperty, value);
        }

        #endregion 属性

        /// <summary>
        /// 初始化<see cref="EnhancedProgressBar"/>的实例
        /// </summary>
        public EnhancedProgressBar() { }

        #region 渲染

        protected override void OnRender(DrawingContext dc)
        {
            double width = ActualWidth;
            double height = ActualHeight;

            if (width <= 0 || height <= 0)
                return;

            double radius = CornerRadius;
            Rect fullRect = new(0, 0, width, height);
            
            RectangleGeometry clipGeometry = new(fullRect, radius, radius);
            dc.PushClip(clipGeometry);

            if (Background != null)
                dc.DrawRectangle(Background, null, new Rect(0, 0, width, height));

            double range = Maximum - Minimum;
            double percentage = range > 0 ? (Value - Minimum) / range * 100.0 : 0;
            percentage = Math.Max(0, Math.Min(100, percentage));

            double drawWidth = width / 100.0 * percentage;

            if (drawWidth > 0)
                dc.DrawRectangle(Foreground, null, new Rect(0, 0, drawWidth, height));

            string displayText = string.IsNullOrEmpty(Text)
                ? $"{Math.Round(percentage, 2)}%"
                : $"{Text}   {Math.Round(percentage, 2)}%";

            Typeface typeface = new(FontFamily, FontStyle, FontWeight, FontStretch);

            if (ShowTextOutline)
            {
                FormattedText ft = CreateFormattedText(displayText, typeface, Foreground);

                double textX = (width - ft.Width) / 2.0;
                double textY = (height - ft.Height) / 2.0;
                Geometry textGeometry = ft.BuildGeometry(new Point(textX, textY));

                dc.DrawGeometry(Background ?? Brushes.Black, null, textGeometry);

                Pen outlinePen = new(TextOutlineStroke ?? Foreground, TextOutlineThickness);
                dc.DrawGeometry(null, outlinePen, textGeometry);
            }
            else
            {
                FormattedText ft = CreateFormattedText(displayText, typeface, Brushes.White);
                double textX = (width - ft.Width) / 2.0;
                double textY = (height - ft.Height) / 2.0;
                dc.DrawText(ft, new Point(textX, textY));
            }
            
            dc.Pop();
            
            if (BorderBrush != null)
            {
                double borderThickness = Math.Max(Math.Max(BorderThickness.Left, BorderThickness.Top),
                    Math.Max(BorderThickness.Right, BorderThickness.Bottom));

                if (borderThickness > 0)
                {
                    Pen borderPen = new(BorderBrush, borderThickness);

                    // 将边框矩形向内缩半个笔宽，使边框完整落在控件内部
                    double half = borderThickness / 2.0;
                    Rect borderRect = new(half, half, width - borderThickness, height - borderThickness);
                    dc.DrawRoundedRectangle(null, borderPen, borderRect, radius, radius);
                }
            }
        }

        #endregion 渲染

        #region 辅助方法

        /// <summary>
        /// 创建 FormattedText 实例
        /// </summary>
        private FormattedText CreateFormattedText(string text, Typeface typeface, Brush foreground)
        {
            return new FormattedText(
                text,
                CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight,
                typeface,
                FontSize,
                foreground);
        }

        #endregion 辅助方法
    }
}
