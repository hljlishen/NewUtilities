using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using Utilities.RadarWorks;

namespace NewUtilities.RadarWorks.Elements.Ppi.WaveGateController
{
    public class WaveGateManager : GraphicElement, ISwtichable
    {
        private WaveGateSelector gateSelector = new WaveGateSelector();
        private readonly List<WaveGateElement> waveGates = new List<WaveGateElement>();
        public event Action<WaveGateModel> WaveGateDeleted;

        public bool IsOn => gateSelector.IsOn;

        public string Name { get; set; } = "波门选择";

        public void Off() => gateSelector.Off();

        public void On() => gateSelector.On();

        protected override void DrawElement(RenderTarget rt)
        {
            //Do nothing
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            d.Elements.Add(LayerId, gateSelector);
            gateSelector.SelectionFinish += GateSelector_SelectionFinish;
        }

        public void DeleteSelection()
        {
            waveGates.Delete((w) => w.Selected, RemoveWaveGateElement);
        }

        private void RemoveWaveGateElement(WaveGateElement w)
        {
            ParentDisplayer.Elements.Remove(LayerId, w);
            WaveGateDeleted?.Invoke(w.Model);
            w.Dispose();
        }

        private void GateSelector_SelectionFinish(System.Drawing.PointF arg1, System.Drawing.PointF arg2)
        {
            WaveGateElement wg = new WaveGateElement() { Sensor = new MouseClickSensor1() };
            waveGates.Add(wg);
            ParentDisplayer.Elements.Add(LayerId, wg);
            wg.Update(arg1, arg2);
        }
    }
}
