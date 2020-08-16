using System.Windows.Forms;

namespace Utilities.RadarWorks
{
    /// <summary>
    /// 鼠标第一次点击目标选中，第二次点击目标以外区域取消选中
    /// </summary>
    public class MouseClickSensor2 : Sensor
    {
        private void Panel_MouseClick(object sender, MouseEventArgs e)
        {
            if(AnyThingSelected(e.Location))
            {
                if(!ParentElement.Selected)
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

        protected override void BindEvents(Control panel) => panel.MouseClick += Panel_MouseClick;

        protected override void UnbindEvents(Control panel) => panel.MouseClick -= Panel_MouseClick;
    }
}
