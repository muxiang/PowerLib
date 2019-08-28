using PowerControl;
using System;
using System.Drawing;
using System.Linq;

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
            flowLayoutPanel1.BackColor = Color.LightGray;
            for (int i = 0; i < 5; i++)
            {
                flowLayoutPanel1.Controls.Add(new ProjectListItem(i + 1));
            }

            flowLayoutPanel1.Controls.OfType<ProjectListItem>().First().Size = new Size(200, 200);
        }
    }
}
