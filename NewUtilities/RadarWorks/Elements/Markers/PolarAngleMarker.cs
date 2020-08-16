﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.RadarWorks;
using Utilities.Tools;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace NewUtilities.RadarWorks.Elements.Markers
{
    public class PolarAngleMarker : RotatableElement<double>, IMarkerInterface
    {
        public double MinValue => 0;

        public double MaxValue => 360;

        public int TextHeight { get; set; } = 20;
        public float TextSize { get; set; } = 12;
        public string TextFont { get; set; } = "Consolas";
        public Color Color { get; set; } = Color.Green;
        public Color SelectColor { get; set; } = Color.Orange;
        public Color TextColor { get; set; } = Color.Black;
        public float Opacity { get; set; } = 0.5f;
        protected Brush textBrush;
        protected Brush normalBrush;
        protected Brush selectBrush;
        protected StrokeStyle strokeStyle;
        protected TextLayout textLayout;

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            textBrush = TextColor.SolidBrush(rt);
            normalBrush = Color.SolidBrush(rt);
            normalBrush.Opacity = Opacity;
            selectBrush = SelectColor.SolidBrush(rt);
            strokeStyle = rt.Factory.CreateStrokeStyle(new StrokeStyleProperties() { DashStyle = DashStyle.Dash });
        }

        public override void Dispose()
        {
            base.Dispose();
            textBrush?.Dispose();
            normalBrush?.Dispose();
            selectBrush?.Dispose();
            strokeStyle?.Dispose();
            textLayout?.Dispose();
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if(Selected)
            {
                Objects[0].DrawFrame(rt, selectBrush, 4, strokeStyle);
            }
            else
                Objects[0].DrawFrame(rt, normalBrush, 2, strokeStyle);
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            var center = ReferenceSystem.ScreenOriginalPoint;
            var radius = InnerMapper.GetScreenLocation(ReferenceSystem.Right, 0).X - center.X;
            var x = center.X + radius * (float)Math.Sin(Functions.DegreeToRadian(Model + RotateAngle));
            var y = center.Y - radius * (float)Math.Cos(Functions.DegreeToRadian(Model + RotateAngle));
            yield return new LiveLine(center, new PointF(x, y));
        }
    }
}
