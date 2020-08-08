using System.Collections.Generic;
using System.Drawing;

namespace Utilities.RadarWorks
{
    public class LiveObjectGroup : LiveObject
    {
        private List<LiveObject> children = new List<LiveObject>();
        public void Add(LiveObject ob) => children.Add(ob);
        public override bool IsPointNear(PointF mouseLocation)
        {
            foreach(var ob in children)
            {
                if (ob.IsPointNear(mouseLocation))
                    return true;
            }
            return false;
        }

        public override bool Selected
        {
            get => base.Selected;
            set
            {
                base.Selected = value;
                foreach (var o in children)
                    o.Selected = value;
            }
        }
    }
}
