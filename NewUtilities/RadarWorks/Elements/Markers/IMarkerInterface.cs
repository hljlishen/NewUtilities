using System.Drawing;
using Utilities.RadarWorks;

namespace Utilities.RadarWorks.Elements.Markers
{
    public interface IMarkerInterface : IDynamicElement<double>
    {
        Color Color { get; set; }
        double MaxValue { get; }
        double MinValue { get; }
        float Opacity { get; set; }
        Color SelectColor { get; set; }
        Color TextColor { get; set; }
        string TextFont { get; set; }
        float TextSize { get; set; }
        string Unit { get; set; }
        string ValueFormat { get; set; }
    }
}