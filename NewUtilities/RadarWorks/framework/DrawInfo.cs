using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Drawing;

namespace Utilities.RadarWorks
{
    public class DrawInfo
    {
        public bool Fill { get; set; } = false;
        public Color NormalFillColor { get; set; }
        public Color SelectedFillColor { get; set; }
        public Color NormalFrameColor { get; set; }
        public Color SelectedFrameColor { get; set; }
        public StrokeStyle NormalFrameStyle { get; set; }
        public StrokeStyle SelectedFrameStyle { get; set; }
        public float NormalFrameWidth { get; set; } = 1;
        public float SelectedFrameWidth { get; set; } = 1;
    }
}
