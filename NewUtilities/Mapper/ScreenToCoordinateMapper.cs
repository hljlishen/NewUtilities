using System;
using System.Drawing;

namespace Utilities.Mapper
{
    public class ScreenToCoordinateMapper : IScreenToCoordinateMapper
    {
        private ValueMapper XAxisMapper { get; set; } = new ValueMapper();
        private ValueMapper YAxisMapper { get; set; } = new ValueMapper();

        public double ScreenLeft => XAxisMapper.Value1Left;

        public double ScreenRight => XAxisMapper.Value1Right;

        public double ScreenTop => YAxisMapper.Value1Left;  //?????

        public double ScreenBottom => YAxisMapper.Value1Right;

        public double CoordinateLeft => XAxisMapper.Value2Left;

        public double CoordinateRight => XAxisMapper.Value2Right;

        public double CoordinateTop => YAxisMapper.Value2Left;  //?????

        public double CoordinateBottom => YAxisMapper.Value2Right;

        public PointF ScreenCenter => new PointF(Math.Abs((float)ScreenLeft + (float)ScreenRight) / 2, Math.Abs((float)ScreenTop + (float)ScreenBottom) / 2);

        public double ScreenWidth => Math.Abs(ScreenRight - ScreenLeft);

        public double ScreenHeight => Math.Abs(ScreenTop - ScreenBottom);

        public ScreenToCoordinateMapper(double screenLeft, double screenRight, double coordinateXLeft, double coordinateXRight, double screenTop, double screenBottom, double coordinateYTop, double coordinateYBottom)
        {
            XAxisMapper.SetRange1(coordinateXLeft, coordinateXRight);
            YAxisMapper.SetRange1(coordinateYTop, coordinateYBottom);
            XAxisMapper.SetRange2(screenLeft, screenRight);
            YAxisMapper.SetRange2(screenTop, screenBottom);
        }

        public ScreenToCoordinateMapper() : this(screenLeft: 0, screenRight: 1, coordinateXLeft: 0, coordinateXRight: 1, screenTop: 1, screenBottom: 0, coordinateYTop: 1, coordinateYBottom: 0)
        { }

        public event Action<IScreenToCoordinateMapper> MapperStateChanged;

        public double GetScreenX(double coordinateX) => XAxisMapper.MapToValue1(coordinateX);
        public double GetScreenY(double coordinateY) => YAxisMapper.MapToValue1(coordinateY);
        public double GetCoordinateX(double screenX) => XAxisMapper.MapToValue2(screenX);
        public double GetCoordinateY(double screenY) => YAxisMapper.MapToValue2(screenY);
        public PointF GetScreenLocation(double coordinateX, double coordinateY) => new PointF((float)GetScreenX(coordinateX), (float)GetScreenY(coordinateY));
        public PointF GetCoordinateLocation(double screenX, double screenY) => new PointF((float)GetCoordinateX(screenX), (float)GetCoordinateY(screenY));

        public void SetScreenArea(double left, double right, double top, double bottom)
        {
            XAxisMapper.SetRange1(left, right);
            YAxisMapper.SetRange1(top, bottom);
            MapperStateChanged?.Invoke(this);
        }

        public void SetCoordinateArea(double left, double right, double top, double bottom)
        {
            XAxisMapper.SetRange2(left, right);
            YAxisMapper.SetRange2(top, bottom);
            MapperStateChanged?.Invoke(this);
        }
    }
}
