using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Drawing;
using System.Windows.Forms;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks
{
    public class LocationDisplay : DynamicElement<Point>
    {
        private Brush textBrush;
        private TextFormat textFormat;
        public string FontName { get; set; }
        public float FontSize { get; set; }
        public Color FontColor { get; set; }

        public CoordinateType CoordinateType { get; set; } = CoordinateType.Rectangular;

        public bool Visible { get; set; } = true;

        public string XValueStringFormat { get; set; } = "0.0000";
        public string YValueStringFormat { get; set; } = "0.0000";

        public Action<string> LocationInfoChanged;

        public LocationDisplay(PositionInfo displayModel, string fontName, float fontSize, Color fontColor)
        {
            DisplayModel = displayModel;
            FontName = fontName;
            FontSize = fontSize;
            FontColor = fontColor;
        }

        public LocationDisplay() : this(new PositionInfo() { FixLocation = new PointF(), LocationType = CoordinateLocation.FixedPosition}, "Berlin Sans FB Demi", 25, Color.White)
        {
        }

        public PositionInfo DisplayModel { get; private set; }

        public override void Dispose()
        {
            base.Dispose();
            textBrush?.Dispose();
            textFormat?.Dispose();
        }
        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            textBrush = FontColor.SolidBrush(rt);
            DWriteFactory dw = DWriteFactory.CreateFactory();
            textFormat = dw.CreateTextFormat(FontName, FontSize);
            dw.Dispose();
        }

        protected override void BindEvents(Control p)
        {
            base.BindEvents(p);
            p.MouseMove += Panel_MouseMove;
        }

        protected override void UnbindEvents(Control p)
        {
            base.UnbindEvents(p);
            p.MouseMove -= Panel_MouseMove;
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            Update(e.Location);
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            string content = GetDisplayContent();
            LocationInfoChanged?.Invoke(content);

            if (!Visible)
                return;
            RectF r = GetDisplayRect(rt);
            rt.DrawText(content, textFormat, r, textBrush);
        }

        private string GetDisplayContent()
        {
            switch (CoordinateType)
            {
                case CoordinateType.Polar:
                    var polarCoodinate = Mapper.GetCoordinateLocation(Model.X, Model.Y).ToRectangularCoordinate().Polar;
                    return $"Az:{polarCoodinate.Az.ToString(XValueStringFormat)} ,Dis:{polarCoodinate.Dis.ToString(YValueStringFormat)}";
                case CoordinateType.Rectangular:
                    var rectangularCoodinate = Mapper.GetCoordinateLocation(Model.X, Model.Y).ToRectangularCoordinate();
                    return $"X:{rectangularCoodinate.X.ToString(XValueStringFormat)} ,Y:{rectangularCoodinate.Y.ToString(YValueStringFormat)}";
                case CoordinateType.Screen:
                    return $"X:{Model.X} ,Y:{Model.Y}";
                default:
                    throw new Exception("CoordinateType变量的值无效");
            }
        }

        private RectF GetDisplayRect(RenderTarget rt)
        {
            if (DisplayModel.LocationType == CoordinateLocation.FollowMouse)
            {
                return new RectF(Model.X + 30, Model.Y, Model.X + 1000, Model.Y + 1000);
            }
            else
            {
                return new RectangleF(DisplayModel.FixLocation.X, DisplayModel.FixLocation.Y, 1000, 1000).ToRectF();
            }
        }
    }
}
