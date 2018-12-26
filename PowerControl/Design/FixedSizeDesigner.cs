using System.Windows.Forms.Design;

namespace PowerControl.Design
{
    /// <summary>
    /// 固定大小设计器
    /// </summary>
    public class FixedSizeDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules => SelectionRules.Visible
                                                         | SelectionRules.Moveable;
    }
}
