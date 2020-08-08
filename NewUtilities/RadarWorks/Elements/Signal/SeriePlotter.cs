using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks.Elements.Signal
{
    public class SeriePlotter : DynamicElement<List<PointF>>, ISwtichable
    {
        public SeriesProperties SeriesProperties { get; private set; }

        public string Name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        private Brush signalBrush;
        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            signalBrush = rt.CreateSolidColorBrush(SeriesProperties.StrokeColor.ToColorF());
        }

        public override void Dispose()
        {
            base.Dispose();
            signalBrush?.Dispose();
        }

        public SeriePlotter(SeriesProperties seriesProperties)
        {
            this.SeriesProperties = seriesProperties;
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if (!IsDataValid() || !IsOn)
                return;
            if (SeriesProperties.PlotStyle == PlotStyle.Analog)
                DrawAnalog(rt);
            else
                DrawDiscrete(rt);
        }

        private void DrawAnalog(RenderTarget rt)
        {
            for (int i = 0; i < Model.Count - 1; i++)
            {
                var p1 = Mapper.GetScreenLocation(Model[i].X, Model[i].Y);
                var p2 = Mapper.GetScreenLocation(Model[i + 1].X, Model[i + 1].Y);
                rt.DrawLine(p1.ToPoint2F(), p2.ToPoint2F(), signalBrush, SeriesProperties.StrokeWidth);
            }
        }
        private bool IsDataValid() => Model != null && Model.Count > 0;

        private void DrawDiscrete(RenderTarget rt)
        {
            var yBottom = Mapper.GetScreenY(0);
            for (int i = 0; i < Model.Count; i++)
            {
                var p = Mapper.GetScreenLocation(Model[i].X, Model[i].Y);
                rt.DrawLine(new Point2F(p.X, p.Y), new Point2F(p.X, (float)yBottom), signalBrush, 1);
            }
        }


        public bool IsOn { get; private set; } = true;
        public void On() => IsOn = true;
        public void Off() => IsOn = false;
    }

    //public class SerieMarker : GraphicElement
    //{
    //    public SeriePlotter Plotter { get; private set; }
    //    public SerieMarker(RectF rect, SeriePlotter plotter)
    //    {
    //        Rect = rect;
    //        Plotter = plotter;
    //    }

    //    public RectF Rect { get; private set; }
    //    protected override void DrawElement(RenderTarget rt)
    //    {
    //        using(var brush = Plotter.SeriesProperties.StrokeColor.SolidBrush(rt))
    //        {
    //            rt.FillRectangle(Rect, brush);
    //        }
    //    }
    //}
}
