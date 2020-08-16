using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using Utilities.RadarWorks;

namespace NewUtilities.RadarWorks.Elements.Markers
{
    public class MultiMarker<T> : GraphicElement where T : IMarkerInterface, new()
    {
        private int markerCount;
        private List<T> markers = new List<T>();
        public MarkerIteratorType IteratorType { get; set; } = MarkerIteratorType.NoLeftOrRight;

        public MultiMarker(int markerCount)
        {
            this.markerCount = markerCount;
            for (int i = 0; i < markerCount; i++)
            {
                markers.Add(new T() { Sensor = new MouseMoveSensor() });
            }
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            foreach (var m in markers)  //向Displayer添加MarkerElement
            {
                d.Elements.Add(LayerId, m);
            }
        }

        protected override void DrawElement(RenderTarget rt)
        {
            var iterator = MarkerModelIterator.GetIterator(IteratorType, markers[0].MinValue, markers[0].MaxValue, markerCount);

            foreach (var m in markers)
            {
                m.Update(iterator.Next());
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            lock(Locker)
            {
                foreach (var m in markers)
                {
                    m.Dispose();
                }
            }
        }
    }
}
