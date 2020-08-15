﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
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
    public class PpiOriginalVideoDot : DynamicElement<OriginVideoDotProperty>
    {
        private Brush fillBrush;

        public override void Dispose()
        {
            fillBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            fillBrush = Color.Red.SolidBrush(rt);   //需要动态计算画刷的颜色
        }
        public PpiOriginalVideoDot(PolarCoordinate location, double am = 0)
        {
            Model.Location = location;
        }

        public override void SetDisplayer(Displayer d)
        {
            ParentDispalyer = d;
        }

        public PpiOriginalVideoDot(OriginVideoDotProperty p) : this(p.Location, p.Am) { }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if (Model.Location.Dis > ReferenceSystem.Top)
                return;
            var rotatedPoint = new PolarCoordinate(Model.Location.Az , Model.Location.El, Model.Location.Dis).Rectangular;
            var scrPoint = Mapper.GetScreenLocation(rotatedPoint.X, rotatedPoint.Y);
            Ellipse e = new Ellipse(scrPoint.ToPoint2F(), 3, 3);
            rt.FillEllipse(e, fillBrush);
        }
    }
}
