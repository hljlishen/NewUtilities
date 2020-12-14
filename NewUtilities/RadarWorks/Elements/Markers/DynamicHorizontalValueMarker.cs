using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.RadarWorks.Elements.Markers
{
    public class DynamicHorizontalValueMarker : HorizontalValueMarker
    {
        public override double MinValue => ReferenceSystem.Bottom;

        public override double MaxValue => ReferenceSystem.Top;
    }
}
