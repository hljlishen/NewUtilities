﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.RadarWorks
{
    public class LayerManager : GraphicElement
    {
        protected Dictionary<int, Layer> layers = new Dictionary<int, Layer>();
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

        public void Add(int layerId, IGraphic e)
        {
            lock(Locker)
            {
                if (!layers.ContainsKey(layerId))
                {
                    Layer layer = new Layer(layerId);
                    layer.SetDisplayer(ParentDispalyer);
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
                layer.SetDisplayer(ParentDispalyer);
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

        protected override IEnumerable<LiveObject> GetObjects() => null;

        public void Remove(int layerId, IGraphic e) => layers[layerId].RemoveElement(e);
    }
}
