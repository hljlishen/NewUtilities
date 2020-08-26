using System;

namespace Utilities.Mapper
{
    public class Area
    {
        public double Left;
        public double Right;
        public double Top;
        public double Bottom;

        public Area(double left, double right, double top, double bottom)
        {
            Set(left, right, top, bottom);
        }

        public void Set(double left, double right, double top, double bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public double VerticalCover => Math.Abs(Top - Bottom);
        public double HorizontalCover => Math.Abs(Right - Left);

        public override string ToString()
        {
            return $"[left:{Left}, right:{Right}, top:{Top}, bottom:{Bottom}]";
        }
    }
}
