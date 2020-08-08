using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks.Elements
{
    public class LabelElement : DynamicElement<string>
    {
        private Brush textBrush;
        public Rectangle Rect { get; set; }

        public string Text 
        { 
            get => Model; 
            set
            {
                Model = value;
            }
        }
        public LabelTextProperties TextProperties { get; set; } = new LabelTextProperties();

        public LabelElement(Rectangle rect, string text)
        {
            Rect = rect;
            Text = text;
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            textBrush = TextProperties.TextColor.SolidBrush(rt);
        }

        public override void Dispose()
        {
            base.Dispose();
            textBrush?.Dispose();
        }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            using (var writeFactory = DWriteFactory.CreateFactory())
            using (var format = writeFactory.CreateTextFormat(TextProperties.FontFamilyName, TextProperties.TextSize))
            using(var layout = writeFactory.CreateTextLayout(Model, format, Rect.Width, Rect.Height))
            {
                layout.TextAlignment = TextProperties.Alignment == LabelTextAlignment.Left ? TextAlignment.Leading : TextAlignment.Trailing;
                rt.DrawTextLayout(new Point2F(Rect.Left, Rect.Top), layout, textBrush);
            }
        }
    }

    public class LabelTextProperties
    {
        public string FontFamilyName { get; set; } = "宋体";
        public float TextSize { get; set; } = 16;
        public Color TextColor { get; set; } = Color.Black;
        public LabelTextAlignment Alignment { get; set; } = LabelTextAlignment.Left;
    }

    public enum LabelTextAlignment
    {
        Left,
        Right
    }
}
