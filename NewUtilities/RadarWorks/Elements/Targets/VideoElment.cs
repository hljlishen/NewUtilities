using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.RadarWorks
{
    public class VideoElment : RotatableElement<List<IVideo>>
    {
        public float echoRadius { get; set; } = 2;

        private Func<IVideo, double> VideoX = (v) => v.Location.X;
        private Func<IVideo, double> VideoY = (v) => v.Location.Y;

        public VideoElment(Func<IVideo, double> videoX, Func<IVideo, double> videoY)
        {
            VideoX = videoX;
            VideoY = videoY;
        }

        public VideoElment()
        {
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if (Model == null)
                return;
            foreach (var t in Model)       //绘制原始视频
            {
                var red = t.Am * 2.97;
                var blue = 255 - red;
                ColorF c = new ColorF((float)red / 255, 0, (float)blue / 255);
                using (var brush = rt.CreateSolidColorBrush(c))
                {
                    //var loc = t.Location.Rectangular;
                    //var scrLoc = Mapper.GetScreenLocation(loc.X, loc.Y);
                    var scrLoc = Mapper.GetScreenLocation(VideoX(t), VideoY(t));
                    Ellipse e = new Ellipse(scrLoc.ToPoint2F(), echoRadius, echoRadius);
                    rt.FillEllipse(e, brush);
                }
            }
        }
    }
}
