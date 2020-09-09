using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using NewUtilities.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utilities.Mapper;
using Utilities.RadarWorks.Elements.Button;

namespace Utilities.RadarWorks.Elements.Signal
{
    public class MultiSignalPlotter : GraphicElement
    {
        Dictionary<string, SeriePlotter> plotterMap = new Dictionary<string, SeriePlotter>();
        Dictionary<string, ButtonElement> buttonMap = new Dictionary<string, ButtonElement>();
        public IButtonLayout buttonLayout { get; private set; }
        private int currentLayerId = -1;
        private bool isLocked = false;
        public bool AdaptToSignal { get; set; } = false;
        public void Lock() => isLocked = true;
        public void Unlock() => isLocked = false;

        public MultiSignalPlotter(IButtonLayout buttonLayout)
        {
            this.buttonLayout = buttonLayout;
        }

        public void UpdateSerie(string serieName, IEnumerable<PointD> points)
        {
            if (isLocked)   //锁定状态不更新数据
                return;
            if (!plotterMap.ContainsKey(serieName))
                throw new ArgumentException($"{serieName}不存在");
            plotterMap[serieName].Update(points.ToList());

            if (AdaptToSignal)//此处可能有性能损耗，每次更新信号数据时都需要计算一次数据范围，应该是所有信号数据更新完毕同一更新一次
            {
                var modelArea = FindUnionArea(plotterMap.Values);

                if (ShouldSetArea(modelArea))
                {
                    Area area = MakeProperArea(modelArea);
                    ReferenceSystem.SetArea(area);
                }
            }
            Redraw();
        }

        /// <summary>
        /// 信号显示器视图适应指定的信号
        /// </summary>
        /// <param name="signalName">自适应信号的名称，如果使用默认值则取所有信号区域的并集作为显示器的区域</param>
        public void AdaptViewToSignal(string signalName = "")
        {
            Area area;
            if (signalName == "")
                area = FindUnionArea(plotterMap.Values);
            else
                area = plotterMap[signalName].ModelArea;

            ReferenceSystem.SetArea(MakeProperArea(area));
        }

        private Area FindUnionArea(IEnumerable<SeriePlotter> series)    
        {
            Area modelArea = null;
            foreach (var plotter in series)
            {
                if (modelArea == null)
                    modelArea = plotter.ModelArea;
                else
                {
                    if (plotter.ModelArea == null)
                        continue;
                    modelArea.Set(Math.Min(modelArea.Left, plotter.ModelArea.Left), Math.Max(modelArea.Right, plotter.ModelArea.Right), Math.Max(modelArea.Top, plotter.ModelArea.Top), Math.Min(modelArea.Bottom, plotter.ModelArea.Bottom));
                }
            }
            return modelArea;
        }

        public void AddSerie(SeriesProperties properties)
        {
            string serieName = properties.Name;
            if (ParentDisplayer == null)
                throw new InvalidOperationException($"必须先将Plotter添加到Displayer中才能调用此方法");
            if (plotterMap.ContainsKey(serieName))
                throw new ArgumentException($"{serieName}已经存在");
            var plotter = new SeriePlotter(properties);
            var button = new PushDownButton(MakeButtonStyle(properties));
            button.Clicked += Button_Clicked;
            lock (Locker)
            {
                plotterMap.Add(serieName, plotter);
                buttonMap.Add(serieName, button);
            }

            ParentDisplayer.Elements.Add(++currentLayerId, plotter);
            ParentDisplayer.Elements.Add(1000, button);
        }

        private ButtenProperties MakeButtonStyle(SeriesProperties properties)
        {
            string serieName = properties.Name;
            var ret = new ButtenProperties(buttonLayout.NextLocation(), buttonLayout.ButtonSize, serieName)
            {
                ForeColor = properties.StrokeColor,
                SelectedColor = Color.Gray,
                ForeFontColor = properties.StrokeColor.ReverseColor(),
                SelectedFontColor = Color.White,
                ForeFrameColor = Color.Black,
                SelectedFrameColor = properties.StrokeColor,
                FrameWidth = 4
            };

            return ret;
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            currentLayerId = LayerId;
            buttonLayout.Displayer = d;
            ReferenceSystem.Animation = false;  //Plotter会自动调整Rs范围，与动画效果冲突
        }

        private void Button_Clicked(ButtonElement obj)
        {
            var plotter = plotterMap[obj.Model.Text];
            if (plotter.IsOn)
                plotter.Off();
            else
                plotter.On();
        }

        protected override void DrawElement(RenderTarget rt)
        {
            lock (Locker)
            {
                buttonLayout.Reset();
                foreach (var s in plotterMap.Keys)
                {
                    var plotter = plotterMap[s];
                    plotter.Redraw();
                    var btn = buttonMap[s];
                    var properties = btn.Model;
                    properties.Location = buttonLayout.NextLocation();
                    btn.Update(properties);
                }
            }
        }

        Area MakeProperArea(Area modelArea)
        {
            var ret = new Area(modelArea.Left, modelArea.Right, modelArea.Top + modelArea.VerticalCover * 0.25, modelArea.Bottom - modelArea.VerticalCover * 0.25);
            return ret;
        }

        bool ShouldSetArea(Area modelArea)
        {
            if (modelArea.Left > ReferenceSystem.Right)
                return true;
            if (modelArea.Right < ReferenceSystem.Left)
                return true;
            var rsArea = new Area(ReferenceSystem.Left, ReferenceSystem.Right, ReferenceSystem.Top, ReferenceSystem.Bottom);
            if (modelArea.VerticalCover < rsArea.VerticalCover * 0.5 || rsArea.VerticalCover < modelArea.VerticalCover * 0.5)
                return true;
            if (modelArea.HorizontalCover < rsArea.HorizontalCover * 0.5 || rsArea.HorizontalCover < modelArea.HorizontalCover * 0.5)
                return true;

            return false;
        }

        public void AddMarker(string serieName, Color c, float x = 0, bool locked = false) => plotterMap[serieName].AddMarker(c, x, locked);

        public void ClearMarker(string serieName) => plotterMap[serieName].ClearMarker();
    }
}
