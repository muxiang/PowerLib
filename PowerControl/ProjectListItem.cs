using PowerControl.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerControl
{
    [ToolboxItem(false)]
    public partial class ProjectListItem : Control
    {
        //缓存画笔画刷字体
        private Pen _penBorder = new Pen(Color.FromArgb(73, 119, 252), 5);
        private Pen _penCheckedBox = new Pen(Color.FromArgb(217, 220, 231), 2);
        private Pen _penHorizontalSeparator = new Pen(Color.FromArgb(245, 246, 250), 2);
        private Pen _penVerticalSeparator = new Pen(Color.FromArgb(210, 210, 210), 1);
        private Brush _brsProjectName = new SolidBrush(Color.Black);
        private Brush _brsText = new SolidBrush(Color.FromArgb(195, 195, 195));
        private Font _fontProjectName = new Font("微软雅黑", 24, FontStyle.Regular, GraphicsUnit.Pixel);
        private Font _fontText = new Font("微软雅黑", 18, FontStyle.Regular, GraphicsUnit.Pixel);

        private bool _checked;
        private bool _isMouseHovering;

        private string _projectStateText = "正常工况";
        private string _computeMethodText = "单一计算";
        private string _plantNameText = "松花江";
        private string _createDateTimeText = "2019/08/28 17:07";
        private string _projectNameText = "工程名称1";
        private int _number = 1;

        private GraphicsPath _graphicsPath;

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

        protected override void OnPaint(PaintEventArgs pe)
        {
            pe.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            pe.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            pe.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            if (_isMouseHovering)
                pe.Graphics.DrawPath(_penBorder, _graphicsPath);

            if (_checked)
            {
                pe.Graphics.DrawPath(_penBorder, _graphicsPath);
                pe.Graphics.DrawImage(Resources.ProjectItemChecked, new RectangleF(20, 20, 32, 32));
            }
            else
                pe.Graphics.DrawEllipse(_penCheckedBox, new RectangleF(20, 20, 32, 32));

            //删除按钮
            pe.Graphics.DrawImage(Resources.ProjectItemBtnDelete, new RectangleF(Width - 20 - 32, 20, 32, 32));

            //工程名称
            pe.Graphics.DrawString(_projectNameText, _fontProjectName, _brsProjectName,
                new PointF(Width / 2F - pe.Graphics.MeasureString(_projectNameText, _fontProjectName).Width / 2F, 180));

            //竖分割线
            pe.Graphics.DrawLine(_penVerticalSeparator, new PointF(Width / 2F, 225), new PointF(Width / 2F, 245));

            pe.Graphics.SmoothingMode = SmoothingMode.Default;

            //工况
            pe.Graphics.DrawString(_projectStateText, _fontText, _brsText,
                new PointF(Width / 2F - pe.Graphics.MeasureString(_projectStateText, _fontText).Width - 5, 225 - 2.5F));
            //计算方式
            pe.Graphics.DrawString(_computeMethodText, _fontText, _brsText,
                new PointF(Width / 2F + 5, 225 - 2.5F));

            pe.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            //横分割线
            pe.Graphics.DrawLine(_penHorizontalSeparator, new PointF(5, 280), new PointF(Width - 5, 280));

            //竖分割线
            pe.Graphics.DrawLine(_penVerticalSeparator, new PointF(120, Height - 55), new PointF(120, Height - 35));

            //厂址
            pe.Graphics.DrawString(_plantNameText, _fontText, _brsText,
                new PointF(120 - pe.Graphics.MeasureString(_plantNameText, _fontText).Width - 5, Height - 55 - 2.5F));
            //日期
            pe.Graphics.DrawString(_createDateTimeText, _fontText, _brsText,
                new PointF(120 + 5, Height - 55 - 2.5F));
        }

        protected override void OnResize(EventArgs e)
        {
            Width = 330;
            Height = 370;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

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
    }
}
