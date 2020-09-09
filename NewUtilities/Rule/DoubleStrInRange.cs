using Utilities.ValueIntervals;

namespace Utilities.Rules
{
    public class DoubleStrInRange : StringRule
    {
        private ValueInterval interval;
        public DoubleStrInRange(ValueInterval interval) : base(@"^[+-]*[0-9]+(\.[0-9]{1,2})?$")
        {
            this.interval = interval;
        }

        public override bool Pass(string input)
        {
            if (!base.Pass(input))
                return false;
            double value = double.Parse(input);
            return interval.IsInRange(value);
        }

        public override string Hint { get => $"请输入{interval}范围内的浮点";}
    }
}
