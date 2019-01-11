using System.Windows.Forms.Design;

namespace PowerControl.Design
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
