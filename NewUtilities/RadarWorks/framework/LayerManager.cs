using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.RadarWorks
{
    public class LayerManager : GraphicElement
    {
        protected Dictionary<int, Layer> layers = new Dictionary<int, Layer>();

        /// <summary>
        /// 重绘改变过的图层
        /// </summary>
        /// <param name="rt"></param>
        public void DrawChangedLayers(RenderTarget rt)
        {
            lock (Locker)
            {
                var keys = (from k in layers.Keys select k).ToList();
                keys.Sort();

                foreach (var key in keys)
                {
                    layers[key].DrawIfChanged(rt);
                }
            }
        }

        /// <summary>
        /// 向添加一个元素
        /// </summary>
        /// <param name="layerId">希望将元素加入的图层的Id，如果当前不存在该Id的图层，则会自动创建</param>
        /// <param name="e">要添加的元素</param>
        public void Add(int layerId, IGraphic e)
        {
            lock(Locker)
            {
                if (!layers.ContainsKey(layerId))
                {
                    Layer layer = new Layer(layerId);
                    layer.SetDisplayer(ParentDisplayer);
                    layers[layerId] = layer;
                }
                layers[layerId].Add(e);
                Redraw();
            }
        }
        internal Layer GetLayer(int layerId) => layers[layerId];
        internal void AddLayer(int layerId)
        {
            lock(Locker)
            {
                if (layers.ContainsKey(layerId))
                    return;
                Layer layer = new Layer(layerId);
                layer.SetDisplayer(ParentDisplayer);
                layers.Add(layerId, layer);
            }
        }

        public override void Dispose()
        {
            lock (Locker)
            {
                foreach (var key in layers.Keys)
                {
                    layers[key].Dispose();
                }
            }
        }

        /// <summary>
        /// 重绘所有图层
        /// </summary>
        /// <param name="rt">渲染对象</param>
        protected override void DrawElement(RenderTarget rt)
        {
            lock (Locker)
            {
                var keys = (from k in layers.Keys select k).ToList();
                keys.Sort();

                foreach (var key in keys)
                {
                    layers[key].Draw(rt);
                }
            }
        }

        /// <summary>
        /// 删除一个元素
        /// </summary>
        /// <param name="layerId">元素所在的图层Id</param>
        /// <param name="e">待删除的元素引用</param>
        public void Remove(int layerId, IGraphic e) => layers[layerId].RemoveElement(e);
    }
}
