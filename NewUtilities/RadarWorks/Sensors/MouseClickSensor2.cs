﻿using System.Windows.Forms;

namespace Utilities.RadarWorks
{
    public class MouseClickSensor2 : Sensor
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

        protected override void BindEvents(Control panel) => panel.MouseClick += Panel_MouseClick;

        protected override void UnbindEvents(Control panel) => panel.MouseClick -= Panel_MouseClick;
    }
}
