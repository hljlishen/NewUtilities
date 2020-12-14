using System.Collections.Generic;
using Utilities.Coordinates;

namespace Utilities.RadarWorks
{
    public class Speed
    {
        double XSpeed { get; }
        double YSpeed { get; }
        double ZSpeed { get; }
        double Vector { get; }
        double SpeedAz { get; }
        double SpeedEl { get; }
    }
    public interface ITrack :ILocatable
    {
        int Id { get; }
        Speed Speed { get; }
        List<PolarCoordinate> TrackTail { get; }
        void UpdateTrack(ITrack t);
    }
}
