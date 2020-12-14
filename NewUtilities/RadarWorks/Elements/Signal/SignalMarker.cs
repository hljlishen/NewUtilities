using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using Utilities.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.RadarWorks;
using Utilities.RadarWorks.Elements.Signal;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;
using System.Linq;

namespace Utilities.RadarWorks.Elements.Signal
{
    class SignalMarker : DynamicElement<PointD>, ISwtichable
    {
        private ConditionalMouseDragDetector DragDetector;

        private readonly Color Color;
        private Brush shapeBrush;

        private SeriePlotter SeriePlotter = null;

        public bool IsOn { get; private set; }

        public string Name { get; set; } = "";

        public bool Locked { get; set; } = false;

        public SignalMarker(Color color, SeriePlotter seriePlotter = null)
        {
            Color = color;
            SeriePlotter = seriePlotter;
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            DragDetector = new ConditionalMouseDragDetector(d.Panel, IsPointNearAnyObject);
            DragDetector.On();
            DragDetector.MouseDrag += DragDetector_MouseDrag;
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            shapeBrush = Color.SolidBrush(rt);
        }
        protected override IEnumerable<LiveObject> GetObjects()
        {
            var p1 = Mapper.GetScreenLocation(Model.X, Model.Y);
            var p2 = new PointF((float)p1.X - 10, (float)p1.Y - 20);
            var p3 = new PointF((float)p1.X + 10, (float)p1.Y - 20);

            yield return new LiveLineGeometry(p1.ToPoint2F(), p2.ToPoint2F(), p3.ToPoint2F());
        }

        private void DragDetector_MouseDrag(Point arg1, Point arg2)
        {
            if (Locked)
                return;

            base.Update(base.Mapper.GetCoordinateLocation(arg2.X, arg2.Y));
        }

        protected override void DoUpdate(PointD t)
        {
            if (SeriePlotter == null)
                base.DoUpdate(t);
            else
            {
                base.DoUpdate(FindNearestPoint(SeriePlotter.Model, t.X));
            }
        }

        public static PointD FindNearestPoint(PointD[] data, double x)
        {
            if (data == null || data.Length == 0)
                return new PointD();
            PointD lastPoint = data[0];
            foreach (var p in data)
            {
                if (p.X - x < 0)
                    lastPoint = p;
                else
                {
                    if (Math.Abs(lastPoint.X - x) < Math.Abs(p.X - x))
                        return lastPoint;
                    else
                        return p;
                }
            }

            return lastPoint;
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if (!IsOn)
                return;
            var scr = Mapper.GetScreenLocation(Model.X, Model.Y);
            Objects[0].Fill(rt, shapeBrush);
            var text = $"({Model.X:0.00},{Model.Y:0.00})";
            using (var f = DWriteFactory.CreateFactory())
            using (var formart = f.CreateTextFormat("宋体", 12))
            using (var layout = f.CreateTextLayout(text, formart, 100, 20))
            using (var textBrush = Color.Black.SolidBrush(rt))
            {
                layout.TextAlignment = TextAlignment.Center;
                //rt.DrawTextLayout(new Point2F(scr.X - 50, scr.Y - 40), layout, textBrush);
                rt.DrawTextLayout(scr.Move(-50, -40).ToPoint2F(), layout, textBrush);
            }
        }

        public void On()
        {
            IsOn = true;
        }

        public void Off()
        {
            IsOn = false;
        }

        public override void Dispose()
        {
            base.Dispose();
            DragDetector.Dispose();
            shapeBrush?.Dispose();
        }
    }
}
