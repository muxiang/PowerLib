using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace PowerLib.WPF.Controls;

/// <summary>
/// 封装一个支持IPV4格式化录入的输入框
/// </summary>
[TemplatePart(Name = "PART_Box1", Type = typeof(TextBox))]
[TemplatePart(Name = "PART_Box2", Type = typeof(TextBox))]
[TemplatePart(Name = "PART_Box3", Type = typeof(TextBox))]
[TemplatePart(Name = "PART_Box4", Type = typeof(TextBox))]
public class IPV4InputBox : Control, INotifyDataErrorInfo
{
    private TextBox[] _arrBoxes;
    private bool _isInternalUpdating;
    private readonly Dictionary<string, List<string>> _errors = new();

    static IPV4InputBox()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(IPV4InputBox), new FrameworkPropertyMetadata(typeof(IPV4InputBox)));
    }

    #region 依赖属性

    public static readonly DependencyProperty AddressProperty =
        DependencyProperty.Register(nameof(Address), typeof(IPAddress), typeof(IPV4InputBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAddressChanged));

    public static readonly DependencyProperty AddressStringProperty =
        DependencyProperty.Register(nameof(AddressString), typeof(string), typeof(IPV4InputBox),
            new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnAddressStringChanged));

    #endregion 依赖属性

    #region 属性

    public IPAddress Address
    {
        get => (IPAddress)GetValue(AddressProperty);
        set => SetValue(AddressProperty, value);
    }

    public string AddressString
    {
        get => (string)GetValue(AddressStringProperty);
        set => SetValue(AddressStringProperty, value);
    }

    #endregion 属性

    #region 依赖属性变更

    private static void OnAddressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        IPV4InputBox ctrl = (IPV4InputBox)d;

        if (ctrl._isInternalUpdating)
            return;

        ctrl._isInternalUpdating = true;
        IPAddress newIp = e.NewValue as IPAddress;
        ctrl.AddressString = newIp?.ToString();
        ctrl.UpdateTextBoxes(newIp?.GetAddressBytes());
        ctrl._isInternalUpdating = false;

        ctrl.Validate();
    }

    private static void OnAddressStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        IPV4InputBox ctrl = (IPV4InputBox)d;
        if (ctrl._isInternalUpdating) return;

        ctrl._isInternalUpdating = true;
        string strNew = e.NewValue as string;

        if (IPAddress.TryParse(strNew, out IPAddress ip))
        {
            ctrl.Address = ip;
            ctrl.UpdateTextBoxes(ip.GetAddressBytes());
        }
        else
        {
            ctrl.Address = null;
            ctrl.UpdateTextBoxes(null);
        }

        ctrl._isInternalUpdating = false;
        ctrl.Validate();
    }

    #endregion 依赖属性变更

    #region 重写FrameworkElement

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        _arrBoxes = new[] { "PART_Box1", "PART_Box2", "PART_Box3", "PART_Box4" }.Select(n => GetTemplateChild(n) as TextBox).ToArray();

        foreach (TextBox box in _arrBoxes)
        {
            if (box == null)
                continue;

            box.PreviewTextInput += (_, e) => e.Handled = !char.IsDigit(e.Text, 0);
            box.TextChanged += (s, _) => Box_TextChanged(s as TextBox);
            box.PreviewKeyDown += Box_PreviewKeyDown;
            DataObject.AddPastingHandler(box, Box_Pasting);
        }

        if (Address != null) UpdateTextBoxes(Address.GetAddressBytes());
    }

    #endregion 重写FrameworkElement

    #region 事件处理

    private void Box_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        if (sender is not TextBox box)
            return;

        switch (e.Key)
        {
            // 跳转上一格
            case Key.Left when string.IsNullOrEmpty(box.Text) || box.SelectionStart == 0 && Array.IndexOf(_arrBoxes, box) > 0:
                FocusPreviousBox(box, false);
                e.Handled = true;
                break;
            case Key.Back when box.SelectionStart == 0 && box.SelectionLength == 0 && Array.IndexOf(_arrBoxes, box) > 0:
                TextBox txtPrevious = GetPreviousBox(box);

                if (txtPrevious != null)
                {
                    string strText = txtPrevious.Text;

                    if (strText.Length > 0)
                    {
                        strText = strText.Substring(0, strText.Length - 1);
                        txtPrevious.Text = strText;
                    }

                    txtPrevious.SelectionStart = txtPrevious.Text.Length;
                    txtPrevious.Focus();
                }

                e.Handled = true;

                break;
            // 跳转下一格
            case Key.OemPeriod when !string.IsNullOrEmpty(box.Text) && Array.IndexOf(_arrBoxes, box) < 3:
            case Key.Decimal when !string.IsNullOrEmpty(box.Text) && Array.IndexOf(_arrBoxes, box) < 3 && box.SelectionStart > 0:
                FocusNextBox(box, true);
                e.Handled = true;
                break;
            case Key.Right when (string.IsNullOrEmpty(box.Text) || box.SelectionStart == box.Text.Length && box.SelectionLength == 0)
                                && Array.IndexOf(_arrBoxes, box) < 3:
                FocusNextBox(box, false);
                e.Handled = true;
                break;
            case Key.Home:
                TextBox firstBox = _arrBoxes[0];
                firstBox.Focus();
                firstBox.SelectionStart = 0;
                break;
            case Key.End:
                TextBox lastBox = _arrBoxes[_arrBoxes.Length - 1];
                lastBox.Focus();
                lastBox.SelectionStart = lastBox.Text.Length;
                break;
        }
    }

    private void Box_TextChanged(TextBox box)
    {
        if (_isInternalUpdating || _arrBoxes == null)
            return;

        // 范围校验 (0-255)
        if (int.TryParse(box.Text, out int val) && val > 255)
        {
            box.Text = "255";
            box.SelectionStart = 3;
        }

        _isInternalUpdating = true;

        bool anyContent = _arrBoxes.Any(b => !string.IsNullOrWhiteSpace(b.Text));
        bool allValid = _arrBoxes.All(b => byte.TryParse(b.Text, out _));

        if (!anyContent)
        {
            Address = null;
            AddressString = null;
        }
        else if (allValid)
        {
            byte[] bytes = _arrBoxes.Select(b => byte.Parse(b.Text)).ToArray();
            Address = new IPAddress(bytes);
            AddressString = Address.ToString();
        }
        else
        {
            Address = null; // 输入不完整，底层对象置 null
            AddressString = string.Join(".", _arrBoxes.Select(b => string.IsNullOrEmpty(b.Text) ? "" : b.Text));
        }

        _isInternalUpdating = false;

        // 输满3位自动跳格
        if (box.Text.Length == 3 && box.IsFocused && Array.IndexOf(_arrBoxes, box) < 3)
            box.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));

        Validate();
    }

    private void Box_Pasting(object sender, DataObjectPastingEventArgs e)
    {
        if (e.DataObject.GetDataPresent(DataFormats.Text))
        {
            string text = (string)e.DataObject.GetData(DataFormats.Text);

            if (IPAddress.TryParse(text, out IPAddress ip))
            {
                Address = ip;

                TextBox txtLast = _arrBoxes[_arrBoxes.Length - 1];
                txtLast.Focus();
                txtLast.Select(txtLast.Text.Length, 0);
            }
        }

        e.CancelCommand();
    }

    #endregion 事件处理

    #region 私有方法

    private void UpdateTextBoxes(byte[] bytes)
    {
        if (_arrBoxes == null)
            return;

        for (int i = 0; i < 4; i++)
            _arrBoxes[i].Text = bytes != null ? bytes[i].ToString() : string.Empty;
    }

    private TextBox GetPreviousBox(TextBox current)
    {
        int index = Array.IndexOf(_arrBoxes, current);
        return index > 0 ? _arrBoxes[index - 1] : null;
    }

    private TextBox GetNextBox(TextBox current)
    {
        int index = Array.IndexOf(_arrBoxes, current);
        return index < _arrBoxes.Length - 1 ? _arrBoxes[index + 1] : null;
    }

    private static void FocusBox(TextBox box, bool selectAll, int selectionStart)
    {
        if (box == null)
            return;

        box.Focus();

        if (selectAll)
            box.SelectAll();
        else
            box.SelectionStart = selectionStart;
    }

    private void FocusPreviousBox(TextBox current, bool selectAll)
    {
        TextBox previous = GetPreviousBox(current);
        FocusBox(previous, selectAll, previous.Text.Length);
    }

    private void FocusNextBox(TextBox current, bool selectAll)
    {
        TextBox next = GetNextBox(current);
        FocusBox(next, selectAll, 0);
    }

    #endregion 私有方法

    #region 实现INotifyDataErrorInfo

    public bool HasErrors => _errors.Any();

    public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

    public System.Collections.IEnumerable GetErrors(string propertyName) => _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;

    private void Validate()
    {
        _errors.Clear();

        if (Address == null)
            _errors[nameof(Address)] = new List<string> { "请输入有效的IPv4地址" };

        ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(nameof(Address)));
        VisualStateManager.GoToState(this, HasErrors ? "Invalid" : "Valid", true);
    }

    #endregion 实现INotifyDataErrorInfo
}