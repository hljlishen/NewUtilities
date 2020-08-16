using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks
{
    public abstract class LiveObject : IDisposable
    {
        public PointF MouseLocation { get; set; }
        public object Value { get; set; }
        public abstract bool IsPointNear(PointF mouseLocation);
        public abstract void DrawFrame(RenderTarget rt, Brush frameBrush, float strokeWidth, StrokeStyle style);
        public abstract void DrawFrame(RenderTarget rt, Brush frameBrush, float strokeWidth);
        public abstract void Fill(RenderTarget rt, Brush fillBrush);
        ~LiveObject()
        {
            Dispose(false);
            GC.SuppressFinalize(this);
        }
        public void Dispose()
        {
            Dispose(true);
        }
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                //释放托管资源
            }

            //释放非托管资源
        }
    }
}
