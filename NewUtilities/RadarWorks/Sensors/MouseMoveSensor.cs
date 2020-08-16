using System.Windows.Forms;

namespace Utilities.RadarWorks
{
    public class MouseMoveSensor : Sensor
    {
        private void Panel_MouseMove(object sender, MouseEventArgs e)
        {
            if (AnyThingSelected(e.Location))
            {
                if (!ParentElement.Selected)
                {
                    ParentElement.Selected = true;
                    InvokeObjectStateChanged();
                    return;
                }
            }
            else
            {
                if (ParentElement.Selected)
                {
                    ParentElement.Selected = false;
                    InvokeObjectStateChanged();
                    return;
                }
            }
        }

        protected override void BindEvents(Control panel) => panel.MouseMove += Panel_MouseMove;

        protected override void UnbindEvents(Control panel) => panel.MouseMove -= Panel_MouseMove;
    }
}
