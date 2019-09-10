using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using PowerControl;
using PowerControl.IrregularControls;

namespace ControlTest
{
    public partial class FrmShadowPanelTest : XForm
    {
        public FrmShadowPanelTest()
        {
            InitializeComponent();
        }

        private void FrmShadowPanelTest_Load(object sender, EventArgs e)
        {
            
        }

        private int nextId = 1;
        private int nextRowId = 1;

        private void xButton1_Click(object sender, EventArgs e)
        {

        }

        private void xButton2_Click(object sender, EventArgs e)
        {
            xDataGridView1.Rows.Add(nextRowId++, "123", "ddddddddd", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }

        private void xButton3_Click(object sender, EventArgs e)
        {
            xDataGridView1.Rows.RemoveAt(xDataGridView1.RowCount - 1);
        }
    }
}
