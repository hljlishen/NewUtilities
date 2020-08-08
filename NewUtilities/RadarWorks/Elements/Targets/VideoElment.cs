using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Drawing;

namespace Utilities.RadarWorks
{
    public class VideoElment : RotatableElement<List<IVideo>>
    {
        public float echoRadius { get; set; } = 2;

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
                    var loc = t.Location.Rectangular;
                    var scrLoc = Mapper.GetScreenLocation(loc.X, loc.Y);
                    Ellipse e = new Ellipse(scrLoc.ToPoint2F(), echoRadius, echoRadius);
                    rt.FillEllipse(e, brush);
                }
            }
        }
    }
}
