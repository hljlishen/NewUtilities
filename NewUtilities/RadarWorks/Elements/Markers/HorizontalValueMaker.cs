using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System.Collections.Generic;
using System.Drawing;
using Utilities.RadarWorks;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks.Elements.Markers
{
    public abstract class HorizontalValueMarker : MarkerElement
    {
        public Color RectFillColor { get; set; } = Color.White;

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
                Objects[1].DrawFrame(rt, normalBrush, 2, strokeStyle);
                Objects[0].DrawFrame(rt, normalBrush, 2);
            }
            else
            {
                Objects[1].DrawFrame(rt, selectBrush, 4, strokeStyle);
                Objects[0].DrawFrame(rt, selectBrush, 4);
                DrawSelectText(rt);
            }
            Objects[0].Fill(rt, rectFillBrush);
            rt.DrawTextLayout((Objects[0] as LiveRect).Rectangle.Location.ToPoint2F(), textLayout, textBrush);
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            var xLeft = Mapper.ScreenLeft;
            var xRight = Mapper.ScreenRight;
            var y = Mapper.GetScreenY(Model);

            using (var f = DWriteFactory.CreateFactory())
            using (var format = f.CreateTextFormat(TextFont, TextSize))
            {
                textLayout?.Dispose();
                textLayout = format.FitLayout(Model.ToString(ValueFormat));
                var rect = new RectangleF((float)xRight - textLayout.MaxWidth, (float)y - textLayout.MaxHeight / 2, textLayout.MaxWidth, textLayout.MaxHeight);
                yield return new LiveRect(rect);
                yield return new LiveLine(new PointF((float)xLeft, (float)y), new PointF((float)xRight - textLayout.MaxWidth, (float)y));
            }
        }
    }
}
