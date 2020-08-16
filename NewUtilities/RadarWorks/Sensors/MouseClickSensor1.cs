using System.Windows.Forms;

namespace Utilities.RadarWorks
{
    /// <summary>
    /// 鼠标第一次点击目标选中，第二次点击目标取消选中
    /// </summary>
    public class MouseClickSensor1 : Sensor
    {
        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            if (AnyThingSelected(e.Location))
            {
                ParentElement.Selected = !ParentElement.Selected;
                InvokeObjectStateChanged();
            }
        }

        protected override void BindEvents(Control panel) => panel.MouseClick += Panel_MouseClick;

        protected override void UnbindEvents(Control panel) => panel.MouseClick -= Panel_MouseClick;
    }
}
