using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using Utilities.RadarWorks;
using System.Collections.Generic;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks.Elements.Markers
{
    public enum YAxisMarkerHandleStyle
    {
        Left,
        Right
    }
    public class MovableYAxisMarker : DynamicElement<double>, ISwtichable
    {
        public Color Color { get; set; } = Color.Blue;
        public bool IsOn => dragDetector.IsOn;
        public string Name { get; set; } = "Y轴标尺";

        public YAxisMarkerHandleStyle HandlePosition { get; set; } = YAxisMarkerHandleStyle.Right;

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

        private void DragDetector_MouseDrag(Point arg1, Point arg2)
        {
            Update(Mapper.GetCoordinateY(arg2.Y));
        }

        public override void Dispose()
        {
            base.Dispose();
            dragDetector.Dispose();
            brush.Dispose();
            format.Dispose();
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            float screenY = (float)Mapper.GetScreenY(Model);
            if (HandlePosition == YAxisMarkerHandleStyle.Left)
            {
                float Left = (float)Mapper.ScreenLeft;

                var p1 = new Point2F(Left + 20, screenY);
                var p2 = new Point2F(Left, screenY - 10);
                var p3 = new Point2F(Left, screenY + 10);

                yield return new LiveLineGeometry(p1, p2, p3);
            }
            else
            {
                float right = (float)Mapper.ScreenRight;
                var p1 = new Point2F(right, screenY - 10);
                var p2 = new Point2F(right, screenY + 10);
                var p3 = new Point2F(right - 20, screenY);

                yield return new LiveLineGeometry(p1, p2, p3);
            }
        }

        public void On() => dragDetector.On();

        public void Off() => dragDetector.Off();

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            float scry = (float)Mapper.GetScreenY(Model);
            using (var factory = DWriteFactory.CreateFactory())
            using (var layout = factory.CreateTextLayout(Model.ToString("0.00"), format, 100, 20))
            using (var s = rt.Factory.CreateStrokeStyle(new StrokeStyleProperties() { DashStyle = DashStyle.Dash }))
            {
                //绘制三角形的Handle
                Objects[0].Fill(rt, brush);

                float left = (float)Mapper.ScreenLeft;
                float right = (float)Mapper.ScreenRight;
                rt.DrawLine(new Point2F(left, scry), new Point2F(right, scry), brush, 3, s);
                layout.TextAlignment = TextAlignment.Center;
                rt.DrawTextLayout(new Point2F(right - 100, scry - 20), layout, brush);
            }
        }
    }
}
