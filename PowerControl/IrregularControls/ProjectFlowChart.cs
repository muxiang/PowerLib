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
    public partial class ProjectFlowChart : Control
    {
        private bool _isComputeBySingle = false;
        private bool _isNormalState = true;
        private ComputeApplications _computeApplications = ComputeApplications.Mike21Ladtap | ComputeApplications.PavanDose | ComputeApplications.HyDrus_1D;

        private string _strComputeTarget1;
        private string _strComputeTarget2;
        private string _strComputeTarget3;
        private string _strTarget1App1;
        private string _strTarget1App2;
        private string _strTarget2App1;
        private string _strTarget2App2;
        private string _strTarget3App1;

        private readonly Font _fontNumber = new Font("微软雅黑", 16, FontStyle.Bold);
        private readonly Font _fontText = new Font("微软雅黑", 10, FontStyle.Regular);
        private readonly Pen _penClickBorder = new Pen(Color.FromArgb(106, 172, 241));
        private readonly Brush _brsClickBackground = new SolidBrush(Color.FromArgb(223, 239, 255));

        private RectangleF _rectCAirDos;
        private RectangleF _rectLadtap;
        private RectangleF _rectHydrus_1D;
        private RectangleF _rectPavan;
        private RectangleF _rectDose;
        private RectangleF _rectResult;
        private RectangleF _rectVisualInterface;

        public event EventHandler CAirDosClicked;
        public event EventHandler LadtapClicked;
        public event EventHandler Hydrus_1DClicked;
        public event EventHandler PavanClicked;
        public event EventHandler DoseClicked;
        public event EventHandler ResultClicked;
        public event EventHandler VisualInterfaceClicked;

        public ProjectFlowChart()
        {
            InitializeComponent();
            InitData();
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
            InitData();
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
                InitData();
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
                InitData();
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
                InitData();
                Invalidate();
            }
        }

        /// <summary>
        /// 数据初始化
        /// </summary>
        private void InitData()
        {
            Height = 115;

            _strComputeTarget1
                = _strComputeTarget2
                = _strComputeTarget3
                = _strTarget1App1
                = _strTarget1App2
                = _strTarget2App1
                = _strTarget2App2
                = _strTarget3App1
                = null;

            if (_isComputeBySingle)
            {
                if (_isNormalState)
                {
                    switch (_computeApplications)
                    {
                        case ComputeApplications.Mike21Ladtap:
                            _strComputeTarget1 = "液态流出物";
                            _strTarget1App1 = "Mike21";
                            _strTarget1App2 = "Ladtap";
                            break;
                        case ComputeApplications.CAirDos:
                            _strComputeTarget1 = "气态流出物";
                            _strTarget1App1 = "C-AirDos";
                            break;
                    }
                }
                else
                {
                    switch (_computeApplications)
                    {
                        case ComputeApplications.Mike21Ladtap:
                            _strComputeTarget1 = "液态释放物";
                            _strTarget1App1 = "Mike21";
                            _strTarget1App2 = "Ladtap";
                            break;
                        case ComputeApplications.HyDrus_1D:
                            _strComputeTarget1 = "土壤包气带";
                            _strTarget1App1 = "HyDrus_1D";
                            break;
                        case ComputeApplications.PavanDose:
                            _strComputeTarget1 = "气态释放物";
                            _strTarget1App1 = "Pavan";
                            _strTarget1App2 = "剂量计算";
                            break;
                    }
                }
            }
            else
            {
                switch (_computeApplications)
                {
                    case ComputeApplications.CAirDos | ComputeApplications.Mike21Ladtap:
                        _strComputeTarget1 = "气态流出物";
                        _strTarget1App1 = "C-AirDos";
                        _strComputeTarget2 = "液态流出物";
                        _strTarget2App1 = "Mike21";
                        _strTarget2App2 = "Ladtap";
                        break;
                    case ComputeApplications.PavanDose | ComputeApplications.Mike21Ladtap | ComputeApplications.HyDrus_1D:
                        _strComputeTarget1 = "气态释放物";
                        _strTarget1App1 = "Pavan";
                        _strTarget1App2 = "剂量计算";
                        _strComputeTarget2 = "液态释放物";
                        _strTarget2App1 = "Mike21";
                        _strTarget2App2 = "Ladtap";
                        _strComputeTarget3 = "土壤包气带";
                        _strTarget3App1 = "Hydrus-1D";
                        break;
                    case ComputeApplications.PavanDose | ComputeApplications.Mike21Ladtap:
                        _strComputeTarget1 = "气态释放物";
                        _strTarget1App1 = "Pavan";
                        _strTarget1App2 = "剂量计算";
                        _strComputeTarget2 = "液态释放物";
                        _strTarget2App1 = "Mike21";
                        _strTarget2App2 = "Ladtap";
                        break;
                    case ComputeApplications.PavanDose | ComputeApplications.HyDrus_1D:
                        _strComputeTarget1 = "气态释放物";
                        _strTarget1App1 = "Pavan";
                        _strTarget1App2 = "剂量计算";
                        _strComputeTarget2 = "土壤包气带";
                        _strTarget2App1 = "Hydrus-1D";
                        break;
                    case ComputeApplications.Mike21Ladtap | ComputeApplications.HyDrus_1D:
                        _strComputeTarget1 = "液态释放物";
                        _strTarget1App1 = "Mike21";
                        _strTarget1App2 = "Ladtap";
                        _strComputeTarget2 = "土壤包气带";
                        _strTarget2App1 = "Hydrus-1D";
                        break;
                }

                Height = 115 * (_strComputeTarget3 != null ? 3 : 2);
            }
        }

        /// <summary>
        /// 绑定单击热区
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="rect">单击热区</param>
        /// <returns>是否绑定成功</returns>
        private bool BindClickRectangle(string text, RectangleF rect)
        {
            switch (text)
            {
                case "C-AirDos":
                    _rectCAirDos = rect;
                    break;
                case "Ladtap":
                    _rectLadtap = rect;
                    break;
                case "Pavan":
                    _rectPavan = rect;
                    break;
                case "Hydrus-1D":
                    _rectHydrus_1D = rect;
                    break;
                case "剂量计算":
                    _rectDose = rect;
                    break;
                case "计算结果":
                    _rectResult = rect;
                    break;
                case "可视化":
                    _rectVisualInterface = rect;
                    break;
                default:
                    return false;
            }

            return true;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button != MouseButtons.Left)
                return;

            if (_rectCAirDos.Contains(e.Location))
            {
                OnCAirDosClicked();
                return;
            }
            if (_rectLadtap.Contains(e.Location))
            {
                OnLadtapClicked();
                return;
            }
            if (_rectDose.Contains(e.Location))
            {
                OnDoseClicked();
                return;
            }
            if (_rectHydrus_1D.Contains(e.Location))
            {
                OnHydrus_1DClicked();
                return;
            }
            if (_rectPavan.Contains(e.Location))
            {
                OnPavanClicked();
                return;
            }
            if (_rectResult.Contains(e.Location))
            {
                OnResultClicked();
                return;
            }
            if (_rectVisualInterface.Contains(e.Location))
                OnVisualInterfaceClicked();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 绘图
            Graphics g = e.Graphics;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            const int padding = 10;
            const int diameter = 50;
            const int linkWidth = 6;
            const int linkLength = 258;
            const int lineHeight = 115;
            const int borderPadding = 8;

            #region 背景

            LinearGradientBrush brs = new LinearGradientBrush(
                    DisplayRectangle,
                    Color.FromArgb(253, 123, 119),
                    Color.FromArgb(139, 145, 255),
                    LinearGradientMode.Horizontal);

            ColorBlend cb = new ColorBlend
            {
                Positions = new[] { 0, .25F, .5F, .75F, 1 },
                Colors = new[]
                {
                    Color.FromArgb(253, 123, 119),
                    Color.FromArgb(239, 135, 66),
                    Color.FromArgb(191, 79, 121),
                    Color.FromArgb(136, 105, 202),
                    Color.FromArgb(139, 145, 255),
                }
            };

            brs.InterpolationColors = cb;

            GraphicsPath gp = new GraphicsPath(FillMode.Winding);
            // 第一行
            gp.AddRectangle(new RectangleF(padding + diameter, padding + diameter / 2F - linkWidth / 2F, 1215, linkWidth));
            // 1
            gp.AddEllipse(padding, padding, diameter, diameter);
            // 2
            gp.AddEllipse(padding + diameter + linkLength, padding, diameter, diameter);
            // 3
            if (_strTarget1App2 == null)
                gp.AddEllipse(padding + diameter * 2 + linkLength * 2, padding, diameter, diameter);
            else
            {
                gp.AddEllipse(padding + diameter * 2 + linkLength * 2 - diameter * 1.5F, padding, diameter, diameter);
                gp.AddEllipse(padding + diameter * 2 + linkLength * 2 + diameter * 1.5F, padding, diameter, diameter);
            }
            // 4
            gp.AddEllipse(padding + diameter * 3 + linkLength * 3, padding, diameter, diameter);
            // 5
            gp.AddEllipse(padding + diameter * 4 + linkLength * 4, padding, diameter, diameter);

            // 第二行
            if (_strComputeTarget2 != null)
            {
                // 横线
                gp.AddRectangle(new RectangleF(padding + diameter + linkLength / 2F, lineHeight + padding + diameter / 2F - linkWidth / 2F,
                    linkLength * 2 + diameter * 2 + linkWidth, linkWidth));

                // 左竖线
                gp.AddRectangle(new RectangleF(padding + diameter + linkLength / 2F, padding + diameter / 2F - linkWidth / 2F,
                    linkWidth, lineHeight));

                // 右竖线
                gp.AddRectangle(new RectangleF(padding + diameter * 3 + linkLength * 2 + linkLength / 2F, padding + diameter / 2F - linkWidth / 2F,
                    linkWidth, lineHeight));

                // 2
                RectangleF rect = new RectangleF(padding + diameter + linkLength, padding + lineHeight, diameter, diameter);
                gp.AddPolygon(Utilities.GetHexagonBySquare(rect));
                // 3
                if (_strTarget2App2 == null)
                {
                    rect = new RectangleF(padding + diameter * 2 + linkLength * 2, padding + lineHeight, diameter, diameter);
                    gp.AddPolygon(Utilities.GetHexagonBySquare(rect));
                }
                else
                {
                    // 3-1
                    rect = new RectangleF(padding + diameter * 2 + linkLength * 2 - diameter * 1.5F, padding + lineHeight, diameter, diameter);
                    gp.AddPolygon(Utilities.GetHexagonBySquare(rect));
                    // 3-2
                    rect = new RectangleF(padding + diameter * 2 + linkLength * 2 + diameter * 1.5F, padding + lineHeight, diameter, diameter);
                    gp.AddPolygon(Utilities.GetHexagonBySquare(rect));
                }
            }

            // 第三行
            if (_strComputeTarget3 != null)
            {
                // 横线
                gp.AddRectangle(new RectangleF(padding + diameter + linkLength / 2F, lineHeight * 2 + padding + diameter / 2F - linkWidth / 2F,
                    linkLength * 2 + diameter * 2 + linkWidth, linkWidth));

                // 左竖线
                gp.AddRectangle(new RectangleF(padding + diameter + linkLength / 2F, lineHeight + padding + diameter / 2F - linkWidth / 2F,
                    linkWidth, lineHeight));

                // 右竖线
                gp.AddRectangle(new RectangleF(padding + diameter * 3 + linkLength * 2 + linkLength / 2F, lineHeight + padding + diameter / 2F - linkWidth / 2F,
                    linkWidth, lineHeight));

                // 2
                Rectangle rect = new Rectangle(padding + diameter + linkLength, padding + lineHeight * 2 + 5, diameter, diameter - 10);
                gp.AddPath(Utilities.GetRoundedRectPath(rect, 15), true);
                // 3
                rect = new Rectangle(padding + diameter * 2 + linkLength * 2, padding + lineHeight * 2 + 5, diameter, diameter - 10);
                gp.AddPath(Utilities.GetRoundedRectPath(rect, 15), true);
            }

            g.FillPath(brs, gp);

            #endregion 背景

            #region 前景

            // 第一行
            // 1
            SizeF szNumber1 = g.MeasureString("1", _fontNumber);
            g.DrawString("1", _fontNumber, Brushes.White, padding + diameter / 2F - szNumber1.Width / 2F, padding + diameter / 2F - szNumber1.Height / 2F);
            string strState = _isNormalState ? "正常工况" : "事故工况";
            SizeF szText1 = g.MeasureString(strState, _fontText);
            g.DrawString(strState, _fontText, Brushes.Black, padding + diameter / 2F - szText1.Width / 2F, padding * 3 + diameter);
            // 2
            SizeF szNumber2 = g.MeasureString("2", _fontNumber);
            g.DrawString("2", _fontNumber, Brushes.White,
                padding + diameter + linkLength + diameter / 2F - szNumber2.Width / 2F,
                padding + diameter / 2F - szNumber2.Height / 2F);
            SizeF szText2 = g.MeasureString(_strComputeTarget1, _fontText);
            g.DrawString(_strComputeTarget1, _fontText, Brushes.Black,
                padding + diameter + linkLength + diameter / 2F - szText2.Width / 2F,
                padding * 3 + diameter);

            Rectangle rectText, rectBorder;

            // 3
            if (_strTarget1App2 == null)
            {
                // 3
                SizeF szNumber3 = g.MeasureString("3", _fontNumber);
                g.DrawString("3", _fontNumber, Brushes.White,
                    padding + diameter * 2 + linkLength * 2 + diameter / 2F - szNumber3.Width / 2F,
                    padding + diameter / 2F - szNumber3.Height / 2F);

                SizeF szText3 = g.MeasureString(_strTarget1App1, _fontText);
                rectText = new Rectangle(
                    (int)(padding + diameter * 2 + linkLength * 2 + diameter / 2F - szText3.Width / 2F),
                    padding * 3 + diameter,
                    (int)szText3.Width, (int)szText3.Height);

                rectBorder = new Rectangle(rectText.Location, rectText.Size);
                rectBorder.Inflate(borderPadding, borderPadding);

                if (BindClickRectangle(_strTarget1App1, rectBorder))
                {
                    g.DrawRectangle(_penClickBorder, rectBorder);
                    rectBorder.Inflate(-1, -1);
                    g.FillRectangle(_brsClickBackground, rectBorder);
                }
                g.DrawString(_strTarget1App1, _fontText, Brushes.Black, rectText.Location);
            }
            else
            {
                // 3-1
                SizeF szNumber3_1 = g.MeasureString("3-1", _fontNumber);
                g.DrawString("3-1", _fontNumber, Brushes.White,
                    padding + diameter * 2 + linkLength * 2 + diameter / 2F - szNumber3_1.Width / 2F
                                                                             - diameter * 1.5F,
                    padding + diameter / 2F - szNumber3_1.Height / 2F);

                SizeF szText3_1 = g.MeasureString(_strTarget1App1, _fontText);
                rectText = new Rectangle((int)(padding + diameter * 2 + linkLength * 2 + diameter / 2F - szText3_1.Width / 2F
                                                                                                       - diameter * 1.5F),
                    padding * 3 + diameter, (int)szText3_1.Width, (int)szText3_1.Height);
                rectBorder = new Rectangle(rectText.Location, rectText.Size);
                rectBorder.Inflate(borderPadding, borderPadding);

                if (BindClickRectangle(_strTarget1App1, rectBorder))
                {
                    g.DrawRectangle(_penClickBorder, rectBorder);
                    rectBorder.Inflate(-1, -1);
                    g.FillRectangle(_brsClickBackground, rectBorder);
                }
                g.DrawString(_strTarget1App1, _fontText, Brushes.Black, rectText.Location);

                // 3-2
                SizeF szNumber3_2 = g.MeasureString("3-2", _fontNumber);
                g.DrawString("3-2", _fontNumber, Brushes.White,
                    padding + diameter * 2 + linkLength * 2 + diameter / 2F - szNumber3_2.Width / 2F + diameter * 1.5F,
                    padding + diameter / 2F - szNumber3_2.Height / 2F);

                SizeF szText3_2 = g.MeasureString(_strTarget1App2, _fontText);
                rectText = new Rectangle((int)(padding + diameter * 2 + linkLength * 2 + diameter / 2F - szText3_2.Width / 2F + diameter * 1.5F),
                    padding * 3 + diameter, (int)szText3_2.Width, (int)szText3_2.Height);

                rectBorder = new Rectangle(rectText.Location, rectText.Size);
                rectBorder.Inflate(borderPadding, borderPadding);

                if (BindClickRectangle(_strTarget1App2, rectBorder))
                {
                    g.DrawRectangle(_penClickBorder, rectBorder);
                    rectBorder.Inflate(-1, -1);
                    g.FillRectangle(_brsClickBackground, rectBorder);
                }
                g.DrawString(_strTarget1App2, _fontText, Brushes.Black, rectText.Location);
            }

            // 4
            SizeF szNumber4 = g.MeasureString("4", _fontNumber);
            g.DrawString("4", _fontNumber, Brushes.White,
                padding + diameter * 3 + linkLength * 3 + diameter / 2F - szNumber4.Width / 2F,
                padding + diameter / 2F - szNumber4.Height / 2F);
            SizeF szText4 = g.MeasureString("计算结果", _fontText);

            rectText = new Rectangle((int)(padding + diameter * 3 + linkLength * 3 + diameter / 2F - szText4.Width / 2F),
                padding * 3 + diameter, (int)szText4.Width, (int)szText4.Height);
            rectBorder = new Rectangle(rectText.Location, rectText.Size);
            rectBorder.Inflate(borderPadding, borderPadding);

            if (BindClickRectangle("计算结果", rectBorder))
            {
                g.DrawRectangle(_penClickBorder, rectBorder);
                rectBorder.Inflate(-1, -1);
                g.FillRectangle(_brsClickBackground, rectBorder);
            }
            g.DrawString("计算结果", _fontText, Brushes.Black, rectText.Location);

            // 5
            SizeF szNumber5 = g.MeasureString("5", _fontNumber);
            g.DrawString("5", _fontNumber, Brushes.White,
                padding + diameter * 4 + linkLength * 4 + diameter / 2F - szNumber5.Width / 2F,
                padding + diameter / 2F - szNumber5.Height / 2F);
            SizeF szText5 = g.MeasureString("可视化", _fontText);

            rectText = new Rectangle((int)(padding + diameter * 4 + linkLength * 4 + diameter / 2F - szText5.Width / 2F),
                padding * 3 + diameter, (int)szText5.Width, (int)szText5.Height);
            rectBorder = new Rectangle(rectText.Location, rectText.Size);
            rectBorder.Inflate(borderPadding, borderPadding);

            if (BindClickRectangle("可视化", rectBorder))
            {
                g.DrawRectangle(_penClickBorder, rectBorder);
                rectBorder.Inflate(-1, -1);
                g.FillRectangle(_brsClickBackground, rectBorder);
            }
            g.DrawString("可视化", _fontText, Brushes.Black, rectText.Location);

            // 第二行
            if (_strComputeTarget2 != null)
            {
                // 2
                g.DrawString("2", _fontNumber, Brushes.White,
                    padding + diameter + linkLength + diameter / 2F - szNumber2.Width / 2F,
                    lineHeight + padding + diameter / 2F - szNumber2.Height / 2F);
                szText2 = g.MeasureString(_strComputeTarget2, _fontText);
                g.DrawString(_strComputeTarget2, _fontText, Brushes.Black,
                    padding + diameter + linkLength + diameter / 2F - szText2.Width / 2F,
                    lineHeight + padding * 3 + diameter);

                // 3
                if (_strTarget2App2 == null)
                {
                    // 3
                    SizeF szNumber3 = g.MeasureString("3", _fontNumber);
                    g.DrawString("3", _fontNumber, Brushes.White,
                        padding + diameter * 2 + linkLength * 2 + diameter / 2F - szNumber3.Width / 2F,
                        lineHeight + padding + diameter / 2F - szNumber3.Height / 2F);
                    SizeF szText3 = g.MeasureString(_strTarget2App1, _fontText);

                    rectText = new Rectangle((int)(padding + diameter * 2 + linkLength * 2 + diameter / 2F - szText3.Width / 2F),
                        lineHeight + padding * 3 + diameter,
                        (int)szText3.Width, (int)szText3.Height);

                    rectBorder = new Rectangle(rectText.Location, rectText.Size);
                    rectBorder.Inflate(borderPadding, borderPadding);

                    if (BindClickRectangle(_strTarget2App1, rectBorder))
                    {
                        g.DrawRectangle(_penClickBorder, rectBorder);
                        rectBorder.Inflate(-1, -1);
                        g.FillRectangle(_brsClickBackground, rectBorder);
                    }
                    g.DrawString(_strTarget2App1, _fontText, Brushes.Black, rectText.Location);
                }
                else
                {
                    // 3-1
                    SizeF szNumber3_1 = g.MeasureString("3-1", _fontNumber);
                    g.DrawString("3-1", _fontNumber, Brushes.White,
                        padding + diameter * 2 + linkLength * 2 + diameter / 2F - szNumber3_1.Width / 2F
                                                                                - diameter * 1.5F,
                        lineHeight + padding + diameter / 2F - szNumber3_1.Height / 2F);
                    SizeF szText3_1 = g.MeasureString(_strTarget2App1, _fontText);

                    rectText = new Rectangle((int)(padding + diameter * 2 + linkLength * 2 + diameter / 2F - szText3_1.Width / 2F - diameter * 1.5F),
                        lineHeight + padding * 3 + diameter,
                        (int)szText3_1.Width, (int)szText3_1.Height);

                    rectBorder = new Rectangle(rectText.Location, rectText.Size);
                    rectBorder.Inflate(borderPadding, borderPadding);

                    if (BindClickRectangle(_strTarget2App1, rectBorder))
                    {
                        g.DrawRectangle(_penClickBorder, rectBorder);
                        rectBorder.Inflate(-1, -1);
                        g.FillRectangle(_brsClickBackground, rectBorder);
                    }
                    g.DrawString(_strTarget2App1, _fontText, Brushes.Black, rectText.Location);

                    // 3-2
                    SizeF szNumber3_2 = g.MeasureString("3-2", _fontNumber);
                    g.DrawString("3-2", _fontNumber, Brushes.White,
                        padding + diameter * 2 + linkLength * 2 + diameter / 2F - szNumber3_2.Width / 2F
                        + diameter * 1.5F,
                        lineHeight + padding + diameter / 2F - szNumber3_2.Height / 2F);
                    SizeF szText3_2 = g.MeasureString(_strTarget2App2, _fontText);

                    rectText = new Rectangle((int)(padding + diameter * 2 + linkLength * 2 + diameter / 2F - szText3_2.Width / 2F + diameter * 1.5F),
                        lineHeight + padding * 3 + diameter,
                        (int)szText3_2.Width, (int)szText3_2.Height);

                    rectBorder = new Rectangle(rectText.Location, rectText.Size);
                    rectBorder.Inflate(borderPadding, borderPadding);

                    if (BindClickRectangle(_strTarget2App2, rectBorder))
                    {
                        g.DrawRectangle(_penClickBorder, rectBorder);
                        rectBorder.Inflate(-1, -1);
                        g.FillRectangle(_brsClickBackground, rectBorder);
                    }
                    g.DrawString(_strTarget2App2, _fontText, Brushes.Black, rectText.Location);
                }
            }

            // 第三行
            if (_strComputeTarget3 != null)
            {
                // 2
                g.DrawString("2", _fontNumber, Brushes.White,
                    padding + diameter + linkLength + diameter / 2F - szNumber2.Width / 2F,
                    lineHeight * 2 + padding + diameter / 2F - szNumber2.Height / 2F);
                szText2 = g.MeasureString(_strComputeTarget3, _fontText);
                g.DrawString(_strComputeTarget3, _fontText, Brushes.Black,
                    padding + diameter + linkLength + diameter / 2F - szText2.Width / 2F,
                    lineHeight * 2 + padding * 3 + diameter);

                // 3
                SizeF szNumber3 = g.MeasureString("3", _fontNumber);
                g.DrawString("3", _fontNumber, Brushes.White,
                    padding + diameter * 2 + linkLength * 2 + diameter / 2F - szNumber3.Width / 2F,
                    lineHeight * 2 + padding + diameter / 2F - szNumber3.Height / 2F);
                SizeF szText3 = g.MeasureString(_strTarget3App1, _fontText);

                rectText = new Rectangle((int)(padding + diameter * 2 + linkLength * 2 + diameter / 2F - szText3.Width / 2F),
                    lineHeight * 2 + padding * 3 + diameter,
                    (int)szText3.Width, (int)szText3.Height);

                rectBorder = new Rectangle(rectText.Location, rectText.Size);
                rectBorder.Inflate(borderPadding, borderPadding);

                if (BindClickRectangle(_strTarget3App1, rectBorder))
                {
                    g.DrawRectangle(_penClickBorder, rectBorder);
                    rectBorder.Inflate(-1, -1);
                    g.FillRectangle(_brsClickBackground, rectBorder);
                }

                g.DrawString(_strTarget3App1, _fontText, Brushes.Black, rectText.Location);
            }

            #endregion 前景
        }

        protected virtual void OnCAirDosClicked()
        {
            CAirDosClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLadtapClicked()
        {
            LadtapClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnHydrus_1DClicked()
        {
            Hydrus_1DClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnPavanClicked()
        {
            PavanClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnDoseClicked()
        {
            DoseClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnResultClicked()
        {
            ResultClicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnVisualInterfaceClicked()
        {
            VisualInterfaceClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
