using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ControlTest
{
    public partial class FrmFormTest : Form
    {
        public FrmFormTest()
        {
            InitializeComponent();
        }

        private void xButton1_Click(object sender, EventArgs e)
        {
            xButton1.Dock = DockStyle.Bottom;
        }
    }
}
