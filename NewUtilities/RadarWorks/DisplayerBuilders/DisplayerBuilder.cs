using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;
using Utilities.RadarWorks;
using Utilities.RadarWorks.framework;
using Utilities.RadarWorks.framework.ButtonLayout;

namespace NewUtilities.RadarWorks.DisplayerBuilders
{
    public abstract class DisplayerBuilder
    {

        public DisplayerBuilder(Control p, IScreenToCoordinateMapper mapper, ReferenceSystem referenceSystem)
        {
            displayer = new Displayer(p, mapper, referenceSystem);
        }
        protected Displayer displayer;
        private int currentLocationDispalyerTop = 0;
        public ExclusiveSwitchableManager ExclusiveSwitchableManager { get; protected set; } = null;
        public ButtonOrgnizer RightBottomButtonOrgnizer { get; protected set; } = null;
        public LocationDisplay UsePolarLocationDisplayer()
        {
            LocationDisplay e = new LocationDisplay(new PositionInfo() { LocationType = CoordinateLocation.FixedPosition, FixLocation = new PointF(0, currentLocationDispalyerTop) }, "宋体", 25, Color.White) { CoordinateType = CoordinateType.Polar, };
            displayer.Elements.Add(30, e);
            currentLocationDispalyerTop += 40;
            return e;
        }

        public LocationDisplay UseRectangularLocationDisplayer()
        {
            LocationDisplay e = new LocationDisplay(new PositionInfo() { LocationType = CoordinateLocation.FixedPosition, FixLocation = new PointF(0, currentLocationDispalyerTop) }, "宋体", 25, Color.White) { CoordinateType = CoordinateType.Rectangular, };
            displayer.Elements.Add(30, e);
            currentLocationDispalyerTop += 40;
            return e;
        }

        public AxisX UseXAxis()
        {
            AxisX e = new AxisX();
            displayer.Elements.Add(1, e);
            return e;
        }

        public AxisY UseYAxis()
        {
            AxisY e = new AxisY();
            displayer.Elements.Add(1, e);
            return e;
        }

        public ZoomController UseZoomer(bool animation = false)
        {
            AddExclusiveManager();
            var ZoomController = new ZoomController(GetSelectStrategy()) { Animation = animation };
            ExclusiveSwitchableManager.Add(ZoomController);
            AddButtonOrgnizer();
            RightBottomButtonOrgnizer.AddButton("复位", (b) =>
            {
                ZoomController.Reset();
            });
            return ZoomController;
        }
        public MouseDragger UseMouseDragger()
        {
            AddExclusiveManager();
            MouseDragger dragger = new MouseDragger();
            ExclusiveSwitchableManager.Add(dragger);
            return dragger;
        }

        protected void AddButtonOrgnizer()
        {
            if (RightBottomButtonOrgnizer == null)
            {
                RightBottomButtonOrgnizer = new ButtonOrgnizer(new ViewRightBottom());
                displayer.Elements.Add(501, RightBottomButtonOrgnizer);
            }
        }

        protected void AddExclusiveManager()
        {
            if (ExclusiveSwitchableManager == null)
            {
                ExclusiveSwitchableManager = new ExclusiveSwitchableManager(new ViewRightTop());
                displayer.Elements.Add(500, ExclusiveSwitchableManager);
            }
        }

        protected abstract SelectStrategy GetSelectStrategy();

        public virtual Displayer Displayer => displayer;
    }
}
