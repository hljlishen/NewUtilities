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
            bool shouldInvoke = false;
            lock (locker)
            {
                foreach (var o in objects)
                {
                    if (o.Selected)
                    {
                        o.Selected = false;
                        shouldInvoke = true;
                    }
                }
            }
            if (shouldInvoke)
                InvokeObjectStateChanged();
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            bool shouldInvoke = false;
            lock (locker)
            {
                foreach (var o in objects)
                {
                    if (o.IsPointNear(e.Location))
                    {
                        o.Selected = true;
                        shouldInvoke = true;
                    }
                }
            }
            if (shouldInvoke)
                InvokeObjectStateChanged();
        }

        protected override void UnbindEvents(Control panel)
        {
            panel.MouseDown -= Panel_MouseDown;
            panel.MouseUp -= Panel_MouseUp;
        }
    }
}
