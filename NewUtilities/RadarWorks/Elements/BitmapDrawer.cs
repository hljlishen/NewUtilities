using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using Bitmap = System.Drawing.Bitmap;

namespace Utilities.RadarWorks
{
    public class BitmapDrawer : RotatableElement<int>
    {
        private Bitmap Img { get; set; }
        bool imgChanged = false;
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            if (Img == null)
                return;
            try
            {
                if (!imgChanged)
                    return;
                Bitmap bmp32 = new Bitmap(Img.Width, Img.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
                Graphics.FromImage(bmp32).DrawImageUnscaled(Img, 0, 0);

                var byteData = GetBitmapBytes(bmp32, out var stride);

                var properties = new BitmapProperties(new Microsoft.WindowsAPICodePack.DirectX.Direct2D1.PixelFormat(Microsoft.WindowsAPICodePack.DirectX.Graphics.Format.R8G8B8A8UNorm, AlphaMode.Premultiplied), Img.HorizontalResolution, Img.VerticalResolution);

                var bmp = rt.CreateBitmap(new SizeU((uint)Img.Width, (uint)Img.Height), properties);
                bmp.CopyFromMemory(byteData, stride);
                if (bmp != null)
                    rt.DrawBitmap(bmp, 0.3f, BitmapInterpolationMode.NearestNeighbor, new RectF(0, 0, Panel.Width, Panel.Bottom));
                bmp?.Dispose();
                imgChanged = false;
            }
            catch
            {

            }
        }

        private byte[] GetBitmapBytes(Bitmap Img, out uint stride)
        {
            var bmpData = Img.LockBits(new Rectangle(0, 0, Img.Width, Img.Height), ImageLockMode.ReadOnly, Img.PixelFormat);
            int numBytes = bmpData.Stride * Img.Height;
            var byteData = new byte[numBytes];
            IntPtr ptr = bmpData.Scan0;
            System.Runtime.InteropServices.Marshal.Copy(ptr, byteData, 0, numBytes);
            Img.UnlockBits(bmpData);
            stride = (uint)bmpData.Stride;
            return byteData;
        }

        public void SetBitmap(Bitmap desBitmap)
        {
            lock (Locker)
            {
                Img = desBitmap;
                imgChanged = true;
                UpdateView();
            }
        }
    }
}
