using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PowerLib.Winform.Controls
{
    /// <summary>
    /// 表示一个包含加载状态动画的遮罩层
    /// </summary>
    /// <inheritdoc />
    public sealed class LoadingLayer : IDisposable
    {
        private Form _form;
        private LoadingCircle _circle;
        private XProgressBar _progressBar;

        // 用于遮罩层创建过程中，立即关闭时设置的标记，仅在Close中设置为true，并在HandleCreated事件处理中读取
        private bool _isClosed;

        private readonly Thread _formThread;

        /// <summary>
        /// 构造一个加载遮罩层
        /// </summary>
        /// <param name="rectLayer">遮罩层坐标及尺寸</param>
        /// <param name="opacity">由一个0~1的double值表示的透明度</param>
        /// <param name="progressBar">
        /// 指示遮罩层是否包含一个进度条，在另一线程中调用 <see cref="UpdateProgress"/>更新进度条
        /// </param>
        /// <param name="customCursor">指定自定义的光标</param>
        /// <param name="newUiThread">指示遮罩层是否在独立的UI线程执行</param>
        public LoadingLayer(
            Rectangle rectLayer,
            double opacity = .5D,
            bool progressBar = false,
            Cursor customCursor = null,
            bool newUiThread = false)
        {
            if (newUiThread)
            {
                _formThread = new Thread(() =>
                {
                    CreateForm(rectLayer, opacity, progressBar, customCursor);
                    _circle.Switch();
                    _form.TopMost = true;
                    Application.Run(_form);
                })
                { IsBackground = true };
                _formThread.SetApartmentState(ApartmentState.STA);

                return;
            }

            CreateForm(rectLayer, opacity, progressBar, customCursor);
        }

        /// <summary>
        /// 构造一个加载遮罩层
        /// </summary>
        /// <param name="parentForm">父窗口</param>
        /// <param name="opacity">由一个0~1的double值表示的透明度</param>
        /// <param name="progressBar">
        /// 指示遮罩层是否包含一个进度条，在另一线程中调用 <see cref="UpdateProgress"/>更新进度条
        /// </param>
        /// <param name="customCursor">指定自定义的光标</param>
        /// <param name="newUiThread">指示遮罩层是否在独立的UI线程执行</param>
        public LoadingLayer(
            Form parentForm,
            double opacity = .5D,
            bool progressBar = false,
            Cursor customCursor = null,
            bool newUiThread = false)
            : this(new Rectangle(parentForm.Location, parentForm.Size), opacity, progressBar, customCursor, newUiThread) { }

        private void CreateForm(
            Rectangle rectLayer,
            double opacity = .5D,
            bool progressBar = false,
            Cursor customCursor = null)
        {
            _form = new Form
            {
                Size = rectLayer.Size,
                Location = rectLayer.Location,
                Opacity = opacity,
                ShowInTaskbar = false,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.Black,
                StartPosition = FormStartPosition.Manual,
                UseWaitCursor = customCursor == null
            };

            _form.HandleCreated += (s1, e1) =>
            {
                if (_isClosed)
                    _form.BeginInvoke(new MethodInvoker(_form.Close));
            };

            if (customCursor != null)
                _form.Cursor = customCursor;

            int wh = Math.Min(rectLayer.Width / 2, rectLayer.Height / 2);
            _circle = new LoadingCircle
            {
                Size = new Size(wh, wh),
                Location = new Point(rectLayer.Width / 2 - wh / 2, rectLayer.Height / 2 - wh / 2)
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
        /// 显示遮罩层，如果在构造此<see cref="LoadingLayer"/>时没有指定newUiThread参数为true，
        /// 此方法将阻塞当前调用线程，直到另一线程中调用了Close()
        /// </summary>
        public void Show()
        {
            if (_formThread == null)
            {
                if (_form.InvokeRequired)
                    _form.BeginInvoke(new MethodInvoker(() =>
                    {
                        if (_form.Disposing || _form.IsDisposed)
                            return;

                        if (_form.Visible)
                            return;

                        _circle.Switch();
                        _form.ShowDialog();
                    }));
                else
                {
                    if (_form.Disposing || _form.IsDisposed)
                        return;

                    if (_form.Visible)
                        return;

                    _circle.Switch();
                    _form.ShowDialog();
                }
            }
            else
                _formThread.Start();
        }

        /// <summary>
        /// 显示遮罩层，在线程池线程执行工作项委托，并在执行完毕后自动关闭遮罩层，
        /// 如果在构造此<see cref="LoadingLayer"/>时没有指定newUiThread参数为true，
        /// 此方法将阻塞当前调用线程，直到<paramref name="waitCallback"/>执行完毕
        /// </summary>
        /// <param name="waitCallback">表示遮罩层显示时，线程池线程需要调用的委托</param>
        /// <param name="state">线程池线程需要使用的状态对象</param>
        public void ShowAutoClose(WaitCallback waitCallback, object state)
        {
            waitCallback += o => Close();
            ThreadPool.UnsafeQueueUserWorkItem(waitCallback, state);
            Show();
        }

        /// <summary>
        /// 显示遮罩层，在线程池线程执行工作项委托，并在执行完毕后自动关闭遮罩层，
        /// 如果在构造此<see cref="LoadingLayer"/>时没有指定newUiThread参数为true，
        /// 此方法将阻塞当前调用线程，直到<paramref name="waitCallback"/>执行完毕
        /// </summary>
        /// <param name="waitCallback">表示遮罩层显示时，线程池线程需要调用的委托</param>
        public void ShowAutoClose(WaitCallback waitCallback)
        {
            ShowAutoClose(waitCallback, null);
        }

        /// <summary>
        /// 关闭遮罩层
        /// </summary>
        public void Close()
        {
            if (_form.IsHandleCreated)
                _form.BeginInvoke(new MethodInvoker(_form.Close));
            else
                _isClosed = true;

            Dispose();
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
                else if (_circle.IsHandleCreated)
                    _circle.Dispose();
            }

            if (_progressBar != null)
            {
                if (_progressBar.InvokeRequired)
                    _progressBar.BeginInvoke(new MethodInvoker(_progressBar.Dispose));
                else if (_progressBar.IsHandleCreated)
                    _progressBar.Dispose();
            }

            if (_form != null)
            {
                if (_form.InvokeRequired)
                    _form.BeginInvoke(new MethodInvoker(_form.Dispose));
                else if (_form.IsHandleCreated)
                    _form.Dispose();
            }
        }
    }
}
