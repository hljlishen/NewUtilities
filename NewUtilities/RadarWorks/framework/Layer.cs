using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;

namespace Utilities.RadarWorks
{
    public class Layer : GraphicElement
    {
        public List<IGraphic> elements = new List<IGraphic>();
        protected BitmapRenderTarget bitmapRt;
        private bool isLocked = false;
        private object owner = null;

        public void Lock(object ob)
        {
            lock (Locker)
            {
                if (isLocked)
                    throw new System.Exception("该Layer已经被锁定，不能执行Lock操作");
                isLocked = true;
                owner = ob;
            }
        }

        public void Unlock(object ob)
        {
            lock (Locker)
            {
                if (!isLocked)
                    return;
                if (ReferenceEquals(owner, ob))
                {
                    isLocked = false;
                    owner = null;
                }
                else
                    throw new System.Exception("请求Unlock的对象和Lock对象不是同一实例");
            }
        }
        public Layer(int id)
        {
            LayerId = id;
        }

        private void DrawLayerToTarget(RenderTarget rt)
        {
            rt.DrawBitmap(bitmapRt.Bitmap, 1, BitmapInterpolationMode.Linear, new RectF(0, 0, Panel.Width, Panel.Height));
        }

        private void DrawLayerOnBitmap()
        {
            bitmapRt.BeginDraw();
            bitmapRt.Clear();

            lock (Locker)
            {
                foreach (var e in elements)
                {
                    e.Draw(bitmapRt);
                }
            }
            bitmapRt.EndDraw();
        }

        public void DrawIfChanged(RenderTarget rt)
        {
            if (HasChanged())
                Draw(rt);
            else
                DrawLayerToTarget(rt);
        }

        public override bool HasChanged() => base.HasChanged() ? true : ElementsChanged();

        private bool ElementsChanged()
        {
            if (elements.Count == 0)
                return false;
            lock (Locker)
            {
                foreach (var e in elements)
                {
                    if (e.HasChanged())
                        return true;
                }
            }

            return false;
        }

        public void Add(IGraphic e)
        {
            lock (Locker)
            {
                if (isLocked)
                    throw new System.Exception($"图层{LayerId}已经被锁定，不能添加新元素");
                elements.Add(e);
            }

            e.LayerId = LayerId;
            e.SetDisplayer(ParentDisplayer);
            Redraw();
        }

        public void AddRange(IEnumerable<IGraphic> es)
        {
            lock (Locker)
            {
                foreach (var e in es)
                {
                    elements.Add(e);
                    e.LayerId = LayerId;
                    e.SetDisplayer(ParentDisplayer);
                }
            }
            Redraw();   //2020-8-11将updateView移出Lock块
        }

        /// <summary>
        /// 将图层中的所有元素替换为参数中的新元素
        /// </summary>
        /// <param name="es">要显示在图层中的元素</param>
        public void RefreshLayerElements(IEnumerable<IGraphic> es)
        {
            lock (Locker)
            {
                //清空当前图层的所有元素
                foreach (var e in elements)
                {
                    e.Dispose();
                }
                elements?.Clear();

                //给图层添加新元素
                foreach (var e in es)
                {
                    elements.Add(e);
                    e.LayerId = LayerId;
                    e.SetDisplayer(ParentDisplayer);
                }
            }
            Redraw();//2020-8-11将updateView移出Lock块
        }

        public void RemoveElement(IGraphic e)
        {
            lock (Locker)
            {
                if (elements.Contains(e))
                {
                    elements.Remove(e);
                    Redraw();
                }
            }
        }

        public void Clear()
        {
            lock (Locker)
            {
                elements.Clear();
            }
        }

        public override void Dispose()
        {
            foreach (var e in elements)
            {
                e.Dispose();
            }
            elements.Clear();
            bitmapRt?.Dispose();
        }

        protected override void DrawElement(RenderTarget rt)
        {
            if (bitmapRt == null || bitmapRt.Size != rt.Size)
            {
                bitmapRt?.Dispose();
                bitmapRt = rt.CreateCompatibleRenderTarget(new CompatibleRenderTargetOptions(), rt.Size);
            }
            DrawLayerOnBitmap();
            DrawLayerToTarget(rt);
            return;
        }
        //protected override IEnumerable<LiveObject> GetObjects() => null;
    }
}
