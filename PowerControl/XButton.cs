using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PowerControl
{
    /// <summary>
    /// 表示一个支持下拉列表的按钮
    /// </summary>
    [DefaultEvent("Click")]
    public partial class XButton : Control
    {
        /// <summary>
        /// 按钮状态
        /// </summary>
        enum ButtonState
        {
            Normal,
            Holding,
            Hovering,
            Checked,
            CheckedHolding,
            CheckedHovering,
            Disabled
        }

        /// <summary>
        /// 鼠标停留位置
        /// </summary>
        enum MouseOn
        {
            /// <summary>
            /// 按钮
            /// </summary>
            Button,
            /// <summary>
            /// 下拉区
            /// </summary>
            DropDownArea,
            /// <summary>
            /// 外部
            /// </summary>
            Outside
        }

        /// <summary>
        /// 初始化可下拉按钮的实例
        /// </summary>
        public XButton()
        {
            InitializeComponent();

            StartColor = Color.FromArgb(126, 135, 142);
            EndColor = Color.FromArgb(90, 107, 125);

            CheckedStartColor = Color.FromArgb(186, 185, 181);
            CheckedEndColor = Color.FromArgb(157, 153, 155);

            ForeColor = Color.White;
            Font = new Font("微软雅黑", 8, FontStyle.Bold, GraphicsUnit.Point);

            _borderColor = Color.White;
            _borderWidth = 0;

            _enableLinearGradientColor = true;
            _enableRoundedRectangle = true;

            _displayStyle = XButtonDisplayStyle.Text;

            DropDownItems.AfterItemAdded += DropDownItemAdded;
            DropDownItems.AfterItemRemoved += DropDownItemRemoved;
        }

        #region 属性

        /// <summary>
        /// 显示图像
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Image Image
        {
            get => _image;
            set
            {
                _image = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 是否选中
        /// </summary>
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Appearance")]
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
        /// 渐变起始颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Color StartColor
        {
            get => _startColor;
            set
            {
                _startColor = value;

                _hoveringStartColor = Utilities.GetLighterColor(StartColor);
                _holdingStartColor = Utilities.GetDeeperColor(StartColor);

                Invalidate();
            }
        }

        /// <summary>
        /// 渐变结束颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Color EndColor
        {
            get => _endColor;
            set
            {
                _endColor = value;
                _hoveringEndColor = Utilities.GetLighterColor(EndColor);
                _holdingEndColor = Utilities.GetDeeperColor(EndColor);

                Invalidate();
            }
        }

        /// <summary>
        /// 选中状态渐变起始颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Color CheckedStartColor
        {
            get => _checkedStartColor;
            set
            {
                _checkedStartColor = value;

                _hoveringCheckedStartColor = Utilities.GetLighterColor(CheckedStartColor);
                _holdingCheckedStartColor = Utilities.GetDeeperColor(CheckedStartColor);

                Invalidate();
            }
        }

        /// <summary>
        /// 选中状态渐变结束颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Color CheckedEndColor
        {
            get => _checkedEndColor;
            set
            {
                _checkedEndColor = value;

                _hoveringCheckedEndColor = Utilities.GetLighterColor(CheckedEndColor);
                _holdingCheckedEndColor = Utilities.GetDeeperColor(CheckedEndColor);

                Invalidate();
            }
        }

        /// <summary>
        /// 是否启用线性渐变颜色
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool EnableLinearGradientColor
        {
            get => _enableLinearGradientColor;
            set
            {
                _enableLinearGradientColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 是否启用圆角矩形
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("Appearance")]
        public bool EnableRoundedRectangle
        {
            get => _enableRoundedRectangle;
            set
            {
                _enableRoundedRectangle = value;
                Invalidate();
            }
        }

        /// <summary> 
        /// 指定为此 XButton 显示何种对象（图像还是文本）
        /// </summary>
        [Browsable(true)]
        [DefaultValue(typeof(XButtonDisplayStyle), "Text")]
        [Description("指定为此 XButton 显示何种对象（图像还是文本）")]
        [Category("Appearance")]
        public XButtonDisplayStyle DisplayStyle
        {
            get => _displayStyle;
            set
            {
                _displayStyle = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 边框颜色
        /// </summary>
        [Browsable(true)]
        [Category("Appearance")]
        public Color BorderColor
        {
            get => _borderColor;
            set
            {
                _borderColor = value;
                Invalidate();
            }
        }

        /// <summary>
        /// 边框宽度
        /// </summary>
        [Browsable(true)]
        [DefaultValue(0)]
        [Category("Appearance")]
        public int BorderWidth
        {
            get => _borderWidth;
            set
            {
                _borderWidth = value;
                if (value > 0)
                    _borderPen = new Pen(_borderColor, value);

                Invalidate();
            }
        }

        /// <summary>
        /// 下拉项集合
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public DropDownButtonItemCollection DropDownItems { get; set; } = new DropDownButtonItemCollection();

        #endregion 属性

        #region 字段

        //鼠标停留渐变起止颜色
        private Color _hoveringStartColor;
        private Color _hoveringEndColor;
        //鼠标按下渐变起止颜色
        private Color _holdingStartColor;
        private Color _holdingEndColor;
        //选中状态鼠标停留渐变起止颜色
        private Color _hoveringCheckedStartColor;
        private Color _hoveringCheckedEndColor;
        //选中状态鼠标按下渐变起止颜色
        private Color _holdingCheckedStartColor;
        private Color _holdingCheckedEndColor;

        //文本画刷
        private SolidBrush _textBrush;

        //按钮状态
        private ButtonState _buttonState;

        //鼠标正在停留
        private bool _isMouseHovering;
        //鼠标正在按住
        private bool _isMouseHolding;

        //支持下拉列表时的原按钮响应区
        private Rectangle _buttonRect;
        //支持下拉列表时的下拉箭头响应区
        private Rectangle _dropArrowAreaRect;

        //支持下拉列表时，用于标识鼠标停留在按钮区、下拉箭头区或界外
        private MouseOn _mouseOnButtonOrArrow = MouseOn.Outside;

        //下拉面板
        [NonSerialized]
        private Panel _dropDownPanel;

        //启用线性渐变颜色
        private bool _enableLinearGradientColor;
        //启用圆角矩形
        private bool _enableRoundedRectangle;

        //是否选中
        private bool _checked;

        //渐变起止颜色
        private Color _startColor;
        private Color _endColor;
        //选中状态渐变起止颜色
        private Color _checkedStartColor;
        private Color _checkedEndColor;

        //边框颜色
        private Color _borderColor;
        //边框宽度
        private int _borderWidth = 0;
        //边框画笔
        private Pen _borderPen;
        //显示风格：文字或图像
        private XButtonDisplayStyle _displayStyle;
        //显示图像
        private Image _image;

        #endregion 字段

        #region 常量

        //下拉箭头区宽度
        private const int DropDownArrowWidth = 15;

        #endregion 常量

        #region 事件

        /// <summary>
        /// 鼠标单击时发生
        /// </summary>
        [Browsable(true)]
        public new event EventHandler Click;

        /// <summary>
        /// 下拉列表展开时发生
        /// </summary>
        [Browsable(true)]
        public event EventHandler DropDown;

        #endregion 事件

        #region 方法

        #region 私有方法

        /// <summary>
        /// 更新按钮状态，绘图前调用
        /// </summary>
        private void UpdateButtonState()
        {
            if (Enabled)
            {
                if (Checked)
                {
                    if (_isMouseHolding)
                        _buttonState = ButtonState.CheckedHolding;
                    else if (_isMouseHovering)
                        _buttonState = ButtonState.CheckedHovering;
                    else
                        _buttonState = ButtonState.Checked;
                }
                else
                {
                    if (_isMouseHolding)
                        _buttonState = ButtonState.Holding;
                    else if (_isMouseHovering)
                        _buttonState = ButtonState.Hovering;
                    else
                        _buttonState = ButtonState.Normal;
                }
            }
            else
                _buttonState = ButtonState.Disabled;
        }

        /// <summary>
        /// 获取绘制颜色
        /// </summary>
        /// <param name="btnState">按钮状态</param>
        /// <param name="cStart">渐变开始颜色</param>
        /// <param name="cEnd">渐变结束颜色</param>
        private void GetDrawColor(ButtonState btnState, out Color cStart, out Color cEnd)
        {
            cStart = EnableLinearGradientColor ? StartColor : EndColor;
            cEnd = EndColor;
            _textBrush = new SolidBrush(Enabled ? ForeColor : Color.FromArgb(76, 92, 95));

            switch (btnState)
            {
                case ButtonState.Normal:
                    break;
                case ButtonState.Holding:
                    cStart = EnableLinearGradientColor ? _holdingStartColor : _holdingEndColor;
                    cEnd = _holdingEndColor;
                    break;
                case ButtonState.Hovering:
                    cStart = EnableLinearGradientColor ? _hoveringStartColor : _hoveringEndColor;
                    cEnd = _hoveringEndColor;
                    break;
                case ButtonState.Checked:
                    cStart = EnableLinearGradientColor ? CheckedStartColor : CheckedEndColor;
                    cEnd = CheckedEndColor;
                    break;
                case ButtonState.CheckedHolding:
                    cStart = EnableLinearGradientColor ? _holdingCheckedStartColor : _holdingCheckedEndColor;
                    cEnd = _holdingCheckedEndColor;
                    break;
                case ButtonState.CheckedHovering:
                    cStart = EnableLinearGradientColor ? _hoveringCheckedStartColor : _hoveringCheckedEndColor;
                    cEnd = _hoveringCheckedEndColor;
                    break;
            }
        }

        #endregion 私有方法

        #region 公开方法

        /// <summary>
        /// 触发按钮的 Click 事件。
        /// </summary>
        public void PerformClick()
        {
            Click?.Invoke(this, EventArgs.Empty);
        }

        #endregion 公开方法

        #endregion 方法

        #region 事件处理

        //下拉项添加
        private void DropDownItemAdded(object sender, DropDownButtonItemCollectionChangedEventArgs e)
        {
            //重绘以更新下拉箭头
            Invalidate();

            Form parentForm = Utilities.GetParentForm(this);
            if (parentForm == null) return;

            if ((sender as DropDownButtonItemCollection).Count == 1)
            {
                //构建下拉面板
                _dropDownPanel = new Panel
                {
                    Size = new Size(Width, Height * DropDownItems.Count),
                    BackColor = Checked ? CheckedEndColor : EndColor,
                    Visible = false,
                    TabStop = true,
                };

                _dropDownPanel.LostFocus += (s1, e1) => _dropDownPanel.Hide();

                //添加到父窗口
                parentForm.Click += (s1, e1) =>
                {
                    if (_dropDownPanel.Visible)
                        _dropDownPanel.Hide();
                };
                parentForm.LocationChanged += (s1, e1) => _dropDownPanel.Location = parentForm.PointToClient(Parent.PointToScreen(new Point(Left, Bottom)));
                parentForm.Controls.Add(_dropDownPanel);
            }

            _dropDownPanel.Height = DropDownItems.Count * Height;

            //添加下拉项
            e.ChangedItem.Location = new Point(0, (DropDownItems.Count - 1) * Height);
            e.ChangedItem.ParentBtn = this;
            e.ChangedItem.Click += (s1, e1) => _dropDownPanel.Hide();

            _dropDownPanel.Controls.Add(e.ChangedItem);

        }

        //下拉项移除
        private void DropDownItemRemoved(object sender, DropDownButtonItemCollectionChangedEventArgs e)
        {
            Invalidate();

            if (Parent == null) return;

            _dropDownPanel.Height = DropDownItems.Count * Height;

            _dropDownPanel.Controls.Remove(e.ChangedItem);
        }

        #endregion 事件处理

        #region 重写
        
        /// <summary>
        /// 句柄创建时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (DropDownItems.Count <= 0) return;

            Form parentForm = Utilities.GetParentForm(this);
            if (parentForm == null) return;

            //构建下拉面板
            _dropDownPanel = new Panel
            {
                Location = parentForm.PointToClient(Parent.PointToScreen(new Point(Left, Bottom))),
                Size = new Size(Width, Height * DropDownItems.Count),
                BackColor = Checked ? CheckedEndColor : EndColor,
                Visible = false,
                TabStop = true
            };

            _dropDownPanel.LostFocus += (s1, e1) => _dropDownPanel.Hide();

            //添加到父窗口
            parentForm.Click += (s1, e1) =>
            {
                if (_dropDownPanel.Visible)
                    _dropDownPanel.Hide();
            };
            parentForm.LocationChanged += (s1, e1) => _dropDownPanel.Location = parentForm.PointToClient(Parent.PointToScreen(new Point(Left, Bottom)));
            parentForm.Controls.Add(_dropDownPanel);

            //添加下拉项
            for (int i = 0; i < DropDownItems.Count; ++i)
            {
                DropDownItems[i].Location = new Point(0, i * Height);
                DropDownItems[i].ParentBtn = this;
                DropDownItems[i].Click += (s1, e1) => _dropDownPanel.Hide();

                _dropDownPanel.Controls.Add(DropDownItems[i]);
            }
        }

        /// <summary>
        /// 文本变更时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            Invalidate();
        }

        /// <summary>
        /// 大小变更时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            _buttonRect = new Rectangle(0, 0, Width - DropDownArrowWidth, Height);
            _dropArrowAreaRect = new Rectangle(_buttonRect.Width, 0, DropDownArrowWidth, Height);
            Invalidate();
        }

        /// <summary>
        /// 前景色变更时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);

            _textBrush = new SolidBrush(Enabled ? ForeColor : Color.FromArgb(76, 92, 95));
            Invalidate();
        }

        /// <summary>
        /// 可用状态变更时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
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
        /// 鼠标移动时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            //不支持下拉时返回
            if (DropDownItems.Count == 0) return;

            //根据鼠标坐标，汇总重绘时需要的数据
            if (_buttonRect.Contains(e.Location))
            {
                if (_mouseOnButtonOrArrow == MouseOn.Button) return;

                _isMouseHovering = true;
                _mouseOnButtonOrArrow = MouseOn.Button;
            }
            else if (_dropArrowAreaRect.Contains(e.Location))
            {
                if (_mouseOnButtonOrArrow == MouseOn.DropDownArea) return;

                _isMouseHovering = true;
                _mouseOnButtonOrArrow = MouseOn.DropDownArea;
            }
            else
            {
                if (_mouseOnButtonOrArrow == MouseOn.Outside) return;

                _isMouseHovering = false;
                _mouseOnButtonOrArrow = MouseOn.Outside;
            }

            //重绘
            Invalidate();
            base.OnMouseMove(e);
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

        /// <summary>
        /// 鼠标按下时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            _isMouseHolding = true;
            Invalidate();
            base.OnMouseDown(e);
        }

        /// <summary>
        /// 鼠标抬起时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isMouseHolding = false;
            Invalidate();
            base.OnMouseUp(e);
        }

        /// <summary>
        /// 鼠标单击时调用
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnMouseClick(MouseEventArgs e)
        {
            //左半区
            if (_buttonRect.Contains(e.Location))
            {
                Click?.Invoke(this, EventArgs.Empty);

                if (_dropDownPanel != null && _dropDownPanel.Visible)
                    _dropDownPanel.Hide();
            }
            //下拉箭头区
            else if (_dropArrowAreaRect.Contains(e.Location))
            {
                //不支持下拉时，仅触发单击
                if (DropDownItems.Count == 0)
                    Click?.Invoke(this, EventArgs.Empty);
                else
                {
                    //显示/隐藏下拉列表
                    if (_dropDownPanel.Visible)
                        _dropDownPanel.Hide();
                    else
                    {
                        _dropDownPanel.BringToFront();
                        _dropDownPanel.Show();
                        _dropDownPanel.Focus();
                        DropDown?.Invoke(this, EventArgs.Empty);
                    }
                }
            }

            base.OnMouseClick(e);
        }

        /// <summary>
        /// 重绘时调用
        /// </summary>
        /// <param name="pe">事件参数</param>
        protected override void OnPaint(PaintEventArgs pe)
        {
            //更新按钮状态
            UpdateButtonState();

            DrawBackgroundAndBorder(pe.Graphics);

            switch (DisplayStyle)
            {
                case XButtonDisplayStyle.Text:
                    DrawText(pe.Graphics);
                    break;
                case XButtonDisplayStyle.Image:
                    DrawImage(pe.Graphics);
                    break;
                case XButtonDisplayStyle.ImageAndText:
                    float textX = DrawText(pe.Graphics);
                    DrawImage(pe.Graphics, textX);
                    break;
            }
        }

        #region 绘图逻辑

        /// <summary>
        /// 绘制背景与边框
        /// </summary>
        /// <param name="g">图面</param>
        private void DrawBackgroundAndBorder(Graphics g)
        {
            //获取渐变颜色起止
            Color cStart, cEnd;
            GetDrawColor(_buttonState, out cStart, out cEnd);

            GraphicsPath gPath = null;
            //圆角
            if (EnableRoundedRectangle)
            {
                gPath = Utilities.GetRoundedRectPath(DisplayRectangle, 10);
                Region = new Region(gPath);
            }
            //直角
            else
                Region = new Region(DisplayRectangle);

            //渐变画刷
            LinearGradientBrush brsh = new LinearGradientBrush(new Point(Width / 2, 0), new Point(Width / 2, Height), cStart, cEnd);
            //填充有效区
            g.FillRegion(brsh, Region);

            //存在下拉项
            if (DropDownItems.Count > 0)
            {
                //存在下拉列表项，绘制下拉箭头区
                if (_buttonState.ToString().Contains("Checked"))
                    GetDrawColor(ButtonState.Checked, out cStart, out cEnd);
                else
                    GetDrawColor(ButtonState.Normal, out cStart, out cEnd);

                brsh = new LinearGradientBrush(new Point(Width - DropDownArrowWidth / 2, 0), new Point(Width - DropDownArrowWidth / 2, Height), cStart, cEnd);

                //根据鼠标位置绘制高亮差别
                switch (_mouseOnButtonOrArrow)
                {
                    case MouseOn.Button:
                        g.FillRectangle(brsh, _dropArrowAreaRect);
                        break;
                    case MouseOn.DropDownArea:
                        g.FillRectangle(brsh, _buttonRect);
                        break;
                    case MouseOn.Outside:
                        break;
                }

                //绘制三角
                g.FillPolygon(_textBrush, new[]
                {
                    new Point(_dropArrowAreaRect.Left + 3, Height / 2 - 3),
                    new Point(_dropArrowAreaRect.Right - 3, Height / 2 - 3),
                    new Point(_dropArrowAreaRect.Left + DropDownArrowWidth / 2,Height / 2 + 3),
                });
            }

            //绘制边框
            if (_borderWidth > 0)
                if (gPath == null)
                    g.DrawRectangle(_borderPen, DisplayRectangle);
                else
                    g.DrawPath(_borderPen, gPath);
        }

        /// <summary>
        /// 绘制显示文本
        /// </summary>
        /// <param name="g">图面</param>
        /// <returns>返回文字起始x坐标</returns>
        private float DrawText(Graphics g)
        {
            //测量文本大小
            SizeF szText = g.MeasureString(Text, Font, Size);

            //起始坐标
            PointF drawPt;
            if (DropDownItems.Count == 0)
                drawPt = new PointF(Width / 2 - szText.Width / 2, Height / 2 - szText.Height / 2);
            else
                drawPt = new PointF((Width - DropDownArrowWidth) / 2 - szText.Width / 2, Height / 2 - szText.Height / 2);

            g.DrawString(Text, Font, _textBrush, new RectangleF(drawPt, szText));

            return drawPt.X;
        }

        /// <summary>
        /// 绘制图像
        /// </summary>
        /// <param name="g">图面</param>
        /// <param name="textX">文字起始X坐标，DisplayStyle为Image时，忽略此参数</param>
        private void DrawImage(Graphics g, float textX = -1)
        {
            float szImg = Height - 2 * 2;

            RectangleF drawRect;

            //仅图像
            if (textX < 0)
                drawRect = new RectangleF(Width / 2 - szImg / 2, Height / 2 - szImg / 2, szImg, szImg);
            //图像与文字
            else
                drawRect = new RectangleF(textX - 2 - szImg, Height / 2 - szImg / 2, szImg, szImg);

            if (_image == null) return;
            g.DrawImage(_image, drawRect);
        }

        #endregion 绘图逻辑

        #endregion 重写
    }
}
