using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using PowerLib.NativeCodes;
using PowerLib.WinForms.Controls;

namespace PowerLib.WinForms.Samples
{
    public partial class FrmXFormTest : XForm
    {
        public FrmXFormTest()
        {
            InitializeComponent();

            xHotkeyRecorder1.HotKey =
                new Tuple<NativeConstants.KeyModifiers, Keys>(
                    NativeConstants.KeyModifiers.Control | NativeConstants.KeyModifiers.Alt, Keys.Y);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            XMessageBox.Show(e.Location.ToString());
            XMessageBox.Show(xHotkeyRecorder1.HotKeyString);
        }

        private void btnCommonButton_Click(object sender, EventArgs e)
        {
            XMessageBox.Show("常规按钮单击", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Height += 50;
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

        private void btnShowLoadingLayerManualClose_Click(object sender, EventArgs e)
        {
            ShowLoadingLayerManualClose();
        }

        private void btnShowLoadingLayerAutoClose_Click(object sender, EventArgs e)
        {
            ShowLoadingLayerAutoClose();
        }

        private void ShowLoadingLayerAutoClose()
        {
            using LoadingLayer ll = new LoadingLayer(this, .8D, true);

            ll.ShowAutoClose(o =>
            {
                LoadingLayer layer = (LoadingLayer)o;

                for (int i = 0; i < 10; i++)
                {
                    Thread.Sleep(500);
                    int progress = (i + 1) * 10;
                    layer.UpdateProgress(progress, $"当前进度 {progress}%");
                }
            }, ll);
        }

        private void ShowLoadingLayerManualClose()
        {
            LoadingLayer ll = new LoadingLayer(this, .8D, true);

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

        private void btnXFileProgressBarTest_Click(object sender, EventArgs e)
        {
            using (FrmXFileProgressBarTest frm = new FrmXFileProgressBarTest())
                frm.ShowDialog();
        }

        private void asdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            XMessageBox.Show("asdasd");
        }

        private void ripplePictureBox1_AfterDrawing(object sender, PaintEventArgs e)
        {
            using Pen p = new Pen(Color.Red, 5);
            e.Graphics.DrawRectangle(p, ripplePictureBox1.ClientRectangle);
        }
    }
}
