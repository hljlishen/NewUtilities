using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.Tools;

namespace Utilities.Signals
{
    public interface SignalGenerator
    {
        double Offset { get; set; }
        int Length { get; set; }
        IEnumerable<PointF> GetSignalData();
    }

    public class SineGenerator : SignalGeneratorBase
    {
        public SineGenerator(int length) : base(length, a => Math.Sin(Functions.DegreeToRadian(a)))
        {
            Length = length;
        }

    }

    public class CosineGenerator : SignalGeneratorBase
    {
        public CosineGenerator(int length) : base(length, a => Math.Cos(Functions.DegreeToRadian(a)))
        {
        }
    }

    public class SignalGeneratorBase : SignalGenerator
    {
        public double Offset { get; set; } = 0;
        public int Length { get; set; }

        public Func<double, double> seedFunc = null;

        public SignalGeneratorBase(int length, Func<double, double> seedFunc)
        {
            Length = length;
            this.seedFunc = seedFunc;
        }

        public IEnumerable<PointF> GetSignalData()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return new PointF(i, (float)seedFunc(i + Offset));
            }
        }
    }
}
