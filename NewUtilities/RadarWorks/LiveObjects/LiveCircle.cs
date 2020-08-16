using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Utilities.Tools;

namespace Utilities.RadarWorks
{
    public class LiveCircle : LiveObject
    {
        public LiveCircle(Ellipse ellipse)
        {
            Ellipse = ellipse;
        }
        public Ellipse Ellipse { get; set; }
        public PointF TextLeftTop { get; set; }

        public double DistanceToCirleEdge(PointF p)
        {
            var disToCenter = Functions.DistanceBetween(p.ToPoint2F(), Ellipse.Point);
            return Math.Abs(Ellipse.RadiusX - disToCenter);
        }

        public override bool IsPointNear(PointF mouseLocation) => DistanceToCirleEdge(mouseLocation) < 6;

        public override void DrawFrame(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush frameBrush, float strokeWidth, StrokeStyle style)
        {
            rt.DrawEllipse(Ellipse, frameBrush, strokeWidth, style);
        }

        public override void Fill(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush fillBrush)
        {
            rt.FillEllipse(Ellipse, fillBrush);
        }

        public override void DrawFrame(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush frameBrush, float strokeWidth)
        {
            rt.DrawEllipse(Ellipse, frameBrush, strokeWidth);
        }
    }
}
