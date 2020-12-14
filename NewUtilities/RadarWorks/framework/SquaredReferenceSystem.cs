namespace Utilities.RadarWorks
{
    public class SquaredReferenceSystem : ReferenceSystem
    {
        public SquaredReferenceSystem(double range) : base(-range, range, range, -range)
        {
        }

        public void SetRange(double range) => SetArea(-range, range, range, -range);
    }
}
