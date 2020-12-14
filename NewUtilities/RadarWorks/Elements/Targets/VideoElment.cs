using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.RadarWorks
{
    public class VideoElment : RotatableElement<IEnumerable<IVideo>>
    {
        public float echoRadius { get; set; } = 2;

        private Func<IVideo, double> VideoX = (v) => v.Location.X;
        private Func<IVideo, double> VideoY = (v) => v.Location.Y;

        public bool FixedColor { get; set; } = true;
        public Color Color { get; set; } = Color.Red;

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

            if(!FixedColor)
            {
                foreach (var t in Model)       //绘制原始视频
                {
                    var red = t.Am * 2.97;
                    var blue = 255 - red;
                    ColorF c = new ColorF((float)red / 255, 0, (float)blue / 255);
                    using (var brush = rt.CreateSolidColorBrush(c))
                    {
                        var x = VideoX(t);
                        var y = VideoY(t);
                        var scrLoc = Mapper.GetScreenLocation(x, y);
                        Ellipse e = new Ellipse(scrLoc.ToPoint2F(), echoRadius, echoRadius);
                        rt.FillEllipse(e, brush);
                    }
                }
            }
            else
            {
                using (var brush = Color.SolidBrush(rt))
                {
                    foreach(var t in Model)
                    {
                        var x = VideoX(t);
                        var y = VideoY(t);
                        var scrLoc = Mapper.GetScreenLocation(x, y);
                        Ellipse e = new Ellipse(scrLoc.ToPoint2F(), echoRadius, echoRadius);
                        rt.FillEllipse(e, brush);
                    }
                }
            }
        }
    }
}
