using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.RadarWorks.framework.ButtonLayout
{
    public abstract class RightBottomButtonLayout : ButtonLayoutBase
    {
        public int RightMargin { get; set; } = 10;
        public int BottomMargin { get; set; } = 10;
        public abstract int Right { get; }
        public abstract int Bottom { get; }

        public override Point NextLocation()
        {
            var x = Right - RightMargin - ButtonSize.Width - CurrentColumnIndex * (ButtonSize.Width + ButtonAlignmentH);
            var y = Bottom - BottomMargin - ButtonSize.Height - CurrentRowIndex * (ButtonSize.Height + ButtonAlignmentV);

            IncreaseIndex();

            return new Point(x, y);
        }
    }

    public class MapperRightBottom : RightBottomButtonLayout
    {
        public override int Right => (int)Displayer.Mapper.ScreenRight;

        public override int Bottom => (int)Displayer.Mapper.ScreenBottom;
    }

    public class ViewRightBottom : RightBottomButtonLayout
    {
        public override int Right => Displayer.Panel.Left;

        public override int Bottom => Displayer.Panel.Bottom;
    }
}
