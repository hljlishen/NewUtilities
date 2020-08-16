using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using Utilities.RadarWorks;

namespace NewUtilities.RadarWorks.Elements
{
    public class MapperFrame : GraphicElement
    {
        public Color FrameColor { get; set; } = Color.Black;
        public float FrameWidth { get; set; } = 3;

        private Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush brush;

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            brush = FrameColor.SolidBrush(rt);
        }

        public override void Dispose()
        {
            base.Dispose();
            brush?.Dispose();
        }
        protected override void DrawElement(RenderTarget rt)
        {
            RectF r = new RectF((float)Mapper.ScreenLeft, (float)Mapper.ScreenTop, (float)Mapper.ScreenRight, (float)Mapper.ScreenBottom);
            rt.DrawRectangle(r, brush, FrameWidth);
        }
    }
}
