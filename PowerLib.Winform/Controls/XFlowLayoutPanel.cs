using System;
using System.Drawing;
using System.Windows.Forms;

namespace PowerLib.Winform.Controls
{
    /// <summary>
    /// 表示以水平或垂直方式动态布置其内容的面板。
    /// </summary>
    public partial class XFlowLayoutPanel : UserControl
    {
        /// <summary>
        /// 获取包含在控件内的控件的集合
        /// </summary>
        public ControlCollection ControlList => fpnlInner.Controls;

        /// <summary>
        /// 初始化<see cref="XFlowLayoutPanel"/>的实例
        /// </summary>
        public XFlowLayoutPanel()
        {
            InitializeComponent();
            KeepInnerAndScrollBar();

            xScrollBar1.Scroll += (s1, e1) =>
            {
                fpnlInner.AutoScrollPosition = new Point(0, xScrollBar1.Value);
                xScrollBar1.Refresh();
            };

            fpnlInner.ControlAdded += FpnlInner_ControlChanged;
            fpnlInner.ControlRemoved += FpnlInner_ControlChanged;

            fpnlInner.MouseMove += (s1, e1) => { if (!fpnlInner.Focused) fpnlInner.Focus(); };
            fpnlInner.MouseWheel += (s1, e1) => xScrollBar1.Value = Math.Abs(fpnlInner.AutoScrollPosition.Y);
        }

        private void FpnlInner_ControlChanged(object sender, ControlEventArgs e)
        {
            xScrollBar1.Visible = fpnlInner.VerticalScroll.Visible;
            KeepInnerAndScrollBar();
        }

        private void KeepInnerAndScrollBar()
        {
            fpnlInner.Size = Size;

            xScrollBar1.Minimum = 0;
            xScrollBar1.Maximum = fpnlInner.DisplayRectangle.Height;
            xScrollBar1.LargeChange = xScrollBar1.Maximum / xScrollBar1.Height + fpnlInner.Height;
            xScrollBar1.SmallChange = 15;
            xScrollBar1.Value = Math.Abs(fpnlInner.AutoScrollPosition.Y);
        }

        /// <inheritdoc cref="UserControl"/>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            KeepInnerAndScrollBar();
        }
    }
}
