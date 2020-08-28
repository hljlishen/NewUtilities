using System.Drawing;

namespace Utilities.RadarWorks.Elements.Signal
{
    public enum PlotStyle
    {
        Discrete,
        Analog,
        Dot
    }
    public class SeriesProperties
    {
        public SeriesProperties(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public float StrokeWidth { get; set; } = 1;
        public Color StrokeColor { get; set; } = Color.Blue;
        public PlotStyle PlotStyle { get; set; } = PlotStyle.Analog;
    }
}
