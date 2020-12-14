using System;
using System.Drawing;

namespace Utilities.RadarWorks.Elements.Signal
{
    class MaxiumMaintainSgnal : SeriePlotter
    {
        public MaxiumMaintainSgnal(SeriePlotter signal) : base(new SeriesProperties(signal.SeriesProperties.Name + "最大") {/*PlotStyle = PlotStyle.Discrete, */StrokeColor = Color.Gray, StrokeWidth = 1})
        {
            signal.Updated += Signal_Updated;
        }

        private void Signal_Updated(SeriePlotter obj)
        {
            //Console.WriteLine($"{obj.Model[0].X}, {obj.Model[0].Y}");
            if (Model == null)
            {
                Model = new Models.PointD[obj.Model.Length];
                obj.Model.CopyTo(Model, 0);
            }
            else
            {
                var count = Math.Min(obj.Model.Length, Model.Length);

                for (int i = 0; i < count; i++)
                {
                    if (Model[i].Y < obj.Model[i].Y)
                    {
                        Model[i].Y = obj.Model[i].Y;
                    }
                }
            }
            Redraw();
        }

        public override void Off()
        {
            base.Off();
            Model = null;
        }
    }
}
