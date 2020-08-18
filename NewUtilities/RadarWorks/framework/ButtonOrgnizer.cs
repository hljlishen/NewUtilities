using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Drawing;
using Utilities.RadarWorks.Elements.Button;

namespace Utilities.RadarWorks.framework
{
    public class ButtonOrgnizer : GraphicElement
    {
        private IButtonLayout layout;
        private List<ReboundButton> buttons = new List<ReboundButton>();

        public void AddButton(string text, Action<ButtonElement> clickAction)
        {
            var btn = new ReboundButton(MakeButtenProperties(text));
            btn.Clicked += clickAction;

            ParentDisplayer.Elements.Add(LayerId, btn);
            buttons.Add(btn);
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            layout.Displayer = d;
        }

        public ButtenProperties MakeButtenProperties(string text)
        {
            var p = new ButtenProperties(layout.NextLocation(), layout.ButtonSize, text) { SelectedColor = Color.Gray, ForeColor = Color.Silver, ForeFrameColor = Color.Black, SelectedFrameColor = Color.Black };
            return p;
        }

        public ButtonOrgnizer(IButtonLayout layout)
        {
            this.layout = layout;
        }

        protected override void DrawElement(RenderTarget rt)
        {
            layout.Reset();
            foreach (var b in buttons)
            {
                b.Update(MakeButtenProperties(b.Model.Text));
            }
        }
    }
}
