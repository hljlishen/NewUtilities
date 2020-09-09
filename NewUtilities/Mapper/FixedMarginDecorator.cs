using Utilities.Mapper;

namespace NewUtilities.Mapper
{
    public class FixedMarginDecorator : MapperDecorator
    {
        public int TopMargin { get; set; } = 0;
        public int LeftMargin { get; set; } = 0;
        public int RightMargin { get; set; } = 0;
        public int BottomMargin { get; set; } = 0;
        public FixedMarginDecorator(IScreenToCoordinateMapper mapper, int topMargin, int leftMargin, int rightMargin, int bottomMargin) : base(mapper)
        {
            TopMargin = topMargin;
            LeftMargin = leftMargin;
            RightMargin = rightMargin;
            BottomMargin = bottomMargin;
        }

        public FixedMarginDecorator(IScreenToCoordinateMapper mapper) : this(mapper, 0, 0, 0, 0)
        {
        }
        public override void SetScreenArea(double left, double right, double top, double bottom)
        {
            base.SetScreenArea(left + LeftMargin, right - RightMargin, top + TopMargin, bottom - BottomMargin);
        }
    }
}
