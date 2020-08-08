using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Mapper;

namespace Utilities.Signals
{
    //public class AreaAnimateIterator
    //{
    //    private Area area;
    //    private Area targetArea;
    //    public uint Frames
    //    {
    //        get => frames;
    //        set
    //        {
    //            frames = value;
    //            CalSteps();
    //        }
    //    }
    //    private uint FrameIndex = 0;
    //    private double xLeftStep, xRightStep;
    //    private double yTopStep, yBottomStep;
    //    private uint frames = 15;

    //    public AreaAnimateIterator(Area area, Area targetArea)
    //    {
    //        this.area = area;
    //        this.targetArea = targetArea;
    //        CalSteps();
    //    }

    //    private void CalSteps()
    //    {
    //        xLeftStep = (targetArea.Left - area.Left) / frames;
    //        xRightStep = (targetArea.Right - area.Right) / frames;
    //        yTopStep = (targetArea.Top - area.Top) / frames;
    //        yBottomStep = (targetArea.Bottom - area.Bottom) / frames;
    //    }

    //    public Area NextArea()
    //    {
    //        Area ret;
    //        if (FrameIndex < Frames - 1)
    //        {
    //            ret = new Area(area.Left + xLeftStep * FrameIndex, area.Right + xRightStep * FrameIndex, area.Top + yTopStep * FrameIndex, area.Bottom + yBottomStep * FrameIndex);
    //        }
    //        else if (FrameIndex == Frames - 1)
    //            ret = targetArea;
    //        else
    //            throw new Exception("No Frames Left");
    //        FrameIndex++;
    //        return ret;
    //    }
    //}

    abstract class ValueAnimator<T> where T : struct
    {
        public T Source { get; private set; }
        public T Destny { get; private set; }
        private List<T> Frames = new List<T>();
        private int currentIndex = 0;
        public uint FrameCount
        {
            get => frameCount;
            set
            {
                frameCount = value;
                Frames = GetFrames(Source, Destny, FrameCount).ToList();
            }
        }
        private uint frameCount = 15;

        protected ValueAnimator(T source, T destny, uint frameCount = 15)
        {
            Source = source;
            Destny = destny;
            FrameCount = frameCount;
        }

        protected abstract IEnumerable<T> GetFrames(T source, T destny, uint frameCount);

        public T Next()
        {
            if (currentIndex < Frames.Count)
                return Frames[currentIndex++];
            else
                throw new Exception("No Frames Left");
        }
    }

    class DoubleAnimator : ValueAnimator<double>
    {
        public DoubleAnimator(double source, double destny, uint frameCount = 15) : base(source, destny, frameCount)
        {
        }

        protected override IEnumerable<double> GetFrames(double source, double destny, uint frameCount)
        {
            var step = (destny - source) / frameCount;
            for (int i = 0; i < frameCount - 1; i++)
            {
                yield return source + step * i;
            }
            yield return destny;
        }
    }

    class AreaAnimator : ValueAnimator<Area>
    {
        public AreaAnimator(Area source, Area destny, uint frameCount = 15) : base(source, destny, frameCount)
        {
        }

        protected override IEnumerable<Area> GetFrames(Area source, Area destny, uint frameCount)
        {
            DoubleAnimator left = new DoubleAnimator(source.Left, destny.Left, frameCount);
            DoubleAnimator top = new DoubleAnimator(source.Top, destny.Top, frameCount);
            DoubleAnimator right = new DoubleAnimator(source.Right, destny.Right, frameCount);
            DoubleAnimator bottom = new DoubleAnimator(source.Bottom, destny.Bottom, frameCount);

            for (int i = 0; i < frameCount; i++)
            {
                yield return new Area(left.Next(), right.Next(), top.Next(), bottom.Next());
            }
        }
    }
}
