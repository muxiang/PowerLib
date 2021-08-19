﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PowerLib.Winform.Controls
{
    public partial class XShadowPanel : Panel
    {
        // 缓存画笔画刷
        private SolidBrush _brsTopBorderColor;
        private LinearGradientBrush _brsBackground;

        private Color _topBorderColor = Color.FromArgb(224, 224, 224);
        private Color _shadowColor = Color.FromArgb(245, 245, 245);
        private float _shadowHeightRatio = 5F / 100;
        private const int TOP_BORDER_WIDTH = 2;

        public XShadowPanel()
        {
            InitializeComponent();

            _brsTopBorderColor = new SolidBrush(_topBorderColor);
            _brsBackground = new LinearGradientBrush(new PointF(Width / 2F, TOP_BORDER_WIDTH),
                new PointF(Width / 2F, TOP_BORDER_WIDTH + Height * _shadowHeightRatio), _shadowColor, BackColor);
        }

        /// <summary>
        /// 顶部边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定顶部边框的颜色")]
        public Color TopBorderColor
        {
            get => _topBorderColor;
            set
            {
                _topBorderColor = value;
                _brsTopBorderColor = new SolidBrush(_topBorderColor);
                Invalidate();
            }
        }

        /// <summary>
        /// 阴影颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定阴影的颜色")]
        public Color ShadowColor
        {
            get => _shadowColor;
            set
            {
                _shadowColor = value;
                _brsBackground = new LinearGradientBrush(new PointF(Width / 2F, TOP_BORDER_WIDTH),
                    new PointF(Width / 2F, TOP_BORDER_WIDTH + Height * _shadowHeightRatio), _shadowColor, BackColor);
                Invalidate();
            }
        }

        /// <summary>
        /// 阴影高度占比
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("指定阴影的高度占比")]
        [DefaultValue(.05F)]
        public float ShadowHeightRatio
        {
            get => _shadowHeightRatio;
            set
            {
                _shadowHeightRatio = value;
                _brsBackground = new LinearGradientBrush(new PointF(Width / 2F, TOP_BORDER_WIDTH),
                    new PointF(Width / 2F, TOP_BORDER_WIDTH + Height * _shadowHeightRatio), _shadowColor, BackColor);
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // 顶部边框
            pe.Graphics.FillRectangle(_brsTopBorderColor, 0, 0, Width, TOP_BORDER_WIDTH);
            // 阴影
            pe.Graphics.FillRectangle(_brsBackground, 0, TOP_BORDER_WIDTH, Width, Height * _shadowHeightRatio);
        }
        
        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            _brsBackground = new LinearGradientBrush(new PointF(Width / 2F, TOP_BORDER_WIDTH),
                new PointF(Width / 2F, TOP_BORDER_WIDTH + Height * _shadowHeightRatio), _shadowColor, BackColor);
            Invalidate();
        }

        protected override void OnResize(EventArgs eventargs)
        {
            base.OnResize(eventargs);
            if (Width == 0)
                return;

            _brsBackground = new LinearGradientBrush(new PointF(Width / 2F, TOP_BORDER_WIDTH),
                new PointF(Width / 2F, TOP_BORDER_WIDTH + Height * _shadowHeightRatio), _shadowColor, BackColor);
            Invalidate();
        }
    }
}
