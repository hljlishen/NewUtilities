using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks.Elements
{
    public class TrackElement : RotatableElement<IEnumerable<ITrack>>
    {
        private Brush tagBrush;
        private Brush tailBrush;
        private Brush targetBrush;

        protected override void InitializeComponents(RenderTarget rt)
        {
            base.InitializeComponents(rt);

            tagBrush = Color.Chocolate.SolidBrush(rt);
            tailBrush = Color.White.SolidBrush(rt);
            targetBrush = Color.AliceBlue.SolidBrush(rt);
            Model = new List<ITrack>();
        }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            foreach (var t in Model)
            {
                DrawTrack(rt, t);
            }
        }

        private void DrawTrack(RenderTarget rt, ITrack t)
        {
            float triangleHeight = 10;
            float triangleVerticalOffset = 5;
            float circleRadius = 5;
            float tagWidth = 40;
            float tagHeight = 20;
            float tagRoundRadius = 3;

            var cooLoc = t.Location.Rectangular;    //对Model进行NUll检测
            var scrLoc = Mapper.GetScreenLocation(cooLoc.X, cooLoc.Y);

            var ellipse = new Ellipse(scrLoc.ToPoint2F(), circleRadius, circleRadius);
            rt.FillEllipse(ellipse, targetBrush);

            RectF r = new RectF
            {
                Left = scrLoc.X - tagWidth / 2,
                Top = scrLoc.Y - triangleVerticalOffset - triangleHeight - tagHeight,
                Width = tagWidth,
                Height = tagHeight
            };
            var tag = new RoundedRect(r, tagRoundRadius, tagRoundRadius);
            rt.FillRoundedRectangle(tag, tagBrush);
            var dw = DWriteFactory.CreateFactory();
            var font = "宋体";
            var layout = dw.CreateTextLayout(t.Id.ToString(), font.MakeFormat(12), tagWidth, tagHeight);
            layout.TextAlignment = TextAlignment.Center;
            rt.DrawTextLayout(new Point2F(r.Left, r.Top), layout, tailBrush);
            layout.Dispose();
        }
    }
}
