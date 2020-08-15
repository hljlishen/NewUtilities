using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Utilities.RadarWorks.Elements.Button;

namespace Utilities.RadarWorks
{
    public class ExclusiveSwitchableManager : GraphicElement
    {
        private Dictionary<ButtonElement, ISwtichable> buttonMap = new Dictionary<ButtonElement, ISwtichable>();
        private IButtonLayout buttonLayout;

        public ExclusiveSwitchableManager(IButtonLayout buttonLayout)
        {
            this.buttonLayout = buttonLayout;
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            buttonLayout.Displayer = d;
        }

        public void Add(ISwtichable s)
        {
            if (LayerId == -1)
                throw new InvalidOperationException("必须先将ExclusiveSwitchableManager添加到Displayer中才能向其中添加元素");
            lock(Locker)
            {
                if (buttonMap.Values.Contains(s))
                    return;
                var properties = MakeButtenProperties(s.Name);
                var btn = new PushDownButton(properties);
                btn.Update(properties);
                btn.Clicked += Btn_Clicked;
                buttonMap.Add(btn, s);
                Redraw();
                displayer.Elements.Add(LayerId, btn);
                displayer.Elements.Add(LayerId, s as GraphicElement);
            }
        }

        public ButtenProperties MakeButtenProperties(string text)
        {
            return new ButtenProperties(buttonLayout.NextLocation(), buttonLayout.ButtonSize, text) { ForeColor = Color.FromArgb(178, 200, 187), SelectedColor = Color.FromArgb(160, 220, 224), ForeFrameColor = Color.Black };
        }

        private void Btn_Clicked(ButtonElement obj)
        {
            lock(Locker)
            {
                var switchable = buttonMap[obj];
                if (switchable.IsOn)
                    switchable.Off();
                else
                {
                    foreach (var btn in buttonMap.Keys)     //确保只有一个控件能工作
                    {
                        if (btn == obj)
                        {
                            buttonMap[btn].On();
                        }
                        else
                        {
                            buttonMap[btn].Off();
                            btn.Selected = false;
                        }
                    }
                }
            }
        }

        protected override void DrawElement(RenderTarget rt)
        {
            buttonLayout.Reset();
            foreach (var b in buttonMap.Keys)
            {
                var selected = b.Selected;
                var p = b.Model;
                p.Location = buttonLayout.NextLocation();
                b.Update(p);
                b.Selected = selected;
            }
        }
    }
}
