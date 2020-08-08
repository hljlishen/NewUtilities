using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.RadarWorks.framework.ButtonLayout
{
    public abstract class LeftBottomButtonLayout : ButtonLayoutBase
    {
        protected abstract int Left { get; }
        protected abstract int Bottom { get; }
        public int LeftMargin { get; set; } = 10;
        public int BottomMargin { get; set; } = 10;

        public override Point NextLocation()
        {
            var y = Bottom - BottomMargin - ButtonSize.Height - (ButtonSize.Height + ButtonAlignmentV) * CurrentRowIndex;
            var x = Left + LeftMargin + CurrentColumnIndex * (ButtonAlignmentH + ButtonSize.Width);

            IncreaseIndex();

            return new Point(x, y);
        }
    }

    public class MapperLeftBottom : LeftBottomButtonLayout
    {
        protected override int Left => (int)Displayer.Mapper.ScreenLeft;

        protected override int Bottom => (int)Displayer.Mapper.ScreenBottom;
    }

    public class ViewLeftBottom : LeftBottomButtonLayout
    {
        protected override int Left => (int)Displayer.Panel.Left;

        protected override int Bottom => (int)Displayer.Panel.Bottom;
    }
}
