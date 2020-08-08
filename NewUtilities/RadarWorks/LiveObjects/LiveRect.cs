using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;

namespace Utilities.RadarWorks
{
    public class LiveRect : LiveObject
    {
        public LiveRect(RectangleF rectangle)
        {
            Rectangle = rectangle;
        }

        public RectangleF Rectangle { get; set; }
        public override bool IsPointNear(PointF mouseLocation) => Rectangle.IsPointInRect(mouseLocation);

        public override void Fill(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush fillBrush)
        {
            rt.FillRectangle(Rectangle.ToRectF(), fillBrush);
        }

        public override void DrawFrame(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush frameBrush, float strokeWidth, StrokeStyle style)
        {
            rt.DrawRectangle(Rectangle.ToRectF(), frameBrush, strokeWidth, style);
        }
    }
}
