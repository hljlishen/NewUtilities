﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.RadarWorks
{
    public class Displayer : IDisposable
    {
        public int UpdateInterval { get; set; } = 10;
        public bool Redraw { get; set; }

        public bool SetClip { get; set; } = true;
        public Control Panel { get; private set; }
        /// <summary>
        /// DIsplay更换显示控件之前触发，Control对象为更换之前的控件
        /// </summary>
        public event Action<Control> BeforeRebindTarget;
        /// <summary>
        /// DIsplay更换显示控件之后触发，Control对象为更换之后的控件
        /// </summary>
        public event Action<Control> AfterRebindTarget;

        public IScreenToCoordinateMapper Mapper { get; }
        private RenderTarget rt;
        public D2DFactory Factory { get; protected set; }

        public ReferenceSystem ReferenceSystem;

        public LayerManager Elements { get; protected set; }
        public readonly object Locker = new object();

        private CancellationTokenSource tokenSource;
        private CancellationToken token;
        private bool Disposed = false;

        public ColorF BackgroundColor { get; private set; }

        private Task updateTask;

        public Displayer(Control p, IScreenToCoordinateMapper mapper, ReferenceSystem referenceSystem) : this(p, mapper, referenceSystem, Color.Black)
        {

        }

        public Displayer(Control p, IScreenToCoordinateMapper mapper, ReferenceSystem referenceSystem, Color backgroundColor)
        {
            Panel = p;
            Panel.SizeChanged += Pb_SizeChanged;

            Mapper = mapper;
            referenceSystem.SetMapper(mapper);
            ReferenceSystem = referenceSystem;
            mapper.SetScreenArea(0, p.Size.Width, 0, p.Size.Height);
            mapper.MapperStateChanged += Mapper_MapperStateChanged;

            Elements = new LayerManager();
            Elements.SetDisplayer(this);
            BackgroundColor = backgroundColor.ToColorF();

            InitializeDisplayerState();
        }

        public void Start()
        {
            tokenSource?.Dispose();
            tokenSource = new CancellationTokenSource();
            token = tokenSource.Token;
            Redraw = true;
            updateTask = Task.Run(Draw);
        }

        private void InitializeDisplayerState()
        {
            var rtps = new RenderTargetProperties
            {
                PixelFormat = new PixelFormat(Microsoft.WindowsAPICodePack.DirectX.Graphics.Format.R8G8B8A8UNorm, AlphaMode.Premultiplied)
                
            };
            rtps.RenderTargetType = RenderTargetType.Hardware;
            var hrtp = new HwndRenderTargetProperties(Panel.Handle, new SizeU((uint)Panel.Width, (uint)Panel.Height), PresentOptions.Immediately);

            Factory = D2DFactory.CreateFactory(D2DFactoryType.SingleThreaded);   //创建工厂
            rt = Factory.CreateHwndRenderTarget(rtps, hrtp);
            //rt.Dpi = new DpiF(Panel.DeviceDpi, Panel.DeviceDpi);
            rt.AntiAliasMode = AntiAliasMode.Aliased;
            rt.TextAntiAliasMode = TextAntiAliasMode.Aliased;
            (rt as HwndRenderTarget).Resize(new SizeU((uint)Panel.Width, (uint)Panel.Height));
            //rt.Transform = Matrix3x2F.Scale(rt.Size.Width / Panel.Width, rt.Size.Height / Panel.Height);
        }
        private void Draw()
        {
            while (true)
            {
                if (token.IsCancellationRequested)
                {
                    return;
                }
                lock (Locker)
                {
                    if (Disposed)
                        return;

                    rt.BeginDraw();
                    rt.Clear(BackgroundColor);
                    if(SetClip)
                        rt.PushAxisAlignedClip(new RectF((float)Mapper.ScreenLeft - 5, (float)Mapper.ScreenTop-5, (float)Mapper.ScreenRight + 5, (float)Mapper.ScreenBottom + 5), AntiAliasMode.Aliased);

                    if (Redraw)
                    {
                        Elements.Draw(rt);
                        Redraw = false;
                    }
                    else
                    {
                        Elements.DrawChangedLayers(rt);
                    }
                    if(SetClip)
                        rt.PopAxisAlignedClip();
                    rt.EndDraw();
                }
                Thread.Sleep(UpdateInterval);
            }
        }

        public void Stop()
        {
            tokenSource.Cancel();
            Thread.Sleep(10);
            //Color c = Panel.BackColor;
            //Panel.BackColor = Color.Black;   //需要先置为黑色，给backcolor赋值本来的颜色不会产生任何操作
            //Panel.BackColor = c;  //backcolor置为本来的颜色
        }

        public void SetPanel(Control p)
        {
            Stop();
            ChangePanel(p);
            DisposeRenderTarget();
            InitializeDisplayerState();
        }

        private void ChangePanel(Control p)
        {
            Panel.SizeChanged -= Pb_SizeChanged; //接触消息绑定
            BeforeRebindTarget?.Invoke(Panel);
            Panel = p;
            Panel.SizeChanged += Pb_SizeChanged;//消息重新绑定
            Mapper.SetScreenArea(0, p.Size.Width, 0, p.Size.Height);
            AfterRebindTarget?.Invoke(p);
        }

        private void DisposeRenderTarget()
        {
            Stop();
            Task.WaitAll(updateTask);
            Factory.Dispose();
            rt.Dispose();
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj) => Redraw = true;

        private void Pb_SizeChanged(object sender, EventArgs e)
        {
            lock (Locker)
            {
                if (Panel.Width < 10 && Panel.Height < 10)  //卫语句，窗口最小化时会触发sizechanged事件，此时width和height都是0，会触发ValueInterval的范围过小异常
                    return;
                Mapper.SetScreenArea(0, Panel.Width, 0, Panel.Height);
                (rt as HwndRenderTarget).Resize(new SizeU((uint)Panel.Width, (uint)Panel.Height));
                //rt.Transform = Matrix3x2F.Scale(rt.Size.Width / Panel.Width, rt.Size.Height / Panel.Height);
                Redraw = true;
            }
        }

        public void Dispose()
        {
            DisposeRenderTarget();
            Elements.Dispose();
            Disposed = true;
        }
    }
}
