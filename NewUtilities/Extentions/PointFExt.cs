using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Coordinates;

namespace System.Drawing
{
    public static class PointFExt
    {
        public static RectangularCoordinate ToRectangularCoordinate(this PointF p) => new RectangularCoordinate(p.X, p.Y, 0);
    }
}
