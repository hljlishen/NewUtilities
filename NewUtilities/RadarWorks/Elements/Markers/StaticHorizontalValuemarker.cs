namespace Utilities.RadarWorks.Elements.Markers
{
    public class StaticHorizontalValueMarker : HorizontalValueMarker
    {
        public override double MinValue => Mapper.CoordinateBottom;

        public override double MaxValue => Mapper.CoordinateTop;
    }
}
