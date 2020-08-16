using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.Coordinates;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks
{
    public class TargetModel
    {
        public uint Id { get; set; }
        public PolarCoordinate Location { get; set; }
        public float Speed { get; set; }
    }
    public class PpiTarget : RotatableElement<TargetModel>
    {
        private static readonly float triangleHeight = 10;
        private static readonly float triangleHorizentalOffset = 5;
        private static readonly float triangleVerticalOffset = 5;
        private static readonly float circleRadius = 5;
        private static readonly float tagWidth = 40;
        private static readonly float tagHeight = 20;
        private static readonly float tagRoundRadius = 3;

        private List<RectangularCoordinate> tail = new List<RectangularCoordinate>();
        public PpiTarget(TargetModel model, string rotateDecoratorName = "default") : base(rotateDecoratorName)
        {
            Model = model;
        }

        protected override void DrawDynamicElement(RenderTarget rt)
        {
            using (Brush tagBrush = Color.Chocolate.SolidBrush(rt))
            using (Brush tailBrush = Color.White.SolidBrush(rt))
            using (Brush tailSelected = Color.Orange.SolidBrush(rt))
            using (Brush targetBrush = Color.AliceBlue.SolidBrush(rt))
            using (Brush targetSelected = Color.Orange.SolidBrush(rt))
            using (Brush tagSelected = Color.Orange.SolidBrush(rt))
            {
                //tagBrush.Opacity = 0.8f;
                if (!Selected)
                    Objects[0].Fill(rt, targetBrush);   //circle
                else
                    Objects[0].Fill(rt, targetSelected);   //circle
                if (!Selected)
                    Objects[1].Fill(rt, tagBrush);      //triangle
                else
                    Objects[1].Fill(rt, tagSelected);      //triangle
                if (!Selected)
                    Objects[2].Fill(rt, tagBrush);      //roundedRectangle
                else
                    Objects[2].Fill(rt, tagSelected);      //roundedRectangle

                for (int i = 3; i < Objects.Count; i++)
                {
                    if (!Selected)
                        Objects[i].Fill(rt, tailBrush);
                    else
                        Objects[i].Fill(rt, tailSelected);
                }
                var dw = DWriteFactory.CreateFactory();
                var font = "宋体";
                var layout = dw.CreateTextLayout(Model.Id.ToString(), font.MakeFormat(12), tagWidth, tagHeight);
                layout.TextAlignment = TextAlignment.Center;
                var rect = ((Objects[2] as LiveRoundedRectangle).RoundedRect.Rect);
                rt.DrawTextLayout(new Point2F(rect.Left, rect.Top), layout, tailBrush);
                layout.Dispose();
            }
        }

        protected override IEnumerable<LiveObject> GetObjects()
        {
            var cooLoc = Model.Location.Rectangular;    //对Model进行NUll检测
            var scrLoc = Mapper.GetScreenLocation(cooLoc.X, cooLoc.Y);

            yield return new LiveCircle(new Ellipse(scrLoc.ToPoint2F(), circleRadius, circleRadius));
            var p1 = new Point2F(scrLoc.X, scrLoc.Y - triangleVerticalOffset);
            var p2 = new Point2F(scrLoc.X - triangleHorizentalOffset, scrLoc.Y - triangleVerticalOffset - triangleHeight);
            var p3 = new Point2F(scrLoc.X + triangleHorizentalOffset, scrLoc.Y - triangleVerticalOffset - triangleHeight);
            yield return new LiveLineGeometry(p1, p2, p3);

            RectF rect = new RectF
            {
                Left = scrLoc.X - tagWidth / 2,
                Top = scrLoc.Y - triangleVerticalOffset - triangleHeight - tagHeight,
                Width = tagWidth,
                Height = tagHeight
            };
            yield return new LiveRoundedRectangle(rect, tagRoundRadius, tagRoundRadius);

            PointF pStart, pEnd = scrLoc;
            for (int i = 0; i < tail.Count - 1; i++)
            {
                pStart = base.Mapper.GetScreenLocation(tail[i].X, tail[i].Y);
                pEnd = base.Mapper.GetScreenLocation(tail[i + 1].X, tail[i + 1].Y);
                yield return new LiveLine(pStart, pEnd);
                yield return new LiveCircle(new Ellipse(pStart.ToPoint2F(), 2, 2));
            }

            yield return new LiveLine(scrLoc, pEnd);
        }
        protected override void DoUpdate(TargetModel t)
        {
            tail.Add(Model.Location.Rectangular);
            base.DoUpdate(t);
        }
    }
}
