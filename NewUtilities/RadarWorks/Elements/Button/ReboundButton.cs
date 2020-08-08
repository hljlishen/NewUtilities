using Utilities.RadarWorks.Sensors;

namespace Utilities.RadarWorks.Elements.Button
{
    public class ReboundButton : ButtonElement
    {
        public ReboundButton(ButtenProperties buttenProperties) : base(buttenProperties, new MouseClickSensor3())
        {
        }

        protected override void ProcessObjectStateChanged(Sensor obj)
        {
            if (Selected)
                InvokeClicked();
        }
    }
}
