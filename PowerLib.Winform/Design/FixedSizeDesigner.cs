using System.Windows.Forms.Design;

namespace PowerLib.Winform.Design
{
    /// <summary>
    /// 固定大小设计器
    /// </summary>
    /// <inheritdoc />
    public class FixedSizeDesigner : ControlDesigner
    {
        ///<inheritdoc cref="ControlDesigner"/>
        public override SelectionRules SelectionRules => SelectionRules.Visible
                                                         | SelectionRules.Moveable;
    }
}
