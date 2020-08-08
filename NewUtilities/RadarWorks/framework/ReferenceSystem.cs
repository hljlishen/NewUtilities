using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using Utilities.Mapper;
using Utilities.Signals;

namespace Utilities.RadarWorks
{
    public class ReferenceSystem
    {
        public ReferenceSystem(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
        public bool Animation { get; set; } = true;
        public double Left { get; protected set; }
        public double Right { get; protected set; }
        public double Top { get; protected set; }
        public double Bottom { get; protected set; }
        public PointF ScreenOriginalPoint { get; private set; }/* => Mapper.GetScreenLocation(0, 0);*/
        public double ScreenLeft { get; private set; } /*=> Mapper.GetScreenX(Left);*/
        public double ScreenRight { get; private set; } /*=> Mapper.GetScreenX(Right);*/
        public double ScreenTop { get; private set; } /*=> Mapper.GetScreenY(Top);*/
        public double ScreenBottom { get; private set; } /*=> Mapper.GetScreenY(Bottom);*/
        public double ScreenWidth { get; private set; } /*=> ScreenRight - ScreenLeft;*/
        public double ScreenHeight { get; private set; } /*=> ScreenBottom - ScreenTop;*/
        public double XDistance => Math.Abs(Right - Left);
        public double YDistance => Math.Abs(Top - Bottom);
        public IScreenToCoordinateMapper Mapper { get; private set; }
        public void SetMapper(IScreenToCoordinateMapper mapper)
        {
            Mapper = mapper;
            Mapper.SetCoordinateArea(Left, Right, Top, Bottom);
            Mapper.MapperStateChanged += Mapper_MapperStateChanged;
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj)
        {
            ScreenOriginalPoint = Mapper.GetScreenLocation(0, 0);
            ScreenLeft = Mapper.GetScreenX(Left);
            ScreenRight = Mapper.GetScreenX(Right);
            ScreenTop = Mapper.GetScreenX(Top);
            ScreenBottom = Mapper.GetScreenX(Bottom);
            ScreenHeight = Math.Abs(ScreenTop - ScreenBottom);
            ScreenWidth = Math.Abs(ScreenRight - ScreenLeft);
        }

        public void SetArea(double left, double right, double top, double bottom)
        {
            if(!Animation)
                DoSetArea(left, right, top, bottom);
            else
                AnimateSetArea(left, right, top, bottom);
        }

        internal void DoSetArea(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
            Mapper.SetCoordinateArea(left, right, top, bottom);
        }

        internal void AnimateSetArea(double left, double right, double top, double bottom)
        {
            var animator = new ChangeRangeAnimator(this, new Area(left, right, top, bottom));
            animator.StartSetRange();
        }
    }

    class ChangeRangeAnimator
    {
        private readonly ReferenceSystem referenceSystem;
        //private Area targetArea;
        private readonly AreaAnimator iterator;
        public int AnimationInterval { get; set; } = 30;
        public int AnimationTime { get; set; } = 500;

        public ChangeRangeAnimator(ReferenceSystem r, Area t)
        {
            this.referenceSystem = r;
            //this.targetArea = t;
            iterator = new AreaAnimator(new Area(r.Left, r.Right, r.Top, r.Bottom), t)
            {
                FrameCount = (uint)(AnimationTime / AnimationInterval)
            };
        }

        public void StartSetRange() => Task.Run(SetRange);

        private void SetRange()
        {
            for (int i = 0; i < iterator.FrameCount; i++)
            {
                var area = iterator.Next();
                referenceSystem.DoSetArea(area.Left, area.Right, area.Top, area.Bottom);
                Thread.Sleep(AnimationInterval);
            }
        }
    }
}
