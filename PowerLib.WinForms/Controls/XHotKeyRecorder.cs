using System;
using System.Text;
using System.Windows.Forms;

using static PowerLib.NativeCodes.NativeConstants;
using static PowerLib.Utilities.CommonUtility;

namespace PowerLib.WinForms.Controls;

/// <summary>
/// 表示基于<see cref="XTextBox"/>的热键记录器
/// </summary>
public class XHotKeyRecorder : XTextBox
{
    private bool _isKeyHolding;
    private Tuple<KeyModifiers, Keys> _hotKey;

    /// <summary>
    /// 初始化<see cref="XHotKeyRecorder"/>的实例
    /// </summary>
    public XHotKeyRecorder()
    {
        ReadOnly = true;
        ShowHotKeys();
    }

    /// <summary>
    /// 获取或设置热键
    /// </summary>
    public Tuple<KeyModifiers, Keys> HotKey
    {
        get => _hotKey;
        set
        {
            _hotKey = value;
            ShowHotKeys();
        }
    }

    /// <summary>
    /// 获取热键的字符串表示
    /// </summary>
    public string HotKeyString
    {
        get
        {
            if (_hotKey == null)
                return NoneString;

            StringBuilder sbHotKey = new();

            KeyModifiers km = _hotKey.Item1;
            Keys keyCode = _hotKey.Item2;

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
    private void ShowHotKeys()
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

        _hotKey = new Tuple<KeyModifiers, Keys>(km, e.KeyCode);
        _isKeyHolding = e.KeyCode != Keys.Menu && e.KeyCode != Keys.ShiftKey && e.KeyCode != Keys.ControlKey;

        Focus();
        ShowHotKeys();
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

        _hotKey = new Tuple<KeyModifiers, Keys>(_hotKey.Item1 & ~kmUp, _hotKey.Item2);

        Focus();
        ShowHotKeys();
    }
}