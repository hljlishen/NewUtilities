//using System;
//using System.Windows.Forms;
//using System.Linq;
//using System.Text;
//using System.Diagnostics;
//using DX = SharpDX;
//using D2D = SharpDX.Direct2D1;
//using WIC = SharpDX.WIC;
//using DDW = SharpDX.DirectWrite;
//using DXGI = SharpDX.DXGI;
//using SharpDX;

//namespace Utilities.Display.framework
//{
//    class BitmapConverter
//    {
//        public D2D.Bitmap ConvertFromSystemBitmap(System.Drawing.Bitmap bmp)
//    {
//        System.Drawing.Bitmap desBitmap;//预定义要是使用的bitmap
//                                        //如果原始的图像像素格式不是32位带alpha通道
//                                        //需要转换为32位带alpha通道的格式
//                                        //否则无法和Direct2D的格式对应
//        if (bmp.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppPArgb)
//        {
//            desBitmap = new System.Drawing.Bitmap(bmp.Width, bmp.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
//            using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(desBitmap))
//            {
//                g.DrawImage(bmp, 0, 0);
//            }
//        }
//        else
//        {
//            desBitmap = bmp;
//        }


//        //直接内存copy会非常快
//        //如果使用循环逐点转换会非常慢
//        System.Drawing.Imaging.BitmapData bmpData = desBitmap.LockBits(
//                    new System.Drawing.Rectangle(0, 0, desBitmap.Width, desBitmap.Height),
//                    System.Drawing.Imaging.ImageLockMode.ReadOnly,
//                    desBitmap.PixelFormat
//                );
//        int numBytes = bmpData.Stride * desBitmap.Height;
//        byte[] byteData = new byte[numBytes];
//        IntPtr ptr = bmpData.Scan0;
//        System.Runtime.InteropServices.Marshal.Copy(ptr, byteData, 0, numBytes);
//        desBitmap.UnlockBits(bmpData);



//        D2D.BitmapProperties bp;
//        D2D.PixelFormat pixelFormat = new D2D.PixelFormat(DXGI.Format.B8G8R8A8_UNorm, D2D.AlphaMode.Premultiplied);

//        bp = new D2D.BitmapProperties(
//                  pixelFormat,
//                  desBitmap.HorizontalResolution,
//                  desBitmap.VerticalResolution
//                );
//        D2D.Bitmap tempBitmap = new D2D.Bitmap(_renderTarget, new Size2(desBitmap.Width, desBitmap.Height), bp);
//        tempBitmap.CopyFromMemory(byteData, bmpData.Stride);

//        return tempBitmap;
//    }
//}
//}
