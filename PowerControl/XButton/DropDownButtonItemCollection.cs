using System;
using System.Collections.Generic;

namespace PowerControl
{
    /// <summary>
    /// 支持下拉按钮下拉项集合
    /// </summary>
    public class DropDownButtonItemCollection : List<DropDownButtonItem>
    {
        /// <summary>
        /// 集合项添加后发生
        /// </summary>
        public event EventHandler<DropDownButtonItemCollectionChangedEventArgs> AfterItemAdded;

        /// <summary>
        /// 集合项移除后发生
        /// </summary>
        public event EventHandler<DropDownButtonItemCollectionChangedEventArgs> AfterItemRemoved;

        /// <summary>
        /// 向集合添加指定项
        /// </summary>
        /// <param name="item">待添加项</param>
        public new void Add(DropDownButtonItem item)
        {
            base.Add(item);
            AfterItemAdded?.Invoke(this, new DropDownButtonItemCollectionChangedEventArgs(item));
        }

        /// <summary>
        /// 从集合移除指定项
        /// </summary>
        /// <param name="item">待移除项</param>
        public new void Remove(DropDownButtonItem item)
        {
            base.Remove(item);
            AfterItemRemoved?.Invoke(this, new DropDownButtonItemCollectionChangedEventArgs(item));
        }
    }
}
