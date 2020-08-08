using System.Drawing;

namespace Utilities.RadarWorks
{
    public abstract class RightTopButtonLayout : ButtonLayoutBase
    {
        public int TopMargin { get; set; } = 10;
        public int RightMargin { get; set; } = 10;

        public override Point NextLocation()
        {

            var x = Right - RightMargin - ButtonSize.Width + CurrentColumnIndex * (ButtonAlignmentH + ButtonSize.Width);
            var y = Top + TopMargin + CurrentRowIndex * (ButtonAlignmentV + ButtonSize.Height);
            IncreaseIndex();
            return new Point(x, y);
        }

        protected abstract int Right { get; }
        protected abstract int Top { get; }
    }

    public class ViewRightTop : RightTopButtonLayout
    {
        protected override int Right => Displayer.Panel.Right;

        protected override int Top => Displayer.Panel.Top;
    }

    public class MapperRightTop : RightTopButtonLayout
    {
        protected override int Right => (int)Displayer.Mapper.ScreenRight;

        protected override int Top => (int)Displayer.Mapper.ScreenTop;
    }
}
