using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerControl;

namespace ControlTest
{
    public partial class FrmXFormTest : XForm
    {
        public FrmXFormTest()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        private void xButton1_Click(object sender, EventArgs e)
        {
            xButton1.Dock = DockStyle.Bottom;
        }
    }
}
