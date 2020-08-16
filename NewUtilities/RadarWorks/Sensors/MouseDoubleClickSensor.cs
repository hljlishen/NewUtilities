using System.Windows.Forms;

namespace Utilities.RadarWorks
{
    public class MouseDoubleClickSensor : Sensor
    {
        protected override void BindEvents(Control panel)
        {
            panel.MouseDoubleClick += Panel_MouseDoubleClick;
        }

        private void Panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lock (locker)
            {
                if (objects == null || objects.Count == 0)
                    return;

                foreach (var o in objects)
                {
                    if (o.IsPointNear(e.Location))
                    {
                        MouseLocation = e.Location;
                        if (!ParentElement.Selected)
                        {
                            ParentElement.Selected = true;
                            InvokeObjectStateChanged();
                        }
                    }
                    else
                    {
                        if (ParentElement.Selected)
                        {
                            ParentElement.Selected = false;
                            InvokeObjectStateChanged();
                        }
                    }
                }
            }
        }

        protected override void UnbindEvents(Control panel)
        {
            panel.MouseDoubleClick -= Panel_MouseDoubleClick;
        }
    }
}
