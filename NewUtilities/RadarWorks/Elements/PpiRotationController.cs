﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Drawing;
using Utilities.Coordinates;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks
{
    public class PpiRotationController : RotationController
    {
        public Color TextColor { get; set; } = Color.Black;
        public Color SelectedMarkerColor { get; set; } = Color.Orange;
        private Brush markerBrush;
        private Brush selectedMarkerBrush;
        private TextFormat textFormat;
        private Brush textBrush;

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            markerBrush = Color.Gray.SolidBrush(rt);
            selectedMarkerBrush = SelectedMarkerColor.SolidBrush(rt);
            textBrush = TextColor.SolidBrush(rt);
            string fontName = "微软雅黑";
            textFormat = fontName.MakeFormat(12);
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            var r = Math.Abs(ReferenceSystem.Right);
            var r1 = r * 0.925;
            var r2 = r * 0.95;
            var r3 = r * 1.05;
            for (int i = 0; i < 360; i++)
            {
                var p1 = new PolarCoordinate(i, 0, r);
                PolarCoordinate p2;
                if (i % 5 == 0)
                    p2 = new PolarCoordinate(i, 0, r1);
                else
                    p2 = new PolarCoordinate(i, 0, r2);
                var p3 = new PolarCoordinate(i, 0, r3);

                var scrP1 = Mapper.GetScreenLocation(p1.X, p1.Y);
                var scrP2 = Mapper.GetScreenLocation(p2.X, p2.Y);
                var scrP3 = Mapper.GetScreenLocation(p3.X, p3.Y);
                if (!isDragging)
                    rt.DrawLine(scrP1.ToPoint2F(), scrP2.ToPoint2F(), markerBrush, 1);
                else
                {
                    rt.DrawLine(scrP1.ToPoint2F(), scrP2.ToPoint2F(), selectedMarkerBrush, 1);
                }

                if (i % 5 == 0)
                    rt.DrawText(i.ToString(), textFormat, new RectangleF((float)(scrP3.X - 5), (float)(scrP3.Y - 5), 100, 100).ToRectF(), textBrush);
            }
        }

        public override void Dispose()
        {
            base.Dispose();
            markerBrush.Dispose();
            selectedMarkerBrush.Dispose();
            textBrush.Dispose();
        }
    }
}
