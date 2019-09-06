using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using PowerControl.Properties;

namespace PowerControl.IrregularControls
{
    /// <summary>
    /// 表示工程列表中的一个项
    /// </summary>
    [ToolboxItem(false)]
    public sealed partial class ProjectListItem : Control
    {
        //缓存画笔画刷字体
        private static readonly Pen PenBorder = new Pen(Color.FromArgb(73, 119, 252), 5);
        private static readonly Pen PenCheckedBox = new Pen(Color.FromArgb(217, 220, 231), 2);
        private static readonly Pen PenHorizontalSeparator = new Pen(Color.FromArgb(245, 246, 250), 2);
        private static readonly Pen PenVerticalSeparator = new Pen(Color.FromArgb(210, 210, 210), 1);
        private static readonly Pen PenBlueDash = new Pen(Color.FromArgb(88, 136, 253), 2) { DashStyle = DashStyle.Dot };
        private static readonly Pen PenPurpleDash = new Pen(Color.FromArgb(138, 101, 247), 2) { DashStyle = DashStyle.Dot };
        private static readonly Pen PenRedDash = new Pen(Color.FromArgb(251, 117, 111), 2) { DashStyle = DashStyle.Dot };
        private static readonly Pen PenCyanDash = new Pen(Color.FromArgb(28, 228, 223), 2) { DashStyle = DashStyle.Dot };
        private static readonly Brush BrsProjectName = new SolidBrush(Color.Black);
        private static readonly Brush BrsText = new SolidBrush(Color.FromArgb(195, 195, 195));
        private static readonly Brush BrsBlueNumber = new LinearGradientBrush(new PointF(138, 113), new PointF(194, 113), Color.FromArgb(103, 160, 254), Color.FromArgb(88, 135, 253));
        private static readonly Brush BrsPurpleNumber = new LinearGradientBrush(new PointF(138, 113), new PointF(194, 113), Color.FromArgb(174, 152, 237), Color.FromArgb(135, 97, 248));
        private static readonly Brush BrsRedNumber = new LinearGradientBrush(new PointF(138, 113), new PointF(194, 113), Color.FromArgb(253, 123, 119), Color.FromArgb(248, 99, 89));
        private static readonly Brush BrsCyanNumber = new LinearGradientBrush(new PointF(138, 113), new PointF(194, 113), Color.FromArgb(17, 232, 115), Color.FromArgb(54, 219, 242));
        private static readonly Font FontProjectName = new Font("微软雅黑", 24, FontStyle.Regular, GraphicsUnit.Pixel);
        private static readonly Font FontText = new Font("微软雅黑", 18, FontStyle.Regular, GraphicsUnit.Pixel);

        private bool _checked;
        private bool _isMouseHovering;

        private string _projectStateText = "正常工况";
        private string _computeMethodText = "单一计算";
        private string _plantNameText = "松花江";
        private string _createDateTimeText = "2019/08/28 17:07";
        private string _projectNameText = "工程名称1";
        private int _number;

        private readonly GraphicsPath _graphicsPath;

        #region 事件

        /// <summary>
        /// 选中状态变更时发生
        /// </summary>
        [Browsable(true)]
        public event EventHandler CheckedChanged;

        /// <summary>
        /// 删除按钮单击时发生
        /// </summary>
        [Browsable(true)]
        public event EventHandler DeleteButtonClick;

        #endregion 事件

        /// <inheritdoc />
        /// <summary>
        /// 初始化工程列表项
        /// </summary>
        /// <param name="number">序号</param>
        public ProjectListItem(int number)
        {
            InitializeComponent();

            _number = number;

            Margin = new Padding(30);
            Width = 330;
            Height = 370;
            BackColor = Color.White;

            _graphicsPath = Utilities.GetRoundedRectPath(DisplayRectangle, 15);
            Region = new Region(_graphicsPath);

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
        }

        /// <summary>
        /// 序号
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("序号")]
        [DefaultValue(1)]
        public int Number
        {
            get => _number;
            set
            {
                _number = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 选中状态
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("选中状态")]
        [DefaultValue(false)]
        public bool Checked
        {
            get => _checked;
            set
            {
                _checked = value;
                OnCheckedChanged();
                Invalidate();
            }
        }

        /// <summary>
        /// 工程名称文字
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("工程名称文字")]
        [DefaultValue("工程名称1")]
        public string ProjectNameText
        {
            get => _projectNameText;
            set
            {
                _projectNameText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 工况文字
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("工况文字")]
        [DefaultValue("正常工况")]
        public string ProjectStateText
        {
            get => _projectStateText;
            set
            {
                _projectStateText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 计算方式文字
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("计算方式文字")]
        [DefaultValue("单一计算")]
        public string ComputeMethodText
        {
            get => _computeMethodText;
            set
            {
                _computeMethodText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 厂址文字
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("厂址文字")]
        [DefaultValue("松花江")]
        public string PlantNameText
        {
            get => _plantNameText;
            set
            {
                _plantNameText = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 创建日期文字
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        [Description("创建日期文字")]
        [DefaultValue("2019/08/28 17:07")]
        public string CreateDateTimeText
        {
            get => _createDateTimeText;
            set
            {
                _createDateTimeText = value;
                Invalidate();
            }
        }

        /// <inheritdoc />
        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            pe.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (_isMouseHovering)
                pe.Graphics.DrawPath(PenBorder, _graphicsPath);

            if (_checked)
            {
                pe.Graphics.DrawPath(PenBorder, _graphicsPath);
                pe.Graphics.DrawImage(Resources.ProjectItemChecked, new RectangleF(20, 20, 32, 32));
            }
            else
                pe.Graphics.DrawEllipse(PenCheckedBox, new RectangleF(20, 20, 32, 32));

            //删除按钮
            pe.Graphics.DrawImage(Resources.ProjectItemBtnDelete, new RectangleF(Width - 20 - 32, 20, 32, 32));

            //序号
            Pen pen = null;
            Brush brs = null;
            switch (_number % 4)
            {
                case 1:
                    pen = PenBlueDash;
                    brs = BrsBlueNumber;
                    break;
                case 2:
                    pen = PenPurpleDash;
                    brs = BrsPurpleNumber;
                    break;
                case 3:
                    pen = PenRedDash;
                    brs = BrsRedNumber;
                    break;
                case 0:
                    pen = PenCyanDash;
                    brs = BrsCyanNumber;
                    break;
            }
            //外圆
            pe.Graphics.DrawEllipse(pen ?? throw new InvalidOperationException(), new RectangleF(Width / 2F - 95 / 2F, 65, 95, 95));
            //六边形
            pe.Graphics.FillPolygon(brs, new[]
            {
                new PointF(138, 113),
                new PointF(152, 87),
                new PointF(180, 87),
                new PointF(194, 113),
                new PointF(180, 138),
                new PointF(152, 138),
            });
            //序号
            SizeF szNumber = pe.Graphics.MeasureString(_number.ToString(), FontText);
            pe.Graphics.DrawString(_number.ToString(), FontText, Brushes.White, 
                138 + (194 - 138) / 2F - szNumber.Width / 2F, 87 + (138 - 87) / 2F - szNumber.Height / 2F);

            //工程名称
            pe.Graphics.DrawString(_projectNameText, FontProjectName, BrsProjectName,
                new PointF(Width / 2F - pe.Graphics.MeasureString(_projectNameText, FontProjectName).Width / 2F, 180));

            //竖分割线
            pe.Graphics.DrawLine(PenVerticalSeparator, new PointF(Width / 2F, 225), new PointF(Width / 2F, 245));

            pe.Graphics.SmoothingMode = SmoothingMode.Default;

            //工况
            pe.Graphics.DrawString(_projectStateText, FontText, BrsText,
                new PointF(Width / 2F - pe.Graphics.MeasureString(_projectStateText, FontText).Width - 5, 225 - 2.5F));
            //计算方式
            pe.Graphics.DrawString(_computeMethodText, FontText, BrsText,
                new PointF(Width / 2F + 5, 225 - 2.5F));

            //横分割线
            pe.Graphics.DrawLine(PenHorizontalSeparator, new PointF(5, 280), new PointF(Width - 5, 280));

            //竖分割线
            pe.Graphics.DrawLine(PenVerticalSeparator, new PointF(120, Height - 55), new PointF(120, Height - 35));

            //厂址
            pe.Graphics.DrawString(_plantNameText, FontText, BrsText,
                new PointF(120 - pe.Graphics.MeasureString(_plantNameText, FontText).Width - 5, Height - 55 - 2.5F));
            //日期
            pe.Graphics.DrawString(_createDateTimeText, FontText, BrsText,
                new PointF(120 + 5, Height - 55 - 2.5F));

            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        /// <inheritdoc />
        protected override void OnResize(EventArgs e)
        {
            Width = 330;
            Height = 370;
        }

        /// <inheritdoc />
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            RectangleF rectBtnDelete = new RectangleF(Width - 20 - 32, 20, 32, 32);
            if (rectBtnDelete.Contains(e.Location))
            {
                OnDeleteButtonClick();
                return;
            }

            Checked = !Checked;
        }

        /// <summary>
        /// 鼠标进入时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseEnter(EventArgs e)
        {
            _isMouseHovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        /// <summary>
        /// 鼠标离开时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseLeave(EventArgs e)
        {
            _isMouseHovering = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        private void OnCheckedChanged()
        {
            CheckedChanged?.Invoke(this, EventArgs.Empty);
        }

        private void OnDeleteButtonClick()
        {
            DeleteButtonClick?.Invoke(this, EventArgs.Empty);
        }
    }
}
