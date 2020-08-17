using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Utilities.Tools;

namespace Utilities.RadarWorks
{
    public class LiveSectorRing : LiveGeometry
    {
        public LiveSectorRing(PointF scrP1, PointF scrP2, PointF center)
        {
            ScrP1 = scrP1;
            ScrP2 = scrP2;
            Center = center;
        }

        public PointF ScrP1 { get; set; }
        public PointF ScrP2 { get; set; }
        public PointF Center { get; set; }

        //public override bool IsPointNear(PointF m)
        //{
        //    var p1Dis = Functions.DistanceBetween(ScrP1.ToPoint2F(), Center.ToPoint2F());
        //    var p2Dis = Functions.DistanceBetween(ScrP2.ToPoint2F(), Center.ToPoint2F());
        //    var angle1 = Functions.StandardAngle(90 - Functions.RadianToDegree(Math.Atan2(Center.Y - ScrP1.Y, ScrP1.X - Center
        //        .X)));
        //    var angle2 = Functions.StandardAngle(90 - Functions.RadianToDegree(Math.Atan2(Center.Y - ScrP2.Y, ScrP2.X - Center
        //        .X)));

        //    var mDis = Functions.DistanceBetween(m.ToPoint2F(), Center.ToPoint2F());
        //    var anglem = Functions.StandardAngle(90 - Functions.RadianToDegree(Math.Atan2(Center.Y - m.Y, m.X - Center
        //        .X)));

        //    var begin = Functions.FindSmallArcBeginAngle(angle1, angle2);
        //    var end = Functions.FindSmallArcEndAngle(angle1, angle2);
        //    if (begin > end)
        //    {
        //        return (anglem >= begin || anglem <= end) && mDis >= Math.Min(p1Dis, p2Dis) && mDis <= Math.Max(p1Dis, p2Dis);
        //    }
        //    else
        //        return anglem >= begin && anglem <= end && mDis >= Math.Min(p1Dis, p2Dis) && mDis <= Math.Max(p1Dis, p2Dis);
        //}

        protected override Geometry GetGeometry(RenderTarget rt)
        {
            return GetPathGeometry(rt, Center, ScrP1, ScrP2);
        }
        public static PathGeometry GetPathGeometry(RenderTarget t, PointF OriginalPoint, PointF p1, PointF p2)
        {
            Point2F innerLeft, outterLeft, outterRight, innerRight;
            PathGeometry waveGate = t.Factory.CreatePathGeometry();

            double mouseBeginAngle = Functions.AngleToNorth(OriginalPoint, p1);
            double mouseEndAngle = Functions.AngleToNorth(OriginalPoint, p2);

            double begin = Functions.FindSmallArcBeginAngle(mouseBeginAngle, mouseEndAngle);
            double end = Functions.FindSmallArcEndAngle(mouseBeginAngle, mouseEndAngle);

            double mouseBeginDis = (float)Functions.DistanceBetween(OriginalPoint.ToPoint2F(), p1.ToPoint2F());
            double mouseEndDis = (float)Functions.DistanceBetween(OriginalPoint.ToPoint2F(), p2.ToPoint2F());
            Point2F mouseBeginZoomed = RadiusWiseZoomPosition(p1, mouseEndDis, OriginalPoint);
            Point2F mouseDragZoomed = RadiusWiseZoomPosition(p2, mouseBeginDis, OriginalPoint);

            if (begin == mouseBeginAngle)    //扇形在鼠标点击一侧开始顺时针扫过
            {
                if (mouseBeginDis < mouseEndDis) //鼠标向外拖
                {
                    innerLeft = p1.ToPoint2F();
                    outterLeft = mouseBeginZoomed;
                    outterRight = p2.ToPoint2F();
                    innerRight = mouseDragZoomed;
                }
                else    //鼠标向内拖
                {
                    innerLeft = mouseBeginZoomed;
                    outterLeft = p1.ToPoint2F();
                    outterRight = mouseDragZoomed;
                    innerRight = p2.ToPoint2F();
                }
            }
            else   //扇形在鼠标拖动一侧开始顺时针扫过
            {
                if (mouseBeginDis < mouseEndDis) //鼠标向外拖
                {
                    innerLeft = mouseDragZoomed;
                    outterLeft = p2.ToPoint2F();
                    outterRight = mouseBeginZoomed;
                    innerRight = p1.ToPoint2F();
                }
                else    //鼠标向内拖
                {
                    innerLeft = p2.ToPoint2F();
                    outterLeft = mouseDragZoomed;
                    outterRight = p1.ToPoint2F();
                    innerRight = mouseBeginZoomed;
                }
            }

            GeometrySink gs = waveGate.Open();
            gs.BeginFigure(innerLeft, FigureBegin.Filled);
            gs.AddLine(outterLeft);

            double disMax = Math.Max(mouseBeginDis, mouseEndDis);
            double disMin = Math.Min(mouseBeginDis, mouseEndDis);

            Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF size = new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF((float)disMax, (float)disMax);
            ArcSegment arc = new ArcSegment(outterRight, size, 0, SweepDirection.Clockwise, ArcSize.Small);
            gs.AddArc(arc);

            gs.AddLine(innerRight);
            size = new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.SizeF((float)disMin, (float)disMin);
            arc = new ArcSegment(innerLeft, size, 0, SweepDirection.Counterclockwise, ArcSize.Small);
            gs.AddArc(arc);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
            gs.Dispose();

            return waveGate;
        }

        public static Point2F RadiusWiseZoomPosition(PointF p, double r, PointF o)
        {
            var ret = new Point2F();

            //计算拖拽位置和坐标原点连线的正北夹角
            var angle = Functions.AngleToNorth(o, p);
            angle = Functions.DegreeToRadian(angle);

            //计算起始角度对应直线与坐标系外圈圆周的交点坐标
            ret.X = (int)(o.X + r * Math.Sin(angle));
            ret.Y = (int)(o.Y - r * Math.Cos(angle));

            return ret;
        }   //极坐标
    }
}
