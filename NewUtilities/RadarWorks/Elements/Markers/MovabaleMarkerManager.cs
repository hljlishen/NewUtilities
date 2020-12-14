using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.RadarWorks.Elements.Markers
{
    public class MovabaleMarkerManager : GraphicElement
    {
        private List<MovableXAxisMarker> xAxisMarkers = new List<MovableXAxisMarker>();
        private List<MovableYAxisMarker> yAxisMarkers = new List<MovableYAxisMarker>();

        public XAxisMarkerHandleStyle XAxisMarkerHandleStyle { get; set; } = XAxisMarkerHandleStyle.Top;
        public YAxisMarkerHandleStyle YAxisMarkerHandleStyle { get; set; } = YAxisMarkerHandleStyle.Left;
        public void AddXMarker()
        {
            var xMarker = new MovableXAxisMarker() { Color = DrawingExtionFuncs.RandomColor(), Model = GetRandomXCoordinate(), HandlePosition = XAxisMarkerHandleStyle };
            ParentDisplayer.Elements.Add(LayerId, xMarker);
            xAxisMarkers.Add(xMarker);
            xMarker.On();
        }

        public void RemoveXMarker()
        {
            if (xAxisMarkers.Count <= 0)
                return;
            var xMarker = xAxisMarkers[0];
            xAxisMarkers.RemoveAt(0);
            ParentDisplayer.Elements.Remove(LayerId, xMarker);
        }

        public void RemoveAllXMarkers()
        {
            var count = xAxisMarkers.Count;
            for (int i = 0; i < count; i++)
                RemoveXMarker();
        }

        public void AddYMarker()
        {
            var yMarker = new MovableYAxisMarker() { Color = DrawingExtionFuncs.RandomColor(), Model = GetRandomYCoordinate(), HandlePosition = YAxisMarkerHandleStyle };
            ParentDisplayer.Elements.Add(LayerId, yMarker);
            yAxisMarkers.Add(yMarker);
            yMarker.On();
        }

        public void RemoveYMarker()
        {
            if (yAxisMarkers.Count <= 0)
                return;
            var yMarker = yAxisMarkers[0];
            yAxisMarkers.RemoveAt(0);
            ParentDisplayer.Elements.Remove(LayerId, yMarker);
        }

        public void RemoveAllYMarkers()
        {
            var count = yAxisMarkers.Count;
            for (int i = 0; i < count; i++)
                RemoveYMarker();
        }

        public double GetRandomXCoordinate()
        {
            var r = new Random(DateTime.Now.Millisecond);
            var random = r.NextDouble(0.2, 0.8);
            var max = Math.Max(Mapper.CoordinateLeft, Mapper.CoordinateRight);
            var distance = Math.Abs(Mapper.CoordinateLeft - Mapper.CoordinateRight);
            return distance * random + Mapper.CoordinateLeft;
        }

        public double GetRandomYCoordinate()
        {
            var r = new Random(DateTime.Now.Millisecond);
            var random = r.NextDouble(0.2, 0.8);
            var max = Math.Max(Mapper.CoordinateTop, Mapper.CoordinateBottom);
            return max * random;
        }
        protected override void DrawElement(RenderTarget rt)
        {
        }
    }
}
