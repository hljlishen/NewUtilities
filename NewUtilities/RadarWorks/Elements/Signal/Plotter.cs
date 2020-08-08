using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using Utilities.RadarWorks.Elements.Button;

namespace Utilities.RadarWorks.Elements.Signal
{
    public class Plotter : GraphicElement
    {
        Dictionary<string, SeriePlotter> plotterMap = new Dictionary<string, SeriePlotter>();
        Dictionary<string, ButtonElement> buttonMap = new Dictionary<string, ButtonElement>();
        public IButtonLayout buttonLayout { get; private set; }
        private int currentLayerId = -1;

        public Plotter(IButtonLayout buttonLayout)
        {
            this.buttonLayout = buttonLayout;
        }

        public void UpdateSerie(string serieName, IEnumerable<PointF> points)
        {
            if (!plotterMap.ContainsKey(serieName))
                throw new ArgumentException($"{serieName}不存在");
            plotterMap[serieName].Update(points.ToList());
        }

        public void AddSerie(SeriesProperties properties)
        {
            string serieName = properties.Name;
            if (currentLayerId < 0)   //currentLayerId = -1表示Plotter还没有加入到Displayer中
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
            buttonLayout.Reset();
            foreach (var s in plotterMap.Keys)
            {
                plotterMap[s].UpdateView();
                var btn = buttonMap[s];
                var properties = btn.Model;
                properties.Location = buttonLayout.NextLocation();
                btn.Update(properties);
            }
        }
    }
}
