using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using Brush = Microsoft.WindowsAPICodePack.DirectX.Direct2D1.Brush;

namespace Utilities.RadarWorks
{
    public enum DrawMethod
    {
        Frame,
        Fill,
        FrameAndFill
    }
    public abstract class LiveObject : IDisposable
    {
        public DrawInfo DrawInfo { get; set; }
        public DrawMethod DrawMethod { get; set; }
        public PointF MouseLocation { get; set; }
        public object Value { get; set; }
        public virtual bool Selected { get; set; } = false;
        public abstract bool IsPointNear(PointF mouseLocation);

        public void Draw(RenderTarget rt)
        {
            if (Selected)
            {
                using(var frameBrush = DrawInfo.SelectedFrameColor.SolidBrush(rt))
                {
                    DrawFrame(rt, frameBrush, DrawInfo.SelectedFrameWidth, DrawInfo.SelectedFrameStyle);
                }
                if(DrawInfo.Fill)
                {
                    using(var fillBrush = DrawInfo.SelectedFillColor.SolidBrush(rt))
                    {
                        Fill(rt, fillBrush);
                    }
                }
            }
            else
            {
                using (var frameBrush = DrawInfo.NormalFrameColor.SolidBrush(rt))
                {
                    DrawFrame(rt, frameBrush, DrawInfo.NormalFrameWidth, DrawInfo.SelectedFrameStyle);
                }
                if (DrawInfo.Fill)
                {
                    using (var fillBrush = DrawInfo.NormalFillColor.SolidBrush(rt))
                    {
                        Fill(rt, fillBrush);
                    }
                }
            }
        }

        public virtual void DrawFrame(RenderTarget rt, Brush frameBrush, float strokeWidth, StrokeStyle style) { }
        public virtual void Fill(RenderTarget rt, Brush fillBrush) { }
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
