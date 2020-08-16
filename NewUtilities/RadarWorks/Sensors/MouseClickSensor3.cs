using System.Windows.Forms;

namespace Utilities.RadarWorks.Sensors
{
    /// <summary>
    /// 鼠标按下选中，鼠标抬起取消选中
    /// </summary>
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
            if(AnyThingSelected(e.Location))
            {
                ParentElement.Selected = true;
                InvokeObjectStateChanged();
            }
        }

        protected override void UnbindEvents(Control panel)
        {
            panel.MouseDown -= Panel_MouseDown;
            panel.MouseUp -= Panel_MouseUp;
        }
    }
}
