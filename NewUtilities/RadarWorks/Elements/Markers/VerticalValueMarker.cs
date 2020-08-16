using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System.Collections.Generic;
using System.Drawing;
using Utilities.RadarWorks;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace NewUtilities.RadarWorks.Elements.Markers
{
    public class VerticalValueMarker : MarkerElement
    {        
        public Color RectFillColor { get; set; } = Color.White;

        public override double MinValue => Mapper.CoordinateLeft;

        public override double MaxValue => Mapper.CoordinateRight;

        protected Brush rectFillBrush;

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            rectFillBrush = RectFillColor.SolidBrush(rt);
        }

        public override void Dispose()
        {
            base.Dispose();
            rectFillBrush?.Dispose();
        }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if (!Selected)
            {
                Objects[1].DrawFrame(rt, normalBrush, 1, strokeStyle);
                Objects[0].DrawFrame(rt, normalBrush, 1);
            }
            else
            {
                Objects[1].DrawFrame(rt, selectBrush, 2, strokeStyle);
                Objects[0].DrawFrame(rt, selectBrush, 2);
            }
            Objects[0].Fill(rt, rectFillBrush);
            rt.DrawTextLayout((Objects[0] as LiveRect).Rectangle.Location.ToPoint2F(), textLayout, textBrush);
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            var yBottom = Mapper.ScreenBottom;
            var yTop = Mapper.ScreenTop;
            var x = Mapper.GetScreenX(Model);

            using (var f = DWriteFactory.CreateFactory())
            using (var format = f.CreateTextFormat(TextFont, TextSize))
            {
                textLayout = f.CreateTextLayout(Model.ToString("0.00"), format, 100, 100);
                textLayout.TextAlignment = TextAlignment.Center;
                var metics = textLayout.Metrics;
                var rect = new RectangleF((float)x - metics.Width / 2, (float)yBottom - metics.Height, metics.Width, metics.Height);
                textLayout.Dispose();
                textLayout = f.CreateTextLayout(Model.ToString("0.00"), format, metics.Width, metics.Height);
                yield return new LiveRect(rect);
                yield return new LiveLine(new PointF((float)x, (float)yBottom - rect.Height), new PointF((float)x, (float)yTop));
            }
        }
    }
}
