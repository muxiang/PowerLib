using PowerControl;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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
            for (int i = 0; i < 10; i++)
            {
                xDataGridView1.Rows.Add(nextRowId++, "123", "ddddddddd", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            xFlowLayoutPanel1.BackColor = Color.LightGray;

            for (int i = 0; i < 5; i++)
            {
                var item = new ProjectListItem(nextId++);
                item.CheckedChanged += (s1, e1) => { XMessageBox.Show("CheckedChanged"); };
                item.DeleteButtonClick += (s1, e1) => xFlowLayoutPanel1.ControlList.Remove((Control)s1);
                xFlowLayoutPanel1.ControlList.Add(item);
            }


            xFlowLayoutPanel1.ControlList.OfType<ProjectListItem>().First().Size = new Size(200, 200);
        }

        private int nextId = 1;
        private int nextRowId = 1;

        private void xButton1_Click(object sender, EventArgs e)
        {
            var item = new ProjectListItem(nextId++);
            item.CheckedChanged += (s1, e1) => { XMessageBox.Show("CheckedChanged"); };
            item.DeleteButtonClick += (s1, e1) => xFlowLayoutPanel1.ControlList.Remove((Control)s1);
            xFlowLayoutPanel1.ControlList.Add(item);
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
