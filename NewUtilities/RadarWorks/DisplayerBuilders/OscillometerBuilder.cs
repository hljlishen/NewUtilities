using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities.Mapper;
using Utilities.RadarWorks;

namespace NewUtilities.RadarWorks.DisplayerBuilders
{
    public class OscillometerBuilder : DisplayerBuilder
    {
        public OscillometerBuilder(Control p, IScreenToCoordinateMapper mapper, ReferenceSystem referenceSystem) : base(p, mapper, referenceSystem)
        {
        }

        protected override SelectStrategy GetSelectStrategy()
        {
            return new RectangleSelection();
        }
    }
}
