using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Utilities.RadarWorks
{
    /// <summary>
    /// Sensor对象负责改变ParentElement的Selected属性
    /// </summary>
    public abstract class Sensor : IDisposable
    {
        public Displayer Displayer => ParentElement?.ParentDispalyer;
        protected List<LiveObject> objects => ParentElement.Objects;
        public Control Panel => Displayer.Panel;

        public event Action<Sensor> ObjectStateChanged;
        public GraphicElement ParentElement { get; set; } = null;
        public void InvokeObjectStateChanged() => ObjectStateChanged?.Invoke(this);

        protected object locker => ParentElement.Locker;
        private void SetDisplayer(Displayer d)
        {
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
        public virtual void Dispose() => UnbindEvents(Panel);
    }
}
