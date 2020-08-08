using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace Utilities.RadarWorks
{
    public class LiveRoundedRectangle : LiveGeometry
    {
        public RoundedRect RoundedRect { get; private set; }

        public LiveRoundedRectangle(RectF rect, float radiusX, float radiusY)
        {
            RoundedRect = new RoundedRect(rect, radiusX, radiusY);
        }

        protected override Geometry GetGeometry(RenderTarget rt)
        {
            return rt.Factory.CreateRoundedRectangleGeometry(RoundedRect);
        }
    }
}
