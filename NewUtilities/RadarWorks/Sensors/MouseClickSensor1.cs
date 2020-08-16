using System.Windows.Forms;

namespace Utilities.RadarWorks
{
    public class MouseClickSensor1 : Sensor
    {
        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            lock (locker)
            {
                if (objects == null || objects.Count == 0)
                    return;

                foreach (var o in objects)
                {
                    if (o.IsPointNear(e.Location))
                    {
                        o.MouseLocation = e.Location;
                        ParentElement.Selected = !ParentElement.Selected;
                        InvokeObjectStateChanged();
                    }
                }
            }
        }

        protected override void BindEvents(Control panel) => panel.MouseClick += Panel_MouseClick;

        protected override void UnbindEvents(Control panel) => panel.MouseClick -= Panel_MouseClick;
    }
}
