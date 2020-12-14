using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using Utilities.RadarWorks;
using System.Collections.Generic;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks.Elements
{
    public enum XAxisMarkerHandleStyle
    {
        Top,
        Bottom
    }
    public class MovableXAxisMarker : DynamicElement<double>, ISwtichable
    {
        public Color Color { get; set; } = Color.Blue;
        public bool IsOn => dragDetector.IsOn;
        public string Name { get; set; } = "X轴标尺";

        public XAxisMarkerHandleStyle HandlePosition { get; set; } = XAxisMarkerHandleStyle.Top;

        private MouseDragDetector dragDetector;
        private Brush brush;
        private TextFormat format;

        protected override void InitializeComponents(RenderTarget rt)
        {
            brush = Color.SolidBrush(rt);
            var factory = DWriteFactory.CreateFactory();
            format = factory.CreateTextFormat("微软雅黑", 12);
            factory.Dispose();
        }
        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);

            dragDetector = new ConditionalMouseDragDetector(d.Panel, IsPointNearAnyObject);
            dragDetector.Off();
            dragDetector.MouseDrag += DragDetector_MouseDrag;

        }

        private void DragDetector_MouseDrag(Point arg1, Point arg2) => Update(Mapper.GetCoordinateX(arg2.X));

        public override void Dispose()
        {
            base.Dispose();
            dragDetector.Dispose();
            brush.Dispose();
            format.Dispose();
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            float screenX = (float)Mapper.GetScreenX(Model);
            if (HandlePosition == XAxisMarkerHandleStyle.Top)
            {
                var top = Mapper.ScreenTop + 40;

                var p1 = new Point2F(screenX, (float)top);
                var p2 = new Point2F(screenX - 10, (float)top - 20);
                var p3 = new Point2F(screenX + 10, (float)top - 20);

                yield return new LiveLineGeometry(p1, p2, p3);
            }
            else
            {
                var Bottom = Mapper.ScreenBottom;
                var p1 = new Point2F((float)(screenX), (float)Bottom-20);
                var p2 = new Point2F(screenX - 10, (float)Bottom);
                var p3 = new Point2F(screenX + 10, (float)Bottom);

                yield return new LiveLineGeometry(p1, p2, p3);
            }
        }

        public void On() => dragDetector.On();

        public void Off() => dragDetector.Off();

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            float scrx = (float)Mapper.GetScreenX(Model);
            using(var factory = DWriteFactory.CreateFactory())
            using(var layout =  factory.CreateTextLayout(Model.ToString("0.00"), format, 100, 20))
            using (var s = rt.Factory.CreateStrokeStyle(new StrokeStyleProperties() { DashStyle = DashStyle.Dash }))
            {
                //绘制三角形的Handle
                Objects[0].Fill(rt, brush);

                var top = Mapper.ScreenTop  + 20;
                var bottom = Mapper.ScreenBottom;
                rt.DrawLine(new Point2F(scrx, (float)top), new Point2F(scrx, (float)bottom), brush, 3, s);
                layout.TextAlignment = TextAlignment.Center;
                rt.DrawTextLayout(new Point2F(scrx - 50, (float)Mapper.ScreenTop), layout, brush);
            }
        }
    }
}
