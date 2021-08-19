using System;
using System.Threading;
using System.Windows.Forms;

using PowerLib.Winform.Controls;

namespace PowerLib.Winform.Demo
{
    public partial class FrmXFormTest : XForm
    {
        public FrmXFormTest()
        {
            InitializeComponent();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            XMessageBox.Show(e.Location.ToString());
        }

        private void btnCommonButton_Click(object sender, EventArgs e)
        {
            XMessageBox.Show("常规按钮单击", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ddbi1_Click(object sender, EventArgs e)
        {
            XMessageBox.Show("下拉项1单击");
        }

        private void ddbi2_Click(object sender, EventArgs e)
        {
            XMessageBox.Show("下拉项2单击");
        }

        private void FrmXFormTest_Load(object sender, EventArgs e)
        {
            loadingCircle1.Switch();
        }

        private void btnLoadingLayer_Click(object sender, EventArgs e)
        {
            using (LoadingLayer ll = new LoadingLayer(this, .8D, true))
            {
                ThreadPool.QueueUserWorkItem(o =>
                {
                    LoadingLayer layer = (LoadingLayer)o;

                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(500);
                        int progress = (i + 1) * 10;
                        layer.UpdateProgress(progress, $"当前进度 {progress}%");
                    }

                    layer.Close();
                }, ll);

                ll.Show();
            }
        }
    }
}
