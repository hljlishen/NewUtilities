using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;
using Utilities.Coordinates;

namespace Utilities.Models
{
    public struct PointD
    {
        public PointD(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; set; }
        public double Y { get; set; }

        public PointF ToPoinF() => new PointF((float)X, (float)Y);
        public Point2F ToPoint2F() => new Point2F((float)X, (float)Y);

        public PointD Move(double x, double y) => new PointD(x + x, y + y);

        public RectangularCoordinate ToRectangularCoordinate() => new RectangularCoordinate(X, Y, 0);
    }
}
