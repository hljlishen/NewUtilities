using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.RadarWorks.Elements.Markers
{
    public class StaticVerticalValueMarker : VerticalValueMarker
    {
        public override double MinValue => Mapper.CoordinateLeft;

        public override double MaxValue => Mapper.CoordinateRight;
    }
}
