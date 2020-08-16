using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;

namespace Utilities.RadarWorks
{
    public class LiveLine : LiveObject
    {
        public PointF P1, P2;

        public LiveLine(PointF p1, PointF p2)
        {
            P1 = p1;
            P2 = p2;
        }

        public double VerticalDistancToPointF(PointF p)
        {
            if (P1.X == P2.X)
            {
                return Math.Abs(p.X - P1.X);
            }
            if(P1.Y == P2.Y)
            {
                return Math.Abs(p.Y - P1.Y);
            }
            double lineK = (P2.Y - P1.Y) / (P2.X - P1.X);
            double lineC = (P2.X * P1.Y - P1.X * P2.Y) / (P2.X - P1.X);
            return Math.Abs(lineK * p.X - p.Y + lineC) / (Math.Sqrt(lineK * lineK + 1));
        }

        public override bool IsPointNear(PointF p)
        {
            var dis = VerticalDistancToPointF(p);
            //Console.WriteLine($"p1:{P1.X}, {P1.Y}");
            //Console.WriteLine($"p2:{P2.X}, {P2.Y}");
            if (dis <= 8)
            {
                var foot = GetFootOfPerpendicular(p);
                if (foot.X >= Math.Min(P1.X, P2.X) && foot.X <= Math.Max(P1.X, P2.X) && foot.Y >= Math.Min(P1.Y, P2.Y) && foot.Y <= Math.Max(P1.Y, P2.Y))
                {
                    //Console.WriteLine($"Line:True");
                    return true;
                }
            }
            //Console.WriteLine("Line:False");
            return false;
        }

        public PointF GetFootOfPerpendicular(PointF p) => GetFootOfPerpendicular(p, P1, P2);

        /// <summary>
        /// 求线段外一点到线段的垂足
        /// </summary>
        /// <param name="pt">线段外一点</param>
        /// <param name="begin">线段端点1</param>
        /// <param name="end">线段端点2</param>
        /// <returns></returns>
        public static PointF GetFootOfPerpendicular(PointF pt, PointF begin, PointF end)
        {
            PointF retVal = new PointF();

            double dx = begin.X - end.X;
            double dy = begin.Y - end.Y;
            if (Math.Abs(dx) < 0.00000001 && Math.Abs(dy) < 0.00000001)
            {
                retVal = begin;
                return retVal;
            }

            double u = (pt.X - begin.X) * (begin.X - end.X) +
                (pt.Y - begin.Y) * (begin.Y - end.Y);
            u /= ((dx * dx) + (dy * dy));

            retVal.X = (float)(begin.X + u * dx);
            retVal.Y = (float)(begin.Y + u * dy);

            return retVal;
        }

        public override void DrawFrame(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush frameBrush, float strokeWidth, StrokeStyle style)
        {
            rt.DrawLine(P1.ToPoint2F(), P2.ToPoint2F(), frameBrush, strokeWidth, style);
        }

        public override void Fill(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush fillBrush)
        {
            rt.DrawLine(P1.ToPoint2F(), P2.ToPoint2F(), fillBrush, 1);
        }

        public override void DrawFrame(RenderTarget rt, Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush frameBrush, float strokeWidth)
        {
            rt.DrawLine(P1.ToPoint2F(), P2.ToPoint2F(), frameBrush, strokeWidth);
        }
    }
}
