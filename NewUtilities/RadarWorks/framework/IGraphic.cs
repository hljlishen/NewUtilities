using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;

namespace Utilities.RadarWorks
{
    public interface IGraphic
    {
        int LayerId { get; set; }
        object Locker { get; }
        IScreenToCoordinateMapper Mapper { get; }
        List<LiveObject> Objects { get; }
        Control Panel { get; }
        Displayer ParentDispalyer { get; }
        ReferenceSystem ReferenceSystem { get; }
        Rectangle ScreenRect { get; }
        bool Selected { get; set; }
        Sensor Sensor { get; set; }

        void Dispose();
        void Draw(RenderTarget rt);
        bool HasChanged();
        void Redraw();
        void SetDisplayer(Displayer d);
    }
}