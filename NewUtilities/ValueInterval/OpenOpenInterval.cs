namespace Utilities.ValueIntervals
{
    class OpenOpenInterval : ValueInterval
    {
        public override bool IsInRange(double value) => value > Min && value < Max;

        public override string ToString() => $"({Min}, {Max})";
    }
}
