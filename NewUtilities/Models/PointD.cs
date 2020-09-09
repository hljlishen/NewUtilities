using System.Drawing;

namespace NewUtilities.Models
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
    }
}
