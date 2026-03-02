using System;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;

using static PowerLib.Utilities.CommonUtility;

namespace PowerLib.WPF.Controls;

/// <summary>
/// 表示一个热键记录器
/// </summary>
public class HotKeyRecorder : TextBox
{
    private Tuple<ModifierKeys, Key> _hotKey;
        
    /// <summary>
    /// 初始化 <see cref="HotKeyRecorder"/> 的实例
    /// </summary>
    public HotKeyRecorder()
    {
        IsReadOnly = true;
        IsUndoEnabled = false; // 禁用 Ctrl+Z 撤销栈，防止干扰
        MinWidth = 100;

        // 初始化显示
        ShowHotKeys();
    }

    /// <summary>
    /// 获取或设置热键
    /// Item1: 修饰键 (Ctrl, Alt, Shift, Windows)
    /// Item2: 主键
    /// </summary>
    public Tuple<ModifierKeys, Key> HotKey
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

            StringBuilder sbHotKey = new StringBuilder();

            ModifierKeys modifiers = _hotKey.Item1;
            Key key = _hotKey.Item2;

            // 拼接修饰键字符串
            if (modifiers.HasFlag(ModifierKeys.Control)) sbHotKey.Append("Ctrl + ");
            if (modifiers.HasFlag(ModifierKeys.Alt)) sbHotKey.Append("Alt + ");
            if (modifiers.HasFlag(ModifierKeys.Shift)) sbHotKey.Append("Shift + ");
            if (modifiers.HasFlag(ModifierKeys.Windows)) sbHotKey.Append("Win + ");

            // 判断是否按下了非修饰键的主键
            // 注意：WPF 中 System 代表 Alt 键被按下时的特殊状态
            bool isModifierKey =
                key == Key.LeftCtrl || key == Key.RightCtrl ||
                key == Key.LeftAlt || key == Key.RightAlt ||
                key == Key.LeftShift || key == Key.RightShift ||
                key == Key.LWin || key == Key.RWin ||
                key == Key.System; // 处理 Alt

            if (!isModifierKey && key != Key.None)
                sbHotKey.Append(key.ToString());

            return sbHotKey.ToString();
        }
    }

    /// <summary>
    /// 更新显示文本
    /// </summary>
    private void ShowHotKeys()
    {
        Text = HotKeyString;
        // 将光标移动到末尾，防止长文本显示不全
        CaretIndex = Text.Length;
    }

    /// <inheritdoc />
    protected override void OnPreviewKeyDown(KeyEventArgs e)
    {
        // 屏蔽输入法处理，标记事件已处理防止 TextBox 输入字符
        e.Handled = true;
        base.OnPreviewKeyDown(e);

        // 获取当前按下的所有修饰键
        ModifierKeys modifiers = Keyboard.Modifiers;

        // 处理 WPF 的 Alt 键怪癖：
        // 当按住 Alt 时，e.Key 会返回 Key.System，此时真实键值在 e.SystemKey 中
        Key realKey = (e.Key == Key.System) ? e.SystemKey : e.Key;

        // 如果只按下了修饰键（如只按了 Ctrl），我们需要更新显示为 "Ctrl + "
        // 但在逻辑上我们记录此时的 Key 为修饰键本身
        _hotKey = new Tuple<ModifierKeys, Key>(modifiers, realKey);

        ShowHotKeys();
    }

    /// <inheritdoc />
    protected override void OnPreviewKeyUp(KeyEventArgs e)
    {
        e.Handled = true;
        base.OnPreviewKeyUp(e);

        // 获取刚刚松开的键
        Key realKey = (e.Key == Key.System) ? e.SystemKey : e.Key;

        // 判断松开的是否是修饰键
        bool isModifierKey =
            realKey == Key.LeftCtrl || realKey == Key.RightCtrl ||
            realKey == Key.LeftAlt || realKey == Key.RightAlt ||
            realKey == Key.LeftShift || realKey == Key.RightShift ||
            realKey == Key.LWin || realKey == Key.RWin ||
            realKey == Key.System;

        // 如果松开的不是修饰键（即松开了主键，如 'A'），我们通常保持显示不变（保留 Ctrl+A）
        // 只有当用户松开修饰键且没有主键时，我们才清除或更新状态
        if (!isModifierKey) return;

        // 重新计算剩余的修饰键
        // 注意：Keyboard.Modifiers 在 KeyUp 时可能已经移除了刚刚松开的键，
        // 但为了保险，我们直接读取当前的键盘状态
        ModifierKeys currentModifiers = Keyboard.Modifiers;

        // 如果所有键都松开了，保持最后的记录状态（通常用户希望看到他们刚刚按下的键）
        // 只有当还在进行组合键操作的中间状态松开修饰键时，才需要特殊逻辑
        // 这里保留原版逻辑的精髓：如果只是松开修饰键，则更新状态（比如按着 Ctrl 显示 Ctrl +，松开变 None）

        // 检查当前是否还有主键被按住（简化逻辑：如果全部松开，就不重置为 None，保留最后状态用户体验更好）
        // 但为了还原你代码中的逻辑：

        // 如果当前没有热键记录，或者已经记录了完整热键（含主键），则不处理 KeyUp
        // 只有在“仅按住修饰键”的状态下松开，才需要刷新显示

        if (_hotKey != null)
        {
            Key currentStoredKey = _hotKey.Item2;
            bool storedKeyIsModifier =
                currentStoredKey == Key.LeftCtrl || currentStoredKey == Key.RightCtrl ||
                currentStoredKey == Key.LeftAlt || currentStoredKey == Key.RightAlt ||
                currentStoredKey == Key.LeftShift || currentStoredKey == Key.RightShift ||
                currentStoredKey == Key.LWin || currentStoredKey == Key.RWin ||
                currentStoredKey == Key.System;

            // 如果之前记录的状态只是修饰键（例如显示 "Ctrl +"），现在松开了 Ctrl，则应该清空
            if (storedKeyIsModifier && currentModifiers == ModifierKeys.None)
            {
                _hotKey = null;
                ShowHotKeys();
            }
            else if (storedKeyIsModifier)
            {
                // 如果还有其他修饰键按着（例如按 Ctrl+Alt，松开 Ctrl，剩 Alt）
                _hotKey = new Tuple<ModifierKeys, Key>(currentModifiers,
                    (currentModifiers.HasFlag(ModifierKeys.Alt) ? Key.LeftAlt :
                        currentModifiers.HasFlag(ModifierKeys.Control) ? Key.LeftCtrl : Key.LeftShift));
                // 这里简单指定一个 Key 占位，实际显示由 HotKeyString 逻辑决定
                ShowHotKeys();
            }
        }
    }
}