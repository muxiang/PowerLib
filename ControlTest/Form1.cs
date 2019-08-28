using PowerControl;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ControlTest
{
    public partial class Form1 : XForm
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void xButton1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("xButton1 Clicked");
        }

        private void ddi1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("下拉项1单击");
        }

        private void ddi2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("xButton3 Clickedaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void xButton2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("xButton2 Clicked");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var item in panel1.Controls.OfType<XButton>())
            {
                item.EnableRoundedRectangle = true;
            }

            radioButton2.Checked = false;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var item in panel1.Controls.OfType<XButton>())
            {
                item.EnableRoundedRectangle = false;
            }

            radioButton1.Checked = false;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (var item in panel1.Controls.OfType<XButton>())
            {
                item.EnableLinearGradientColor = checkBox1.Checked;
            }
        }

        private void xButton3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("xButton3 Clickedaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
        }

        private void xButton1_Click_1(object sender, EventArgs e)
        {
            LoadingLayer ll = new LoadingLayer(this, .7D, true);
            ThreadPool.QueueUserWorkItem(o =>
            {
                Thread.Sleep(1000);
                ll.UpdateProgress(20, "正在读取相位...");
                Thread.Sleep(1000);
                ll.UpdateProgress(30, "正在读取阶段...");
                Thread.Sleep(1000);
                ll.UpdateProgress(40, "正在读取方案...");
                Thread.Sleep(1000);
                ll.UpdateProgress(50, "正在读取时间表...");
                Thread.Sleep(1000);
                ll.UpdateProgress(60, "正在读取调度表...");
                Thread.Sleep(1000);
                ll.UpdateProgress(70, "正在读取手动参数...");
                Thread.Sleep(1000);
                ll.Close();
            });

            ll.Show();
        }

        private void xButton2_Click_1(object sender, EventArgs e)
        {
            loadingCircle1.Switch();
        }

        private void xButton3_Click_1(object sender, EventArgs e)
        {
            XMessageBox.Show("这次怎么样？", "询问", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            XMessageBox.Show("这次怎么样？\r\n这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？", "询问", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Warning);
            XMessageBox.Show("这次怎么样？\r\n这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？这次怎么样？", "询问", MessageBoxButtons.AbortRetryIgnore);
            XMessageBox.Show("这次怎么样？\r\n这次怎么样？\r\n这次怎么样？\r\n这次怎么样？\r\n这次怎么样？\r\n这次怎么样？\r\n这次怎么样？", "询问", MessageBoxButtons.YesNo, MessageBoxIcon.Hand);
            XMessageBox.Show("这次怎么样？\r\n这次怎么样？\r\n这次怎么样？\r\n这次怎么样？\r\n这次怎么样？\r\n这次怎么样？\r\n这次怎么样？", "询问", MessageBoxButtons.YesNo);
        }

        private void Form1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //MessageBox.Show("Test");
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            //MessageBox.Show("Test");
        }

        private async void xButton7_Click(object sender, EventArgs e)
        {
            string x = await ReadAsync();
            MessageBox.Show(x);
        }

        private async Task<string> ReadAsync()
        {
            await Task.Run(() =>
            {
                Thread.Sleep(5000);
            });
            return "asd";
        }

        private void xButton8_Click(object sender, EventArgs e)
        {
            FrmAbout frm = new FrmAbout();
            frm.ShowDialog();
        }
    }
}
