using PowerControl;
using System;

namespace ControlTest
{
    public partial class FrmShadowPanelTest : XForm
    {
        public FrmShadowPanelTest()
        {
            InitializeComponent();
        }

        private void FrmShadowPanelTest_Load(object sender, System.EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                xDataGridView1.Rows.Add(i + 1, "123", "ddddddddd", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }
    }
}
