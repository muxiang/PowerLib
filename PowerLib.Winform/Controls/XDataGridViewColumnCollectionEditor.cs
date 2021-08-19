using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace PowerLib.Winform.Controls
{
    internal class XDataGridViewColumnCollectionEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(ITypeDescriptorContext context,
            IServiceProvider provider, object value)
        {
            var field = context.Instance.GetType().GetField("dataGridView1",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance);

            var dataGridView1 = (DataGridView)field.GetValue(context.Instance);
            dataGridView1.Site = ((Control)context.Instance).Site;
            var columnsProperty = TypeDescriptor.GetProperties(dataGridView1)["Columns"];
            var tdc = new TypeDescriptorContext(dataGridView1, columnsProperty);
            var editor = (UITypeEditor)columnsProperty.GetEditor(typeof(UITypeEditor));
            var result = editor.EditValue(tdc, provider, value);
            dataGridView1.Site = null;
            return result;
        }
    }
}
