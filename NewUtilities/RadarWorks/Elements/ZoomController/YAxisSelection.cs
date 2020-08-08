using System;
using System.Drawing;

namespace Utilities.RadarWorks
{
    public class YAxisSelection : SelectStrategy
    {
        public override Rectangle CalRect(Point centerPoint, Point CornerPoint)
        {
            var left = Mapper.ScreenLeft;
            var right = Mapper.ScreenRight;
            var top = Math.Min(centerPoint.Y, CornerPoint.Y);
            var bottom = Math.Max(centerPoint.Y, CornerPoint.Y);
            return new Rectangle((int)left, top, (int)right - (int)left, bottom - top);
        }
    }
}
