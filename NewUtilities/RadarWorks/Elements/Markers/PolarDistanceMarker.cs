using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.RadarWorks;

namespace NewUtilities.RadarWorks.Elements.Markers
{
    public class PolarDistanceMarker : MarkerElement
    {
        public override double MinValue => 0;

        public override double MaxValue => ReferenceSystem.Right;

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if(!Selected)
            {
                Objects[0].DrawFrame(rt, normalBrush, 2);
            }
            else
            {
                Objects[0].DrawFrame(rt, selectBrush, 4);
                if (sensor == null)
                    return;
                DrawSelectText(rt);
            }
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            var center = ReferenceSystem.ScreenOriginalPoint;
            var radius = Math.Abs(Mapper.GetScreenY(Model) - center.Y);
            yield return new LiveCircle(new Ellipse(center.ToPoint2F(), (float)radius, (float)radius));
        }
    }
}
