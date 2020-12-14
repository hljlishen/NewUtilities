using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Tools;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks
{
    class WaveGateSelector : GraphicElement, ISwtichable
    {
        private PointF corner1;
        private PointF corner2;
        private Brush fillBrush;

        private MouseDragDetector dragDetector;

        public string Name { get; set; } = "波门选择";

        private PathGeometry geo;

        public event Action<PointF, PointF> SelectionFinish;

        public override void Dispose()
        {
            base.Dispose();
            fillBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            fillBrush = Color.Yellow.SolidBrush(rt);
            fillBrush.Opacity = 0.8f;
        }

        protected override void BindEvents(Control Panel)
        {
            dragDetector = new MouseDragDetector(Panel);
            dragDetector.MouseDrag += DragDetector_MouseDrag;
            dragDetector.MouseUp += DragDetector_MouseUp;
        }

        private void DragDetector_MouseUp(Point obj)
        {
            var dis = Functions.DistanceBetween(corner1.ToPoint2F(), corner2.ToPoint2F());
            if (dis < 10)
                return;
            SelectionFinish?.Invoke(corner1, corner2);
            corner2 = new PointF();
            corner1 = new PointF();
            Redraw();
        }

        private void DragDetector_MouseDrag(Point arg1, Point arg2)
        {
            corner1 = arg1;
            corner2 = arg2;
            Redraw();
        }

        protected override void UnbindEvents(Control p)
        {
            dragDetector.MouseDrag -= DragDetector_MouseDrag;
            dragDetector.MouseUp -= DragDetector_MouseUp;
            dragDetector?.Dispose();
        }
        protected override void DrawElement(RenderTarget rt)
        {
            geo = LiveSectorRing.GetPathGeometry(rt, ReferenceSystem.ScreenOriginalPoint.ToPoinF(), corner1, corner2);
            rt.FillGeometry(geo, fillBrush);
        }

        public void On() => dragDetector.On();
        public void Off() => dragDetector.Off();
        public bool IsOn => dragDetector.IsOn;
    }
}
