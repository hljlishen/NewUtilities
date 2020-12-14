using System.Drawing;

namespace Utilities.RadarWorks
{
    public abstract class LeftTopButtonLayout : ButtonLayoutBase
    {
        public int LeftMargin { get; set; } = 10;
        public int TopMargin { get; set; } = 10;
        protected abstract int Left { get; }
        protected abstract int Top { get; }
        public override Point NextLocation()
        {
            var x = Left + LeftMargin + CurrentColumnIndex * (ButtonAlignmentH + ButtonSize.Width);
            var y = Top + TopMargin + CurrentRowIndex * (ButtonAlignmentV + ButtonSize.Height);

            IncreaseIndex();

            return new Point(x, y);
        }
    }

    public class ViewLeftTop : LeftTopButtonLayout
    {
        protected override int Left => 0;

        protected override int Top => 0;
    }

    public class MapperLeftTop : LeftTopButtonLayout
    {
        protected override int Left => (int)Displayer.Mapper.ScreenLeft;

        protected override int Top => (int)Displayer.Mapper.ScreenTop;
    }
}
