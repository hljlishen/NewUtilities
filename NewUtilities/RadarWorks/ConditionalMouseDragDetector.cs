using System;
using System.Drawing;
using System.Windows.Forms;
using Utilities.RadarWorks;

namespace Utilities.RadarWorks
{
    public class ConditionalMouseDragDetector : MouseDragDetector
    {
        private Func<Point, bool> condition = null;
        public ConditionalMouseDragDetector(Control panel, Func<Point, bool> condition) : base(panel)
        {
            this.condition = condition;
        }

        protected override void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (condition == null || !condition.Invoke(e.Location))
                return;
            base.Panel_MouseDown(sender, e);
        }
    }
}
