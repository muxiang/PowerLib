using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Security.Permissions;

namespace PowerControl.Design
{
    /// <summary>
    /// 用于支持属性列表中的事件页
    /// </summary>
    [PermissionSet(SecurityAction.InheritanceDemand, Name = "FullTrust")]
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    public class TypeEventsTab : System.Windows.Forms.Design.EventsTab
    {
        [Browsable(true)]
        private IServiceProvider sp;

        /// <summary>
        /// 构造事件页
        /// </summary>
        /// <param name="sp"></param>
        public TypeEventsTab(IServiceProvider sp) : base(sp)
        {
            this.sp = sp;
        }

        /// <summary>
        /// 重写返回事件集合
        /// </summary>
        /// <param name="context"></param>
        /// <param name="component"></param>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object component, Attribute[] attributes)
        {
            //  Obtain an instance of the IEventBindingService.
            IEventBindingService eventPropertySvc = (IEventBindingService)
                sp.GetService(typeof(IEventBindingService));

            //  Return if an IEventBindingService could not be obtained.
            if (eventPropertySvc == null)
                return new PropertyDescriptorCollection(null);

            //  Obtain the events on the component.
            EventDescriptorCollection events =
                TypeDescriptor.GetEvents(component, attributes);

            //  Create an array of the events, where each event is assigned 
            //  a category matching its type.
            EventDescriptor[] newEvents = new EventDescriptor[events.Count];
            for (int i = 0; i < events.Count; i++)
                newEvents[i] = TypeDescriptor.CreateEvent(events[i].ComponentType, events[i],
                    new CategoryAttribute(events[i].EventType.FullName));
            events = new EventDescriptorCollection(newEvents);

            //  Return event properties for the event descriptors.
            return eventPropertySvc.GetEventProperties(events);
        }

        /// <summary>
        /// 事件页名称
        /// </summary>
        public override string TabName => "事件";

        /// <summary>
        /// 事件页图标
        /// </summary>
        public override Bitmap Bitmap => Properties.Resources.iconEvent;
    }
}