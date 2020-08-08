using Utilities.Coordinates;

namespace Utilities.RadarWorks
{
    public interface ILocatable
    {
        PolarCoordinate Location { get; }
    }
}
