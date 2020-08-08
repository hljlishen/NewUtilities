namespace Utilities.RadarWorks
{
    public class SquaredReferenceSystem : ReferenceSystem
    {
        public SquaredReferenceSystem(double range) : base(-range, range, range, -range)
        {
        }

        public void SetRange(double range)
        {
            Left = -range;
            Right = range;
            Top = range;
            Bottom = -range;
        }
    }
}
