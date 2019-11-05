using System;
using System.Drawing;
using System.Windows.Forms;

namespace PowerControl
{
    /// <summary>
    /// 表示一个包含加载状态动画的遮罩层
    /// </summary>
    /// <inheritdoc />
    public sealed class LoadingLayer : IDisposable
    {
        private readonly Form _form;
        private readonly LoadingCircle _circle;
        private readonly XProgressBar _progressBar;

        /// <summary>
        /// 构造一个加载遮罩层
        /// </summary>
        /// <param name="parent">父窗口</param>
        /// <param name="opacity">由一个0~1的double值表示的透明度</param>
        /// <param name="progressBar">
        /// 指示遮罩层是否包含一个进度条，在另一线程中调用 <see cref="UpdateProgress"/>更新进度条
        /// </param>
        public LoadingLayer(Form parent, double opacity = .5D, bool progressBar = false)
        {
            _form = new Form
            {
                Size = parent.Size,
                Location = parent.Location,
                Opacity = opacity,
                ShowInTaskbar = false,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black,
                StartPosition = FormStartPosition.CenterParent,
                UseWaitCursor = true
            };

            int wh = Math.Min(parent.Width / 2, parent.Height / 2);
            _circle = new LoadingCircle
            {
                Size = new Size(wh, wh),
                Location = new Point(parent.Width / 2 - wh / 2, parent.Height / 2 - wh / 2)
            };

            if (progressBar)
            {
                _progressBar = new XProgressBar
                {
                    Size = new Size(_circle.Width, 50),
                    Location = new Point(_circle.Left, _circle.Bottom + 20),
                    Min = 0,
                    Max = 100
                };

                _circle.Top -= _progressBar.Height + 20;
            }

            _form.Controls.AddRange(new Control[] { _circle, _progressBar });
        }

        /// <summary>
        /// 显示遮罩层，此方法将阻塞当前调用线程，
        /// 调用前请确保在另一线程中调用了Close()
        /// </summary>
        public void Show()
        {
            if (_form.InvokeRequired)
                _form.BeginInvoke(new MethodInvoker(() =>
                {
                    _circle.Switch();
                    _form.ShowDialog();
                }));
            else
            {
                _circle.Switch();
                _form.ShowDialog();
            }
        }

        /// <summary>
        /// 关闭遮罩层
        /// </summary>
        public void Close()
        {
            if (_form.InvokeRequired)
                _form.BeginInvoke(new MethodInvoker(() => _form.Close()));
            else
                _form.Close();
        }

        /// <summary>
        /// 更新进度
        /// </summary>
        /// <param name="value">进度值(0-100)</param>
        /// <param name="text">文本</param>
        public void UpdateProgress(int value, string text = null)
        {
            if (_progressBar == null) return;
            if (value < 0) value = 0;
            if (value > 100) value = 100;

            if (_progressBar.InvokeRequired)
                _progressBar.BeginInvoke(new MethodInvoker(() =>
                {
                    _progressBar.Value = value;
                    if (text != null && _progressBar.Text != text)
                        _progressBar.Text = text;
                }));
            else
            {
                _progressBar.Value = value;
                if (text != null)
                    _progressBar.Text = text;
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (_circle != null)
            {
                if (_circle.InvokeRequired)
                    _circle.BeginInvoke(new MethodInvoker(_circle.Dispose));
                else
                    _circle.Dispose();
            }

            if (_progressBar != null)
            {
                if (_progressBar.InvokeRequired)
                    _progressBar.BeginInvoke(new MethodInvoker(_progressBar.Dispose));
                else
                    _progressBar.Dispose();
            }

            if (_form != null)
            {
                if (_form.InvokeRequired)
                    _form.BeginInvoke(new MethodInvoker(_form.Dispose));
                else
                    _form.Dispose();
            }
        }
    }
}
