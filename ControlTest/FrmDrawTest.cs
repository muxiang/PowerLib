using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PowerControl;

namespace ControlTest
{
    public partial class FrmDrawTest : XForm
    {
        public FrmDrawTest()
        {
            InitializeComponent();

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
        }
        
        private void xButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Padding.Top.ToString());
        }
    }
}
