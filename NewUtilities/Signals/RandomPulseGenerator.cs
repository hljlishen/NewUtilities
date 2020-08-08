using System;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.Signals
{
    public class RandomPulseGenerator : SignalGenerator
    {
        private Random xRdm = new Random(DateTime.Now.Millisecond);
        private Random yRdm = new Random(DateTime.Now.Millisecond + 1000);

        public RandomPulseGenerator(int length)
        {
            Length = length;
        }

        public double Offset { get; set; }
        public int Length { get; set; }

        public int PulseCount { get; set; } = 10;
        public int PulseWidth { get; set; } = 10;

        public IEnumerable<PointF> GetSignalData()
        {
            List<PointF> ret = new List<PointF>();
            for(int i = 0; i < Length; i++)
            {
                ret.Add(new PointF(i, 0));
            }
            for(int i = 0; i < PulseCount; i++)
            {
                var pulseCenter = xRdm.Next(0, Length);
                var pulsePeak = (yRdm.NextDouble() - 0.5) * 2;

                DoubleAnimator values = new DoubleAnimator(pulsePeak, 0, (uint)PulseWidth);
                for (int j = 0; j < PulseWidth; j++)
                {
                    var value = values.Next();
                    if(pulseCenter - j >= 0)
                        ret[pulseCenter - j] = new PointF(pulseCenter - j, (float)value);
                    if(pulseCenter + j < Length)
                        ret[pulseCenter + j] = new PointF(pulseCenter + j, (float)value);
                }
            }

            return ret;
        }
    }
}
