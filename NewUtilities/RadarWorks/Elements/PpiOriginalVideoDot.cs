using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Utilities.Coordinates;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks
{
    public struct OriginVideoDotProperty
    {
        public PolarCoordinate Location;
        public int Am;
    }
    public class VideoDot : DynamicElement<OriginVideoDotProperty>
    {
        private Brush fillBrush;

        public bool FixedColor { get; set; } = true;
        public Color Color { get; set; } = Color.Red;

        public float Radius { get; set; } = 3;

        public override void Dispose()
        {
            fillBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            if(FixedColor)
                fillBrush = Color.SolidBrush(rt);   //需要动态计算画刷的颜色
        }
        public VideoDot(PolarCoordinate location, double am = 0)
        {
            var m = Model;
            m.Location = location;
            Model = m;
        }

        public override void SetDisplayer(Displayer d)
        {
            ParentDisplayer = d;
        }

        public VideoDot(OriginVideoDotProperty p) : this(p.Location, p.Am) { }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if (Model.Location.Dis > ReferenceSystem.Right)
                return;
            //var rotatedPoint = new PolarCoordinate(Model.Location.Az , Model.Location.El, Model.Location.Dis).Rectangular;
            //var scrPoint = Mapper.GetScreenLocation(rotatedPoint.X, rotatedPoint.Y);
            var scrPoint = Mapper.GetScreenLocation(X(Model), Y(Model));
            Ellipse e = new Ellipse(scrPoint.ToPoint2F(), Radius, Radius);
            rt.FillEllipse(e, fillBrush);
        }

        public Func<OriginVideoDotProperty, double> X = (p) => p.Location.X;
        public Func<OriginVideoDotProperty, double> Y = (p) => p.Location.Y;
    }
}
