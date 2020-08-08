using System;

namespace Utilities.Circlation
{
    public class RangedInt
    {
        public RangedInt(int min, int max)
        {
            if (max <= min)
                throw new ArgumentException("max小于或等于min");
            Max = max;
            Min = min;
            Value = min;
        }

        public int Next
        {
            get
            {
                var ret = Value++;

                if (Value > Max)
                    Value = Min;
                return ret;
            }
        }

        public void Reset() => Value = Min;

        private int Value;
        public int Max { get; private set; }
        public int Min { get; private set; }
    }
}
