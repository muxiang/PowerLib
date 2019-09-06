using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerControl.IrregularControls
{
    public partial class ProjectFlowChart : UserControl
    {
        private const int LineHeight = 115;

        private bool _isComputeBySingle = true;
        private bool _isNormalState = true;
        private ComputeApplications _computeApplications = ComputeApplications.Mike21Ladtap;

        public ProjectFlowChart()
        {
            InitializeComponent();
        }

        public ProjectFlowChart(
            bool isComputeBySingle,
            bool isNormalState,
            ComputeApplications computeApplications)
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);

            _isComputeBySingle = isComputeBySingle;
            _isNormalState = isNormalState;
            _computeApplications = computeApplications;
        }

        /// <summary>
        /// 单一计算或联合计算
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("单一计算或联合计算")]
        [DefaultValue(true)]
        public bool IsComputeBySingle
        {
            get => _isComputeBySingle;
            set
            {
                _isComputeBySingle = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 正常工况或事故工况
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("正常工况或事故工况")]
        [DefaultValue(true)]
        public bool IsNormalState
        {
            get => _isNormalState;
            set
            {
                _isNormalState = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 计算程序集合
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("计算程序集合")]
        [DefaultValue(ComputeApplications.Mike21Ladtap)]
        public ComputeApplications ComputeApplications
        {
            get => _computeApplications;
            set
            {
                _computeApplications = value;
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            const int padding = 10;
            const int diameter = 50;
            const int linkWidth = 6;

            const int linkLength1 = 258;
            const int linkLength2 = 252;

            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(padding, padding, diameter, diameter);
            gp.AddRectangle(new RectangleF(padding + diameter, padding + diameter / 2F - linkWidth / 2F, linkLength1, linkWidth));
            gp.AddEllipse(padding + diameter + linkLength1, padding, diameter, diameter);
            gp.AddRectangle(new RectangleF(padding + diameter * 2 + linkLength1, padding + diameter / 2F - linkWidth / 2F, linkLength2, linkWidth));
            gp.AddEllipse(padding + diameter * 2 + linkLength1 + linkLength2, padding, diameter, diameter);
            gp.AddRectangle(new RectangleF(padding + diameter * 3 + linkLength1 + linkLength2, padding + diameter / 2F - linkWidth / 2F, linkLength2, linkWidth));
            gp.AddEllipse(padding + diameter * 3 + linkLength1 + linkLength2 * 2, padding, diameter, diameter);
            gp.AddRectangle(new RectangleF(padding + diameter * 4 + linkLength1 + linkLength2 * 2, padding + diameter / 2F - linkWidth / 2F, linkLength2, linkWidth));
            gp.AddEllipse(padding + diameter * 4 + linkLength1 + linkLength2 * 3, padding, diameter, diameter);

            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            LinearGradientBrush brs = new LinearGradientBrush(
                DisplayRectangle,
                Color.FromArgb(253, 123, 119),
                Color.FromArgb(139, 145, 255),
                LinearGradientMode.Horizontal);

            ColorBlend cb = new ColorBlend();

            cb.Positions = new[]
            {
                0,
                .25F,
                .5F,
                .75F,
                1
            };

            cb.Colors = new[]
            {
                Color.FromArgb(253, 123, 119),
                Color.FromArgb(239, 135, 66),
                Color.FromArgb(191, 79, 121),
                Color.FromArgb(136, 105, 202),
                Color.FromArgb(139, 145, 255),
            };

            brs.InterpolationColors = cb;

            e.Graphics.FillPath(brs, gp);

            if (_isComputeBySingle)
            {
                if (_isNormalState)
                {

                }
            }
            else
            {

            }
        }
    }
}
