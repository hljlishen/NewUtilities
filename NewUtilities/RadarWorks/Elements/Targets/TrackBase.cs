using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using Utilities.Coordinates;

namespace Utilities.RadarWorks
{
    public class TrackBase : ITrack
    {
        public int Id { get; private set; }

        public Speed Speed { get; private set; }

        public List<PolarCoordinate> TrackTail { get; private set; } = new List<PolarCoordinate>();

        public PolarCoordinate Location { get; private set; }

        public virtual void UpdateTrack(ITrack t)
        {
            TrackTail.Add(Location);
            Speed = t.Speed;
            Location = t.Location;
        }
    }

    //public class TrackElementBase : DynamicElement<ITrack>
    //{
    //    protected override void DrawDynamicElement(RenderTarget rt)
    //    {
    //        throw new System.NotImplementedException();
    //    }

    //    protected override IEnumerable<LiveObject> GetObjects()
    //    {
    //        return base.GetObjects();
    //    }
    //}
}
