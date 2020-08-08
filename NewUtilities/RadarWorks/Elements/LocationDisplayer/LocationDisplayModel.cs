using System.Drawing;

namespace Utilities.RadarWorks
{
    public struct LocationDisplayModel
    {
        public CoordinateLocation LocationType;
        public PointF FixLocation;
        public string FontName;
        public float FontSize;
        public Color FontColor;
        public CoordinateType coordinateType;
    }
}
