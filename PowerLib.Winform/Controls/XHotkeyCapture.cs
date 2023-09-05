using System;
using System.Text;
using System.Windows.Forms;

using static PowerLib.NativeCodes.NativeConstants;
using static PowerLib.Utilities.CommonUtility;

namespace PowerLib.Winform.Controls
{
    /// <summary>
    /// 表示基于<see cref="XTextBox"/>的热键捕捉器
    /// </summary>
    public class XHotkeyCapture : XTextBox
    {
        private bool _isKeyHolding;
        private Tuple<KeyModifiers, Keys> _hotkey;

        /// <summary>
        /// 初始化<see cref="XHotkeyCapture"/>的实例
        /// </summary>
        public XHotkeyCapture()
        {
            ReadOnly = true;
            ShowHotkeys();
        }

        /// <summary>
        /// 获取或设置热键
        /// </summary>
        public Tuple<KeyModifiers, Keys> Hotkey
        {
            get => _hotkey;
            set
            {
                _hotkey = value;
                ShowHotkeys();
            }
        }

        /// <summary>
        /// 获取热键的字符串表示
        /// </summary>
        public string HotKeyString
        {
            get
            {
                if (_hotkey == null)
                    return NoneString;

                StringBuilder sbHotKey = new();

                KeyModifiers km = _hotkey.Item1;
                Keys keyCode = _hotkey.Item2;

                sbHotKey.Append(km.HasFlag(KeyModifiers.Control) ? "Ctrl + " : string.Empty);
                sbHotKey.Append(km.HasFlag(KeyModifiers.Alt) ? "Alt + " : string.Empty);
                sbHotKey.Append(km.HasFlag(KeyModifiers.Shift) ? "Shift + " : string.Empty);

                _isKeyHolding = keyCode != Keys.Menu && keyCode != Keys.ShiftKey && keyCode != Keys.ControlKey;

                if (_isKeyHolding)
                    sbHotKey.Append(keyCode.ToString());

                return sbHotKey.ToString();
            }
        }

        /// <summary>
        /// 显示热键
        /// </summary>
        private void ShowHotkeys()
        {
            Text = HotKeyString;
        }

        /// <inheritdoc />
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            KeyModifiers km =
                (e.Alt ? KeyModifiers.Alt : 0) |
                (e.Control ? KeyModifiers.Control : 0) |
                (e.Shift ? KeyModifiers.Shift : 0);

            _hotkey = new Tuple<KeyModifiers, Keys>(km, e.KeyCode);
            _isKeyHolding = e.KeyCode != Keys.Menu && e.KeyCode != Keys.ShiftKey && e.KeyCode != Keys.ControlKey;

            Focus();
            ShowHotkeys();
        }

        /// <inheritdoc />
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (_isKeyHolding) return;

            KeyModifiers kmUp =
                (e.Alt || e.KeyCode.HasFlag(Keys.Menu) ? KeyModifiers.Alt : 0) |
                (e.Control || e.KeyCode.HasFlag(Keys.ControlKey) ? KeyModifiers.Control : 0) |
                (e.Shift || e.KeyCode.HasFlag(Keys.ShiftKey) ? KeyModifiers.Shift : 0);

            _hotkey = new Tuple<KeyModifiers, Keys>(_hotkey.Item1 & ~kmUp, _hotkey.Item2);
            //Global.Hotkeys.Item2 &= ~e.KeyCode;

            Focus();
            ShowHotkeys();
        }
    }
}
