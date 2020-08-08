using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;

namespace Utilities.RadarWorks.Elements.Signal
{
    public class XAxisMeasure : GraphicElement, ISwtichable
    {
        private MovableXAxisMarker marker1 = new MovableXAxisMarker();
        private MovableXAxisMarker marker2 = new MovableXAxisMarker() { Color = Color.DeepPink };
        public bool IsOn => marker1.IsOn;

        public string Name { get; set; } = "X轴测量";

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            d.Elements.Add(LayerId, marker1);
            d.Elements.Add(LayerId, marker2);
            marker1.Model = Mapper.CoordinateLeft + (Mapper.CoordinateRight - Mapper.CoordinateLeft) / 3;
            marker2.Model = Mapper.CoordinateLeft + (Mapper.CoordinateRight - Mapper.CoordinateLeft) * 2 / 3;
        }

        public void Off()
        {
            marker1.Off();
            marker2.Off();
            UpdateView();
        }

        public void On()
        {
            marker1.On();
            marker2.On();
            UpdateView();
        }

        protected override void DrawElement(RenderTarget rt)
        {
            marker1.UpdateView();
            marker2.UpdateView();
        }
    }
}
