using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using NewUtilities.RadarWorks.Elements.Signal;
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
        public bool AdaptToSignal { get; set; } = true;
        public void Lock() => isLocked = true;
        public void Unlock() => isLocked = false;

        public MultiSignalPlotter(IButtonLayout buttonLayout)
        {
            this.buttonLayout = buttonLayout;
        }

        public void UpdateSerie(string serieName, IEnumerable<PointF> points)
        {
            if (isLocked)   //锁定状态不更新数据
                return;
            if (!plotterMap.ContainsKey(serieName))
                throw new ArgumentException($"{serieName}不存在");
            plotterMap[serieName].Update(points.ToList());
            Redraw();
        }

        public void AddSerie(SeriesProperties properties)
        {
            string serieName = properties.Name;
            if (displayer == null)
                throw new InvalidOperationException($"必须先将Plotter添加到Displayer中才能调用此方法");
            if (plotterMap.ContainsKey(serieName))
                throw new ArgumentException($"{serieName}已经存在");
            var plotter = new SeriePlotter(properties);
            plotterMap.Add(serieName, plotter);
            var button = new PushDownButton(MakeButtonStyle(properties));
            buttonMap.Add(serieName, button);
            button.Clicked += Button_Clicked;

            displayer.Elements.Add(++currentLayerId, plotter);
            displayer.Elements.Add(1000, button);
            //plotter.AddMarker();
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
            lock(Locker)
            {
                buttonLayout.Reset();
                Area modelArea = null;
                foreach (var s in plotterMap.Keys)
                {
                    var plotter = plotterMap[s];
                    plotter.Redraw();
                    var btn = buttonMap[s];
                    var properties = btn.Model;
                    properties.Location = buttonLayout.NextLocation();
                    btn.Update(properties);

                    if(AdaptToSignal)
                    {
                        if (modelArea == null)
                            modelArea = plotter.ModelArea;
                        else
                        {
                            modelArea.Set(Math.Min(modelArea.Left, plotter.ModelArea.Left), Math.Max(modelArea.Right, plotter.ModelArea.Right), Math.Max(modelArea.Top, plotter.ModelArea.Top), Math.Min(modelArea.Bottom, plotter.ModelArea.Bottom));
                        }
                        if (modelArea == null)
                            continue;
                        if (ShouldSetArea(modelArea))
                        {
                            ReferenceSystem.SetArea(MakeNewArea(modelArea));
                        }
                    }
                }
            }

            Area MakeNewArea(Area modelArea)
            {
                var ret = new Area(modelArea.Left, modelArea.Right, modelArea.Top + modelArea.VerticalCover * 0.25, modelArea.Bottom - modelArea.VerticalCover * 0.25);
                return ret;
            }

            bool ShouldSetArea(Area modelArea)
            {
                if (modelArea.Right > ReferenceSystem.Right)
                    return true;
                if (modelArea.Left < ReferenceSystem.Left)
                    return true;
                if (modelArea.Top > ReferenceSystem.Top)
                    return true;
                if (modelArea.Bottom < ReferenceSystem.Bottom)
                    return true;
                var rsArea = new Area(ReferenceSystem.Left, ReferenceSystem.Right, ReferenceSystem.Top, ReferenceSystem.Bottom);
                if (modelArea.VerticalCover < rsArea.VerticalCover * 0.5)
                    return true;
                if (modelArea.HorizontalCover < rsArea.HorizontalCover * 0.9)
                    return true;
                return false;
            }
        }

        public SeriePlotter GetSerie(string Name)
        {
            return plotterMap[Name];
        }
    }
}
