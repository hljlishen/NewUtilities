using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.Coordinates;
using Utilities.RadarWorks;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks.Elements.Ppi
{
    public class WaveGateModel
    {
        public double DistanceMin { get; set; }
        public double DistanceMax { get; set; }
        public double AngleMin { get; set; }
        public double AngleMax { get; set; }
    }
    class WaveGateElement : RotatableElement<WaveGateModel>
    {
        private Brush frameBrush, normalFillBrush, selectedFillBrush;

        public Color FrameColor { get; set; }
        public Color NormalFillColor { get; set; }
        public Color SelectedFillColor { get; set; }
        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            frameBrush = FrameColor.SolidBrush(rt);
            normalFillBrush = NormalFillColor.SolidBrush(rt);
            normalFillBrush.Opacity = 0.5f;
            selectedFillBrush = SelectedFillColor.SolidBrush(rt);
            selectedFillBrush.Opacity = 0.5f;
        }
        public override void Dispose()
        {
            base.Dispose();
            frameBrush.Dispose();
            normalFillBrush.Dispose();
            selectedFillBrush.Dispose();
        }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            Brush fill;
            if (Selected)
                fill = selectedFillBrush;
            else
                fill = normalFillBrush;

            Objects[0].DrawFrame(rt, frameBrush, 1);
            Objects[0].Fill(rt, fill);
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            if (Model == null)
                yield break;
            var corner1 = new PolarCoordinate(Model.AngleMin, 0, Model.DistanceMin).Rectangular;
            var corner2 = new PolarCoordinate(Model.AngleMax, 0, Model.DistanceMax).Rectangular;

            var scrCnr1 = Mapper.GetScreenLocation(corner1.X, corner1.Y);
            var scrCnr2 = Mapper.GetScreenLocation(corner2.X, corner2.Y);

            yield return new LiveSectorRing(scrCnr1.ToPoinF(), scrCnr2.ToPoinF(), ReferenceSystem.ScreenOriginalPoint.ToPoinF());
        }

        public void Update(PointF scrP1, PointF scrP2)
        {
            var coo1 = Mapper.GetCoordinateLocation(scrP1.X, scrP1.Y).ToRectangularCoordinate().Polar;
            var coo2 = Mapper.GetCoordinateLocation(scrP2.X, scrP2.Y).ToRectangularCoordinate().Polar;
            var model = new WaveGateModel() { AngleMax = Math.Max(coo1.Az, coo2.Az), DistanceMax = Math.Max(coo1.Dis, coo2.Dis), AngleMin = Math.Min(coo1.Az, coo2.Az), DistanceMin = Math.Min(coo1.Dis, coo2.Dis) };
            Update(model);
        }
    }
}
