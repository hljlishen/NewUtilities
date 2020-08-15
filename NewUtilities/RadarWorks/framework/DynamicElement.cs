using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace Utilities.RadarWorks
{
    public abstract class DynamicElement<T> : GraphicElement
    {
        public T Model = default;
        protected override void DrawElement(RenderTarget rt)
        {
            lock (Locker)
            {
                if (Model == null)
                    return;
                DrawDynamicElement(rt);
            }
        }

        protected abstract void DrawDynamicElement(RenderTarget rt);

        public virtual void Update(T t)
        {
            lock (Locker)
            {
                DoUpdate(t);
                RefreshObjects();
                Redraw();
            }
        }

        protected virtual void DoUpdate(T t)
        {
            Model = t;
        }
    }
}
