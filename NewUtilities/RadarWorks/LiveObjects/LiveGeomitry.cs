using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;

namespace Utilities.RadarWorks
{
    public abstract class LiveGeometry : LiveObject
    {
        //public LiveGeometry(Geometry geometry)
        //{
        //    Geometry = Guards.Guard.NullCheckAssignment(geometry);
        //}

        public Geometry Geometry { get; protected set; } = null;
        public override bool IsPointNear(PointF mouseLocation)
        {
            if (Geometry == null)
                return false;
            return Geometry.FillContainsPoint(mouseLocation.ToPoint2F());
        }

        public override void DrawFrame(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush frameBrush, float strokeWidth, StrokeStyle style)
        {
            if (Geometry == null)
                Geometry = GetGeometry(rt);
            rt.DrawGeometry(Geometry, frameBrush, strokeWidth, style);
        }

        public override void DrawFrame(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush frameBrush, float strokeWidth)
        {
            if (Geometry == null)
                Geometry = GetGeometry(rt);
            rt.DrawGeometry(Geometry, frameBrush, strokeWidth);
        }

        public override void Fill(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush fillBrush)
        {
            if (Geometry == null)
                Geometry = GetGeometry(rt);
            rt.FillGeometry(Geometry, fillBrush);
        }

        protected abstract Geometry GetGeometry(RenderTarget rt);

        protected override void Dispose(bool disposing)
        {
            Geometry?.Dispose();
        }
    }
}
