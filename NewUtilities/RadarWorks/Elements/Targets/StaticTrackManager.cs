using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Microsoft.WindowsAPICodePack.DirectX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks.Elements
{
    public class StaticTrackManager : RotatableElement<IEnumerable<ITrack>>
    {
        private Brush tagBrush;
        private Brush tailBrush;
        private Brush targetBrush;

        private Func<ITrack, double> TrackX;
        private Func<ITrack, double> TrackY;

        public void RemoveTrack(int id)
        {
            lock(Locker)
            {
                Model = Model.SkipWhile((t) => t.Id == id);
            }
        }

        protected override void DoUpdate(IEnumerable<ITrack> tracks)
        {
            if (tracks == null)
                return;

            //var excepts = Model.Except(tracks, new ITrackEqualComparer());
            //取与tracks的Id交集
            Model = Model.Intersect(tracks, new ITrackEqualComparer()).ToList();
            foreach (var newTrack in tracks)
            {
                bool isNewTrack = true;
                foreach(var oldTrack in Model)
                {
                    if(newTrack.Id == oldTrack.Id)  //当前Track的id已经存在
                    {
                        isNewTrack = false;
                        oldTrack.UpdateTrack(newTrack);
                    }
                }

                if(isNewTrack)  //原来的Model中不存在当前Track的id
                {
                    Model = Model.Append(newTrack);
                }
            }

            Redraw();
        }

        public StaticTrackManager(Func<ITrack, double> trackX, Func<ITrack, double> trackY)
        {
            TrackX = trackX;
            TrackY = trackY;
        }

        public StaticTrackManager() : this((t) => t.Location.X, (t) => t.Location.Y)
        {
        }

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

            var scrLoc = Mapper.GetScreenLocation(TrackX(t), TrackY(t));

            var ellipse = new Ellipse(scrLoc.ToPoint2F(), circleRadius, circleRadius);
            rt.FillEllipse(ellipse, targetBrush);

            RectF r = new RectF
            {
                Left = (float)(scrLoc.X - tagWidth / 2),
                Top = (float)(scrLoc.Y - triangleVerticalOffset - triangleHeight - tagHeight),
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
            dw.Dispose();
            layout.Dispose();
        }
    }

    class ITrackEqualComparer : IEqualityComparer<ITrack>
    {
        public bool Equals(ITrack x, ITrack y) => x.Id == y.Id;

        public int GetHashCode(ITrack obj) => obj.GetHashCode();
    }
}
