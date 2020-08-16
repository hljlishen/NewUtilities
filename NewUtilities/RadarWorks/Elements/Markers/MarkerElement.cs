using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System.Drawing;
using Utilities.RadarWorks;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace NewUtilities.RadarWorks.Elements.Markers
{
    public abstract class MarkerElement : DynamicElement<double>, IMarkerInterface
    {
        public abstract double MinValue { get; }
        public abstract double MaxValue { get; }
        public float TextSize { get; set; } = 12;
        public string TextFont { get; set; } = "Consolas";
        public Color Color { get; set; } = Color.Green;
        public Color SelectColor { get; set; } = Color.Orange;
        public Color TextColor { get; set; } = Color.Black;
        public float Opacity { get; set; } = 0.5f;
        protected Brush textBrush;
        protected Brush normalBrush;
        protected Brush selectBrush;
        protected StrokeStyle strokeStyle;
        protected TextLayout textLayout;

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            textBrush = TextColor.SolidBrush(rt);
            normalBrush = Color.SolidBrush(rt);
            normalBrush.Opacity = Opacity;
            selectBrush = SelectColor.SolidBrush(rt);
            strokeStyle = rt.Factory.CreateStrokeStyle(new StrokeStyleProperties() { DashStyle = DashStyle.Dash });
        }

        public override void Dispose()
        {
            base.Dispose();
            textBrush?.Dispose();
            normalBrush?.Dispose();
            selectBrush?.Dispose();
            strokeStyle?.Dispose();
            textLayout?.Dispose();
        }

        protected void DrawSelectText(RenderTarget rt)
        {
            using (var factory = DWriteFactory.CreateFactory())
            using (var f = factory.CreateTextFormat(TextFont, TextSize))
            using (var l = f.FitLayout(Model.ToString("0.0")))
            {
                var location = sensor.MouseLocation;
                rt.DrawTextLayout(location.OffSet(-l.MaxWidth, -l.MaxHeight).ToPoint2F(), l, selectBrush);
            }
        }
    }
}
