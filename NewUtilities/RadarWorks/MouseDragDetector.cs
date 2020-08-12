using System;
using System.Drawing;
using System.Windows.Forms;

namespace Utilities.RadarWorks
{
    public class MouseDragDetector : IDisposable, ISwtichable
    {
        private bool mouseDown = false;
        private Point mouseDownPos;
        private Point mouseCurrentPos;
        private bool isOn = false;

        public event Action<Point, Point> MouseDrag;
        public event Action<Point> MouseUp;
        public event Action<Point> MouseDown;

        public event Action<Point, Point> MouseDragFinish;
        public MouseDragDetector(Control panel)
        {
            Panel = panel;
            Panel.MouseDown += Panel_MouseDown;
            Panel.MouseUp += Panel_MouseUp;
            //Panel.MouseMove += Panel_MouseMove;
            Panel.MouseDoubleClick += Panel_MouseDoubleClick;
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

        private void Panel_MouseUp(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
                return;
            mouseDown = false;
            Panel.MouseMove -= Panel_MouseMove;
            //if (!IsMouseMoved())
            //    return;
            MouseUp?.Invoke(e.Location);
            MouseDragFinish?.Invoke(mouseDownPos, mouseCurrentPos);
        }

        private bool IsMouseMoved()
        {
            return Math.Abs(mouseCurrentPos.X - mouseDownPos.X) > 5 && Math.Abs(mouseCurrentPos.Y - mouseDownPos.Y) > 5;
        }

        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mouseDown)
                return;
            mouseCurrentPos = e.Location;
            //if (!IsMouseMoved())
            //    return;
            MouseDrag?.Invoke(mouseDownPos, mouseCurrentPos);
        }

        protected virtual void Panel_MouseDown(object sender, MouseEventArgs e)
        {
            if (!IsOn || e.Button != MouseButtons.Left)
                return;
            mouseDown = true;
            mouseDownPos = e.Location;
            mouseCurrentPos = e.Location;
            Panel.MouseMove += Panel_MouseMove;
            MouseDown?.Invoke(e.Location);
        }

        public void Dispose()
        {
            Panel.MouseDown -= Panel_MouseDown;
            Panel.MouseUp -= Panel_MouseUp;
            Panel.MouseDoubleClick -= Panel_MouseDoubleClick;
            //Panel.MouseMove -= Panel_MouseMove;
        }

        public void On() => isOn = true;

        public void Off() => isOn = false;

        public Control Panel { get; private set; }

        public bool IsOn => isOn;

        public string Name { get; set; } = "";
    }
}
