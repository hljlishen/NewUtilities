﻿using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using Utilities.RadarWorks.framework;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.RadarWorks
{
    public abstract class GraphicElement : IGraphic
    {
        /// <summary>
        /// 目标所属的图层的Id
        /// </summary>
        public int LayerId { get; set; } = -1;

        /// <summary>
        /// 该对象是否已被修改，如果Changed为true，会导致该对象所属的图层重绘
        /// </summary>
        private bool Changed = true;

        /// <summary>
        /// 表示当前元素是否被选中
        /// </summary>
        public bool Selected { get; set; } = false;

        /// <summary>
        /// 该GraphicElement对象所属的Displayer对象
        /// </summary>
        public Displayer ParentDisplayer { get; protected set; }

        /// <summary>
        /// 多线程方位该对象时的锁对象
        /// </summary>
        public object Locker { get; private set; } = new object();
        protected Sensor sensor;
        public Control Panel => ParentDisplayer.Panel;
        public Rectangle ScreenRect => new Rectangle((int)Mapper.ScreenLeft, (int)Mapper.ScreenTop, (int)Mapper.ScreenWidth, (int)Mapper.ScreenHeight);
        public virtual IScreenToCoordinateMapper Mapper => ParentDisplayer.Mapper;
        public ReferenceSystem ReferenceSystem => ParentDisplayer.ReferenceSystem;
        protected virtual void InitializeComponents(RenderTarget rt) { }
        public virtual bool HasChanged() => Changed;
        public virtual void Redraw() => Changed = true;
        public List<LiveObject> Objects { get; protected set; } = new List<LiveObject>();
        protected bool Initialized { get; set; } = true;

        /// <summary>
        /// 如果元素需要相应鼠标，必须重写此函数。
        /// </summary>
        /// <returns>LiveObject相应鼠标操作的区域</returns>
        protected virtual IEnumerable<LiveObject> GetObjects() => null;
        public Sensor Sensor
        {
            get => sensor;
            set
            {
                sensor?.Dispose();
                sensor = Guards.Guard.NullCheckAssignment(value);
                if (ParentDisplayer != null)     //如果元素对象还没有加入Displayer，则先不调用
                    sensor.SetParentElement(this);
                sensor.ObjectStateChanged += Sensor_ObjectStateChanged;
            }
        }

        public object Tag { get; set; }

        protected virtual void Sensor_ObjectStateChanged(Sensor obj) => Redraw();

        /// <summary>
        /// 重新生成LiveObject对象集合
        /// 次函数的调用时机：
        /// 1.Mapper状态改变时，此时保持LiveObject的选中状态
        /// 2.DynamicElement的Update函数调用
        /// 3.SetDisplayer时调用
        /// </summary>
        protected virtual void RefreshObjects()
        {
            lock (Locker)
            {
                if (Objects != null)
                {
                    foreach (var ob in Objects)
                        ob?.Dispose();
                }
                Objects?.Clear();
                Objects = GetObjects()?.ToList();
            }
        }

        /// <summary>
        /// 框架负责调用次函数，用户不要主动调用
        /// </summary>
        /// <param name="d">显示器</param>
        public virtual void SetDisplayer(Displayer d)
        {
            ParentDisplayer = d;
            Sensor?.SetParentElement(this);
            ParentDisplayer.BeforeRebindTarget += Displayer_BeforeRebindTarget;
            ParentDisplayer.AfterRebindTarget += Displayer_AfterRebindTarget;
            BindEvents(d.Panel);
        }

        /// <summary>
        /// Displayer更换显示控件之前每个Displayer.Elements中的元素会收到通知，主要处理绑定p的事件
        /// </summary>
        private void Displayer_AfterRebindTarget(Control p)
        {
            BindEvents(p);      //更换显示控件之后需重新绑定事件
            Initialized = true; //初始化标志置为true，下次绘制图形之前调用InitializeComponents函数，初始化绘制需要用到的对象
            Redraw();       //重绘标志置为true，下次刷新图像时重绘该元素
        }

        /// <summary>
        /// Displayer更换显示控件之前每个Displayer.Elements中的元素会收到通知，主要处理解绑p中的事件
        /// </summary>
        /// <param name="p">更换显示控件之前的原始显示控件</param>
        private void Displayer_BeforeRebindTarget(Control p) => UnbindEvents(p);

        /// <summary>
        /// GraphElement子类可以通过该函数订阅关注的Control事件
        /// </summary>
        /// <param name="p">用于显示Displayer的控件</param>
        protected virtual void BindEvents(Control p) { }

        /// <summary>
        /// GraphElement析构或Display更换显示控件时会调用该函数解绑Control事件
        /// </summary>
        /// <param name="p">用于显示Displayer的控件</param>
        protected virtual void UnbindEvents(Control p) { }

        /// <summary>
        /// 绘制元素
        /// </summary>
        /// <param name="rt">绘制元素的渲染目标</param>
        public void Draw(RenderTarget rt)
        {
            if (Initialized)
            {
                InitializeComponents(rt);
                Initialized = false;
            }
            RefreshObjects();   //重新生成图形元素
            DrawElement(rt);
            Changed = false;
        }

        protected virtual void DrawObjects(RenderTarget rt, DrawToolSet toolSet)
        {
            lock(Locker)
            {
                foreach (var o in Objects)
                {
                    if (toolSet.FrameWidth > 0)
                    {
                        if (toolSet.FrameStrokeStyle != null)
                            o.DrawFrame(rt, toolSet.FrameBrush, toolSet.FrameWidth, toolSet.FrameStrokeStyle);
                        else
                            o.DrawFrame(rt, toolSet.FrameBrush, toolSet.FrameWidth);
                    }
                    if (toolSet.FillBrush != null)
                        o.Fill(rt, toolSet.FillBrush);
                }
            }
        }
        protected abstract void DrawElement(RenderTarget rt);

        public virtual void Dispose()
        {
            Sensor?.Dispose();
            ParentDisplayer.BeforeRebindTarget -= Displayer_BeforeRebindTarget;
            ParentDisplayer.AfterRebindTarget -= Displayer_AfterRebindTarget;
            UnbindEvents(ParentDisplayer.Panel);
        }

        protected bool IsPointNearAnyObject(Point mouseDownPoint)
        {
            lock (Locker)
            {
                foreach (var o in Objects)
                {
                    if (o.IsPointNear(mouseDownPoint))
                        return true;
                }
                return false;
            }
        }

        public Dictionary<string, DrawToolSet> GetToolSets()
        {
            return new Dictionary<string, DrawToolSet>();
        }
    }
}
