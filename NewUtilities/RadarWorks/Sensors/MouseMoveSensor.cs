using System.Windows.Forms;

namespace Utilities.RadarWorks
{
    public class MouseMoveSensor : Sensor
    {
        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            lock(locker)
            {
                if (objects == null || objects.Count == 0)
                    return;

                foreach (var o in objects)
                {
                    if (o.IsPointNear(e.Location))
                    {
                        o.MouseLocation = e.Location;
                        if (!o.Selected)
                        {
                            o.Selected = true;
                            InvokeObjectStateChanged();
                        }
                    }
                    else
                    {
                        if (o.Selected)
                        {
                            o.Selected = false;
                            InvokeObjectStateChanged();
                        }
                    }
                }
            }
        }

        protected override void BindEvents(Control panel) => panel.MouseMove += Panel_MouseMove;

        protected override void UnbindEvents(Control panel) => panel.MouseMove -= Panel_MouseMove;
    }
}
