using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using Utilities.RadarWorks;

namespace NewUtilities.RadarWorks.Elements.Markers
{
    public class MultiMarker<T> : GraphicElement where T : IMarkerInterface, new()
    {
        private int markerCount;
        private List<T> markers = new List<T>();

        public float TextSize { get; private set; }
        public string TextFont { get; private set; }

        public string Unit
        {
            get
            {
                if (markers == null || markers.Count == 0)
                    return "";
                else
                    return markers[0].Unit;
            }
            set
            {
                lock (Locker)
                {
                    foreach (var m in markers)  //向Displayer添加MarkerElement
                    {
                        m.Unit = value;
                    }
                }
            }
        }

        public string ValueFormat
        {
            get
            {
                if (markers == null || markers.Count == 0)
                    return "";
                else
                    return markers[0].ValueFormat;
            }
            set
            {
                lock (Locker)
                {
                    foreach (var m in markers)  //向Displayer添加MarkerElement
                    {
                        m.ValueFormat = value;
                    }
                }
            }
        }

        public MarkerIteratorType IteratorType { get; set; } = MarkerIteratorType.NoLeftOrRight;

        public MultiMarker(int markerCount, float textSize = 12, string textFont = "Consolas")
        {
            this.markerCount = markerCount;
            TextSize = textSize;
            TextFont = textFont;
            for (int i = 0; i < markerCount; i++)
            {
                markers.Add(new T() { Sensor = new MouseMoveSensor(), TextFont = TextFont, TextSize = TextSize });
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
