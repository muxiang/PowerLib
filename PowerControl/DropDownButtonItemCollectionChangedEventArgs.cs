using System;

namespace PowerControl
{
    /// <summary>
    /// 下拉按钮下拉集合变更事件参数
    /// </summary>
    public class DropDownButtonItemCollectionChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 构造下拉按钮下拉集合变更事件参数的实例
        /// </summary>
        /// <param name="item"></param>
        public DropDownButtonItemCollectionChangedEventArgs(DropDownButtonItem item)
        {
            ChangedItem = item;
        }

        /// <summary>
        /// 变更项
        /// </summary>
        public DropDownButtonItem ChangedItem { get; set; }
    }
}
