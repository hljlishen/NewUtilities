﻿using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Mapper;

namespace Utilities.Signals
{
    public abstract class ValueAnimator<T>
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

    public class DoubleAnimator : ValueAnimator<double>
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
