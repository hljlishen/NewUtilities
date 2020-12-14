using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;
using Utilities.Tools;

namespace Utilities.RadarWorks
{
    public class RotationController : RotatableElement<double>, ISwtichable
    {
        protected bool isDragging = false;
        private double lastAngle;
        private MouseDragDetector dragDetector;

        public RotationController(string rotateDecoratotInstanceName = "default") : base(rotateDecoratotInstanceName)
        {
        }

        public string Name { get; set; } = "旋转控制";

        protected override void DrawDynamicElement(RenderTarget rt)
        {

        }

        protected override void BindEvents(Control p)
        {
            base.BindEvents(p);
            dragDetector = new MouseDragDetector(p);
            dragDetector.MouseDrag += DragDetector_MouseDrag;
            dragDetector.MouseUp += DragDetector_MouseUp;
        }

        private void DragDetector_MouseUp(Point obj)
        {
            isDragging = false;
            lastAngle = (Mapper as PolarRotateDecorator).RotateAngle;
            Redraw();
        }

        private void DragDetector_MouseDrag(Point arg1, Point arg2)
        {
            isDragging = true;
            var angle1 = Functions.AngleToNorth(ReferenceSystem.ScreenOriginalPoint.ToPoinF(), arg1);
            var angle2 = Functions.AngleToNorth(ReferenceSystem.ScreenOriginalPoint.ToPoinF(), arg2);
            var diff = (angle2 - angle1);
            (Mapper as PolarRotateDecorator).RotateAngle = lastAngle + diff;
            Redraw();
        }

        protected override void UnbindEvents(Control p)
        {
            base.UnbindEvents(p);
            dragDetector.MouseDrag += DragDetector_MouseDrag;
            dragDetector.MouseUp += DragDetector_MouseUp;
            dragDetector.Dispose();
        }

        public void On() => dragDetector.On();
        public void Off() => dragDetector.Off();
        public bool IsOn => dragDetector.IsOn;
    }
}
