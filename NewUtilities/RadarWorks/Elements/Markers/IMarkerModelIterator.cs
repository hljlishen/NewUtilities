using System;

namespace Utilities.RadarWorks.Elements.Markers
{
    public interface IMarkerModelIterator
    {
        double Min { get; }
        double Max { get; }
        int MarkerCount { get; }
        double Next();
        void Reset();
    }

    public enum MarkerIteratorType
    {
        NoLeft,
        NoRight,
        NoLeftOrRight,
        LeftAndRight
    }

    public abstract class MarkerModelIterator : IMarkerModelIterator
    {
        public double Min { get; protected set; }
        public double Max { get; protected set; }
        public int MarkerCount { get; protected set; }

        protected int currentIndex = 0;

        public static MarkerModelIterator GetIterator(MarkerIteratorType type, double min, double max, int markerCount)
        {
            switch (type)
            {
                case MarkerIteratorType.NoLeft:
                    return new NoLeft(min, max, markerCount);
                case MarkerIteratorType.NoRight:
                    return new NoRight(min, max, markerCount);
                case MarkerIteratorType.NoLeftOrRight:
                    return new NoLeftOrRight(min, max, markerCount);
                case MarkerIteratorType.LeftAndRight:
                    return new LeftAndRight(min, max, markerCount);
                default:
                    throw new ArgumentException("错误的MarkerIteratorType类型");
            }
        }

        protected MarkerModelIterator(double min, double max, int markerCount)
        {
            if (markerCount <= 0)
                throw new ArgumentException("markerCount不能小于等于0");
            Min = min;
            Max = max;
            MarkerCount = markerCount;
        }

        public void Reset() => currentIndex = 0;
        public abstract double Next();
    }

    internal class NoLeftOrRight : MarkerModelIterator
    {
        public NoLeftOrRight(double min, double max, int markerCount) : base(min, max, markerCount)
        {
        }

        public override double Next()
        {
            var step = (Max - Min) / (MarkerCount + 1);
            return Min + step * ++currentIndex;
        }
    }

    internal class NoLeft : MarkerModelIterator
    {
        public NoLeft(double min, double max, int markerCount) : base(min, max, markerCount)
        {
        }
        public override double Next()
        {
            var step = (Max - Min) / MarkerCount;
            return Min + step * ++currentIndex;
        }
    }

    internal class NoRight : MarkerModelIterator
    {
        public NoRight(double min, double max, int markerCount) : base(min, max, markerCount)
        {
        }
        public override double Next()
        {
            var step = (Max - Min) / MarkerCount;
            return Min + step * currentIndex++;
        }
    }

    internal class LeftAndRight : MarkerModelIterator
    {
        public LeftAndRight(double min, double max, int markerCount) : base(min, max, markerCount)
        {
        }
        public override double Next()
        {
            var step = (Max - Min) / (MarkerCount - 1);
            return Min + step * currentIndex++;
        }
    }
}
