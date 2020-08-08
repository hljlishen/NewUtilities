using System.Threading;
using System.Threading.Tasks;
using Utilities.Mapper;
using Utilities.Signals;

namespace Utilities.RadarWorks
{
    class ZoomAnimator 
    {
        private IScreenToCoordinateMapper mapper;
        public int AnimationTime { get; set; } = 500;
        public int UpdateInterval { get; set; } = 30;
        private readonly AreaAnimator iterator;

        public ZoomAnimator(Area targetArea, IScreenToCoordinateMapper mapper)
        {
            this.mapper = mapper;
            iterator = new AreaAnimator(new Area(mapper.CoordinateLeft, mapper.CoordinateRight, mapper.CoordinateTop, mapper.CoordinateBottom), targetArea)
            {
                FrameCount = (uint)(AnimationTime / UpdateInterval)
            };
        }

        public void StartZoom() => Task.Run(AnimateZoom);

        private void AnimateZoom()
        {
            for(int i = 0; i < iterator.FrameCount; i++)
            {
                var area = iterator.Next();
                mapper.SetCoordinateArea(area.Left, area.Right, area.Top, area.Bottom);
                Thread.Sleep(UpdateInterval);
            }
        }
    }
}
