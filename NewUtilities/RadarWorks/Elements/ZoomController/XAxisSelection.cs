using System;
using System.Drawing;

namespace Utilities.RadarWorks
{
    public class XAxisSelection : SelectStrategy
    {
        public override Rectangle CalRect(Point centerPoint, Point CornerPoint)
        {
            var left = Math.Min(centerPoint.X, CornerPoint.X);
            var right = Math.Max(centerPoint.X, CornerPoint.X);
            var top = Mapper.ScreenTop;
            var bottom = Mapper.ScreenBottom;
            return new Rectangle(left, (int)top, right - left, (int)bottom - (int)top);
        }
    }
}
