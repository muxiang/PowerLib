using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace PowerLib.Winform.Controls
{
    internal class TypeDescriptorContext : ITypeDescriptorContext
    {
        private readonly Control _editingObject;
        private readonly PropertyDescriptor _editingProperty;

        public TypeDescriptorContext(Control obj, PropertyDescriptor property)
        {
            _editingObject = obj;
            _editingProperty = property;
        }
        public IContainer Container => _editingObject.Container;

        public object Instance => _editingObject;

        public void OnComponentChanged()
        {
        }
        public bool OnComponentChanging()
        {
            return true;
        }
        public PropertyDescriptor PropertyDescriptor => _editingProperty;

        public object GetService(Type serviceType)
        {
            return _editingObject.Site.GetService(serviceType);
        }
    }
}
