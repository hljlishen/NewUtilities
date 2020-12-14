namespace Utilities.RadarWorks.Elements.Markers
{
    public class DynamicVerticalValueMarker : VerticalValueMarker
    {
        public override double MinValue => ReferenceSystem.Left;

        public override double MaxValue => ReferenceSystem.Right;
    }
}
