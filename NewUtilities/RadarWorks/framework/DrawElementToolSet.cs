using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using Utilities.RadarWorks;

namespace Utilities.RadarWorks.framework
{
    public class DrawToolSet
    {
        public Brush FrameBrush { get; set; }
        public float FrameWidth { get; set; }
        public StrokeStyle FrameStrokeStyle { get; set; }
        public Brush FillBrush { get; set; }
    }
    public class FlyWeightToolSetCache<T> where T : IGraphic, new()
    {
        private static Dictionary<string, DrawToolSet> DrawDetails = new Dictionary<string, DrawToolSet>();
        private static bool Initialized = false;
        public static DrawToolSet GetToolSet(string toolSetName)
        {
            if (!Initialized)
                Initialize();
            return DrawDetails[toolSetName];
        }

        public static void Initialize()
        {
            T t = new T();
            DrawDetails = t.GetToolSets();
            Initialized = true;
            t.Dispose();
        }
    }
}
