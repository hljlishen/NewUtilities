using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks
{
    public class AxisY : GraphicElement
    {
        private Brush axisBrush;
        public Color Color { get; set; } = Color.White;
        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            axisBrush = Color.SolidBrush(rt);
        }

        public override void Dispose()
        {
            base.Dispose();
            axisBrush?.Dispose();
        }
        protected override void DrawElement(RenderTarget rt)
        {
            var originalPoint = ReferenceSystem.ScreenOriginalPoint;
            var screenTop = Mapper.GetScreenY(ReferenceSystem.Top);
            var screenBottom = Mapper.GetScreenY(ReferenceSystem.Bottom);

            rt.DrawLine(new Point2F(originalPoint.X, (float)screenTop), new Point2F(originalPoint.X, (float)screenBottom), axisBrush, 2);
        }
    }
}
