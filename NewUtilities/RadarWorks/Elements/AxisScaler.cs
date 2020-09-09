using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Utilities.RadarWorks;
using Utilities.RadarWorks.framework;
using Utilities.ValueIntervals;

namespace NewUtilities.RadarWorks.Elements
{
    public class AxisScaler : GraphicElement
    {
        public ButtonOrgnizer Row1 = new ButtonOrgnizer(new MapperLeftTop() { ColumnSize = 1, ButtonSize = new System.Drawing.Size(40, 40), LeftMargin = 60 });
        public ButtonOrgnizer Row2 = new ButtonOrgnizer(new MapperLeftTop() { ColumnSize = 3, ButtonSize = new System.Drawing.Size(40, 40), LeftMargin = 10, TopMargin = 60 });
        public ButtonOrgnizer Row3 = new ButtonOrgnizer(new MapperLeftTop() { ColumnSize = 1, ButtonSize = new System.Drawing.Size(40, 40), LeftMargin = 60, TopMargin = 110 });
        public float XRatio { get; set; } = 0.3f;
        public float YRatio { get; set; } = 0.3f;
        private void ExtendX(float ratio)
        {
            var min = Mapper.CoordinateLeft;
            var max = Mapper.CoordinateRight;
            ValueInterval.ExtendInterval(ref min, ref max, ratio);
            Mapper.SetCoordinateArea(min, max, Mapper.CoordinateTop, Mapper.CoordinateBottom);
        }
        private void ExtendY(float ratio)
        {
            var min = Mapper.CoordinateBottom;
            var max = Mapper.CoordinateTop;
            ValueInterval.ExtendInterval(ref min, ref max, ratio);
            Mapper.SetCoordinateArea(Mapper.CoordinateLeft, Mapper.CoordinateRight, max, min);
        }
        public void XUp(ButtonElement e) => ExtendX(1 - XRatio);
        public void XDown(ButtonElement e) => ExtendX(1 + XRatio);
        public void YUp(ButtonElement e) => ExtendY(1 - YRatio);
        public void YDown(ButtonElement e) => ExtendY(1 + YRatio);

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            d.Elements.Add(LayerId, Row1);
            d.Elements.Add(LayerId, Row2);
            d.Elements.Add(LayerId, Row3);
            InitRows();
        }

        private void InitRows()
        {
            Row1.AddButton("↑\r\n↓", YUp);
            Row2.AddButton("→←", XDown);
            Row2.AddButton("默认", Reset);
            Row2.AddButton("←→", XUp);
            Row3.AddButton("↓\r\n↑", YDown);
        }

        public void Reset(ButtonElement e)
        {
            Mapper.SetCoordinateArea(ReferenceSystem.Left, ReferenceSystem.Right, ReferenceSystem.Top, ReferenceSystem.Bottom);
        }
        protected override void DrawElement(RenderTarget rt)
        {
            //throw new NotImplementedException();
        }
    }
}
