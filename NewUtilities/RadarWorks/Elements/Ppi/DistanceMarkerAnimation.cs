using System;
using System.Threading.Tasks;

namespace Utilities.RadarWorks
{
    class DistanceMarkerAnimation
    {
        public double TargetDistance { get; set; }
        public uint Frames { get; set; } = 10;
        public uint Interval { get; set; } = 100;

        private ReferenceSystem referenceSystem;
        private double targetRange;

        public DistanceMarkerAnimation(ReferenceSystem referenceSystem, double targetRange)
        {
            this.referenceSystem = referenceSystem ?? throw new ArgumentNullException(nameof(referenceSystem));
            this.targetRange = targetRange;
        }

        public void StartSetRange()
        {
            Task.Run(SetRange);
        }

        private void SetRange()
        {
            var step = (referenceSystem.Top - targetRange) / Frames;
            var startRange = referenceSystem.Top;
            for (int i = 0; i < Frames - 1; i ++)
            {
                var range = startRange + step * i;
                referenceSystem.SetArea(-range, range, range, -range);
            }

            referenceSystem.SetArea(-targetRange, targetRange, targetRange, -targetRange);
        }
    }
}
