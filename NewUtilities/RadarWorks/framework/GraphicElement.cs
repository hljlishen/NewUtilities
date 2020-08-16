using System;
using System.Windows.Forms;
using Utilities.Mapper;
using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Utilities.RadarWorks.Elements.Button;

namespace Utilities.RadarWorks
{
    public abstract class GraphicElement : IDisposable, IGraphic
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
        public Displayer ParentDispalyer { get; protected set; }

        /// <summary>
        /// 多线程方位该对象时的锁对象
        /// </summary>
        public object Locker { get; private set; } = new object();
        protected Sensor sensor;
        public Control Panel => ParentDispalyer.Panel;
        public Rectangle ScreenRect => new Rectangle((int)Mapper.ScreenLeft, (int)Mapper.ScreenTop, (int)Mapper.ScreenWidth, (int)Mapper.ScreenHeight);
        public virtual IScreenToCoordinateMapper Mapper => ParentDispalyer.Mapper;
        public ReferenceSystem ReferenceSystem => ParentDispalyer.ReferenceSystem;
        protected abstract void DrawElement(RenderTarget rt);
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
                if (ParentDispalyer != null)     //如果元素对象还没有加入Displayer，则先不调用
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
            ParentDispalyer = d;
            Sensor?.SetParentElement(this);
            RefreshObjects();
            Mapper.MapperStateChanged += Mapper_MapperStateChanged;
            ParentDispalyer.BeforeRebindTarget += Displayer_BeforeRebindTarget;
            ParentDispalyer.AfterRebindTarget += Displayer_AfterRebindTarget;
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
            DrawElement(rt);
            Changed = false;
        }

        /// <summary>
        /// Mapper状态发生变化时触发
        /// </summary>
        /// <param name="obj">Mapper引用</param>
        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj)
        {
            RefreshObjects();   //重新生成图形元素
            Redraw();           //mapper状态改变后需重绘视图
        }

        public virtual void Dispose()
        {
            Sensor?.Dispose();
            Mapper.MapperStateChanged -= Mapper_MapperStateChanged;
            ParentDispalyer.BeforeRebindTarget -= Displayer_BeforeRebindTarget;
            ParentDispalyer.AfterRebindTarget -= Displayer_AfterRebindTarget;
            UnbindEvents(ParentDispalyer.Panel);
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
    }
}
