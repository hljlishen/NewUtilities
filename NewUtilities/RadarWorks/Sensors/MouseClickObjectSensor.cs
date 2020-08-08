using System;
using System.Drawing;
using System.Windows.Forms;

namespace Utilities.RadarWorks.Sensors
{
    public class MouseClickObjectSensor : Sensor
    {
        private bool mouseDown = false;
        private Point mouseDownPos;
        private Point mouseCurrentPos;

        public event Action<Point, Point> MouseDrag;
        protected override void BindEvents(Control panel)
        {
            panel.MouseDown += Panel_MouseDown;
            panel.MouseDoubleClick += Panel_MouseDoubleClick;
        }

        private void Panel_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            mouseDown = false;
            try
            {
                Panel.MouseMove -= Panel_MouseMove;
            }
            catch
            {

            }
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            mouseCurrentPos = e.Location;
            if (!IsMouseMoved())
                return;
            MouseDrag?.Invoke(mouseDownPos, mouseCurrentPos);
        }

        private bool IsMouseMoved()
        {
            return Math.Abs(mouseCurrentPos.X - mouseDownPos.X) > 5 && Math.Abs(mouseCurrentPos.Y - mouseDownPos.Y) > 5;
        }

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
                return;
            mouseDown = false;
            Panel.MouseMove -= Panel_MouseMove;
            Panel.MouseUp -= Panel_MouseUp;
        }

        private void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || !IsPointNearAnyObject(e.Location))
                return;
            mouseDown = true;
            mouseDownPos = e.Location;
            mouseCurrentPos = e.Location;
            Panel.MouseMove += Panel_MouseMove;
            Panel.MouseUp += Panel_MouseUp;
        }

        private bool IsPointNearAnyObject(Point mouseDownPoint)
        {
            foreach (var o in objects)
            {
                if (o.IsPointNear(mouseDownPoint))
                    return true;
            }
            return false;
        }

        protected override void UnbindEvents(Control panel)
        {
            panel.MouseDown -= Panel_MouseDown;
            panel.MouseUp -= Panel_MouseUp;
            panel.MouseDoubleClick -= Panel_MouseDoubleClick;
        }
    }
}
