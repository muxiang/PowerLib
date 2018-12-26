using System;
using System.ComponentModel;
using System.Windows.Forms;

using PowerControl.Design;

namespace PowerControl
{
    /// <summary>
    /// 表示一个下拉按钮的下拉项
    /// </summary>
    [DefaultEvent("Click")]
    [DefaultProperty("Caption")]
    [DesignTimeVisible(false)]
    [ToolboxItem(false)]
    [PropertyTab(typeof(TypeEventsTab), PropertyTabScope.Component)]
    public class DropDownButtonItem : Panel
    {
        #region 构造器

        /// <summary>
        /// 初始化可下拉按钮的实例
        /// </summary>
        public DropDownButtonItem()
        {
            _button = new XButton
            {
                Dock = DockStyle.Fill,
                Location = new System.Drawing.Point(0, 0)
            };

            Caption = Name;
            Controls.Add(_button);
        }

        #endregion 构造器

        #region 属性

        /// <summary>
        /// 显示文本
        /// </summary>
        [Browsable(true)]
        public string Caption
        {
            get => _text;
            set
            {
                _text = value;
                _button.Text = _text;
            }
        }

        /// <summary>
        /// 父按钮
        /// </summary>
        [Browsable(true)]
        public XButton ParentBtn
        {
            get => _parentBtn;
            set
            {
                if (value == null) return;

                _parentBtn = value;
                _button.StartColor = _parentBtn.StartColor;
                _button.EndColor = _parentBtn.EndColor;
                _button.CheckedStartColor = _parentBtn.CheckedStartColor;
                _button.CheckedEndColor = _parentBtn.CheckedEndColor;
                _button.Checked = _parentBtn.Checked;
                Size = _parentBtn.Size;
            }
        }

        /// <summary>
        /// 鼠标单击时发生
        /// </summary>
        public new event EventHandler Click
        {
            add { _button.Click += value; }
            remove { _button.Click -= value; }
        }

        #endregion 属性

        #region 重写

        /// <summary>
        /// 获取一个值，指示控件是否获得焦点
        /// </summary>
        public override bool Focused => _button.Focused;

        #endregion 重写

        #region 字段

        //显示文本
        private string _text;
        //内部按钮
        private XButton _button;
        //父按钮
        private XButton _parentBtn;

        #endregion 字段        
    }
}
