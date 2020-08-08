using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.RadarWorks.Elements.Button
{
    public class PushDownButton : ButtonElement
    {
        public PushDownButton(ButtenProperties buttenProperties) : base(buttenProperties, new MouseClickSensor1())
        {
        }

        protected override void ProcessObjectStateChanged(Sensor obj) => InvokeClicked();
    }
}
