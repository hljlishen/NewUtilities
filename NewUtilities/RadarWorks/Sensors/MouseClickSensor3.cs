using System.Windows.Forms;

namespace Utilities.RadarWorks.Sensors
{
    public class MouseClickSensor3 : Sensor
    {
        protected override void BindEvents(Control panel)
        {
            panel.MouseDown += Panel_MouseDown;
            panel.MouseUp += Panel_MouseUp;
        }

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if(ParentElement.Selected)
            {
                ParentElement.Selected = false;
                InvokeObjectStateChanged();
            }
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            lock (locker)
            {
                foreach (var o in objects)
                {
                    if (o.IsPointNear(e.Location))
                    {
                        o.MouseLocation = e.Location;
                        ParentElement.Selected = true;
                        InvokeObjectStateChanged();
                        break;
                    }
                }
            }
        }

        protected override void UnbindEvents(Control panel)
        {
            panel.MouseDown -= Panel_MouseDown;
            panel.MouseUp -= Panel_MouseUp;
        }
    }
}
