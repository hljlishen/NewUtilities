using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using NewUtilities.Models;
using NewUtilities.RadarWorks.Elements.Signal;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.Mapper;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks.Elements.Signal
{
    class SeriePlotter : DynamicElement<List<PointD>>, ISwtichable
    {
        public SeriesProperties SeriesProperties { get; private set; }

        private List<SignalMarker> Markers = new List<SignalMarker>();

        private object markerLocker = new object();

        public Area ModelArea { get; private set; }

        public string Name { get; set; } = "";

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
            else if (SeriesProperties.PlotStyle == PlotStyle.Discrete)
                DrawDiscrete(rt);
            else
                DrawDots(rt);
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

        private void DrawDots(RenderTarget rt)
        {
            for (int i = 0; i < Model.Count; i++)
            {
                var str = $"{Model[i].X}";
                var font = "微软雅黑";
                using (TextFormat format = font.MakeFormat(12))
                using (var layout = format.FitLayout(str))
                using (var brush = Color.Black.SolidBrush(rt))
                {
                    var p1 = Mapper.GetScreenLocation(Model[i].X, Model[i].Y);
                    rt.FillEllipse(new Ellipse(p1.ToPoint2F(), 6, 6), signalBrush);
                    rt.DrawTextLayout(new Point2F(p1.X - layout.MaxWidth / 2, p1.Y - 20), layout, brush);
                }
            }
        }


        public bool IsOn { get; private set; } = true;
        public void On()
        {
            IsOn = true;
            ShowMarkers();
            Redraw();
        }

        public void ShowMarkers()
        {
            lock (markerLocker)
            {
                foreach (var m in Markers)
                {
                    m.On();
                }
            }
        }

        public void HideMarkers()
        {
            lock (markerLocker)
            {
                foreach (var m in Markers)
                {
                    m.Off();
                }
            }
        }

        public void Off()
        {
            IsOn = false;
            HideMarkers();
            Redraw();
        }

        public void AddMarker(Color c, float x = 0, bool locked = false)
        {
            var marker = new SignalMarker(c, this) { Model = new PointD(x, 0) };
            marker.On();
            marker.Locked = locked;

            ParentDisplayer.Elements.Add(LayerId, marker);
            lock (markerLocker)
            {
                Markers.Add(marker);
            }
        }

        public void RemoveMarker()
        {
            if (Markers.Count <= 0)
                return;

            ParentDisplayer.Elements.Remove(LayerId, Markers[0]);
            lock (markerLocker)
            {
                Markers[0].Dispose();
                Markers.RemoveAt(0);
            }
        }

        public void ClearMarker()
        {
            var count = Markers.Count;
            for (int i = 0; i < count; i++)
            {
                RemoveMarker();
            }
        }

        protected override void DoUpdate(List<PointD> t)
        {
            base.DoUpdate(t);
            RefreshMarkers();
            SetModelArea();
        }

        private void RefreshMarkers()
        {
            lock (markerLocker)
            {
                foreach (var m in Markers)
                {
                    m.Update(m.Model);
                }
            }
        }

        private void SetModelArea()
        {
            double top = Model[0].Y;
            double bottom = Model[0].X;
            var left = Model[0].X;
            double right = 0;
            var yBottom = Mapper.GetScreenY(0);
            for (int i = 0; i < Model.Count; i++)
            {
                top = Math.Max(Model[i].Y, top);
                bottom = Math.Min(Model[i].Y, bottom);
                if (i == Model.Count - 1)
                    right = Model[i].X;
            }
            if (ModelArea == null)
                ModelArea = new Area(left, right, top, bottom);
            else
                ModelArea.Set(left, right, top, bottom);
        }
    }
}
