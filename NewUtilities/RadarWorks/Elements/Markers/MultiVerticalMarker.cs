using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using Utilities.RadarWorks;

namespace NewUtilities.RadarWorks.Elements.Markers
{
    public class MultiVerticalMarker<T> : GraphicElement where T : MarkerElement, new()
    {
        private int markerCount;
        private List<T> markers = new List<T>();

        public MultiVerticalMarker(int markerCount)
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
            if (markerCount == 0)
                return;
            var min = markers[0].MinValue;
            var max = markers[0].MaxValue;
            var step = (max - min) / (markerCount + 1);    //此处markerCount+1，表示不显示信号视图范围最左边和最右边的两条线

            for (int i = 1; i < markerCount +1; i++)
            {
                double value = min + step * i;
                markers[i - 1].Update(value);
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
