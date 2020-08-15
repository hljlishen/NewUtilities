using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Utilities.RadarWorks
{
    public abstract class Sensor : IDisposable
    {
        public Displayer Displayer => ParentElement?.ParentDispalyer;
        public List<LiveObject> objects => ParentElement.Objects;
        public Control Panel => Displayer.Panel;

        public event Action<Sensor> ObjectStateChanged;
        public GraphicElement ParentElement { get; set; } = null;
        public void InvokeObjectStateChanged() => ObjectStateChanged?.Invoke(this);

        //protected object locker = null;
        //public void SetLocker(object locker) => this.locker = locker ?? throw new NullReferenceException("locker为空");
        protected object locker => ParentElement.Locker;
        private void SetDisplayer(Displayer d)
        {
            //Displayer = d;
            d.BeforeRebindTarget += Displayer_BeforeRebindTarget;
            d.AfterRebindTarget += Displayer_AfterRebindTarget;
            BindEvents(d.Panel);
        }

        public virtual void SetParentElement(GraphicElement e)
        {
            ParentElement = e;
            SetDisplayer(e.ParentDispalyer);
        }
        private void Displayer_AfterRebindTarget(Control panel) => BindEvents(panel);
        protected abstract void BindEvents(Control panel);
        private void Displayer_BeforeRebindTarget(Control panel) => UnbindEvents(panel);
        protected abstract void UnbindEvents(Control panel);
        //public void SetObjects(List<LiveObject> objects) => this.objects = objects;
        public virtual void Dispose() => UnbindEvents(Panel);
    }
}
