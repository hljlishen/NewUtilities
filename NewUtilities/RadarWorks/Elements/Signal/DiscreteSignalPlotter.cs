﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks
{
    public class DiscreteSignalPlotter : DynamicElement<List<double>>
    {
        private Brush signalBrush;
        private double[] data;

        public override void Dispose()
        {
            base.Dispose();
            signalBrush?.Dispose();
        }

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);
            signalBrush = rt.CreateSolidColorBrush(Color.GreenYellow.ToColorF());
        }
        protected override void DoUpdate(List<double> t) => data = t.ToArray();

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            //var yBottom = (float)Mapper.GetScreenY(0);
            var yBottom = Mapper.GetScreenLocation(0, 0).Y;
            if (data == null)
                return;
            for (int i = 0; i < data.Length; i++)
            {
                //var x = (float)Mapper.GetScreenX(i);
                //var yTop = (float)Mapper.GetScreenY(data[i]);
                //rt.DrawLine(new Point2F(x, yTop), new Point2F(x, yBottom), signalBrush, 1);
                var p = Mapper.GetScreenLocation(i, data[i]);
                rt.DrawLine(p.ToPoint2F(), new Point2F((float)p.X, (float)yBottom), signalBrush, 1);
            }
        }
    }
}
