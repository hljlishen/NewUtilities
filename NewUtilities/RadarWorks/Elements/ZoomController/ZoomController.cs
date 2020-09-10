using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks
{
    public class ZoomController : GraphicElement, ISwtichable
    {
        private Brush fillBrush;
        private Brush frameBrush;
        private Rectangle selectedRect;
        public MouseDragDetector dragDetector;
        public bool Animation { get; set; } = true;
        public SelectStrategy SelectStrategy { get; protected set; }

        public string Name { get; set; } = "放缩控制";

        public override void Dispose()
        {
            base.Dispose();
            fillBrush?.Dispose();
            frameBrush?.Dispose();
        }
        protected override void UnbindEvents(Control p)
        {
            dragDetector.MouseDrag -= DragDetector_MouseDrag;
            dragDetector.MouseUp -= DragDetector_MouseUp;
            dragDetector.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            fillBrush = Color.Blue.SolidBrush(rt);
            fillBrush.Opacity = 0.5f;
            frameBrush = Color.White.SolidBrush(rt);
        }

        public ZoomController() : this(new RectangleSelection()) { }
        public ZoomController(SelectStrategy selectStrategy)
        {
            SelectStrategy = selectStrategy;
            SelectStrategy.SetZoomController(this);
        }

        protected override void BindEvents(Control p)
        {
            dragDetector = new MouseDragDetector(p);
            dragDetector.MouseDrag += DragDetector_MouseDrag;
            dragDetector.MouseUp += DragDetector_MouseUp;
        }

        private void DragDetector_MouseUp(Point obj)
        {
            if (!SelectStrategy.IsRectBigEnough(selectedRect, Mapper))
            {
                Redraw();
            }
            else
            {
                var left = Mapper.GetCoordinateX(selectedRect.Left);
                var right = Mapper.GetCoordinateX(selectedRect.Right);
                var top = Mapper.GetCoordinateY(selectedRect.Top);
                var bottom = Mapper.GetCoordinateY(selectedRect.Bottom);
                SetMapperCoordinateArea(left, right, top, bottom);
            }

            //此处必须清零coverRect，原因是：当鼠标只做点击不拖动时，coverRect会保留上次放缩时计算的CoverRect值，因此会通过IsRectBigEnough的校验
            selectedRect = new Rectangle(0, 0, 0, 0);
            return;
        }

        private void DragDetector_MouseDrag(Point arg1, Point arg2)
        {
            selectedRect = SelectStrategy.CalRect(arg1, arg2);
            Redraw();
        }

        private void SetMapperCoordinateArea(double left, double right, double top, double bottom)
        {
            if (!Animation)
                Mapper.SetCoordinateArea(left, right, top, bottom);
            else
            {
                Area targetArea = new Area(left, right, top, bottom);
                ZoomAnimator zoomAnimation = new ZoomAnimator(targetArea, Mapper);
                zoomAnimation.StartZoom();
            }
        }

        public void Reset() => SetMapperCoordinateArea(ReferenceSystem.Left, ReferenceSystem.Right, ReferenceSystem.Top, ReferenceSystem.Bottom);

        protected override void DrawElement(RenderTarget rt)
        {
            SelectStrategy.DrawZoomView(selectedRect.ToRectF(), rt, fillBrush, frameBrush, 2);
        }

        public void SetStrategy(SelectStrategy s)
        {
            lock (Locker)
            {
                SelectStrategy = s;
                s.SetZoomController(this);
            }
        }

        public void On() => dragDetector.On();
        public void Off() => dragDetector.Off();
        public bool IsOn => dragDetector.IsOn;
    }
}
