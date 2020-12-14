using System.Drawing;
using System.Windows.Forms;
using Utilities.Mapper;
using Utilities.RadarWorks;
using Utilities.RadarWorks.Elements.Markers;
using Utilities.RadarWorks.Elements.Ppi.WaveGateController;

namespace NewUtilities.RadarWorks.DisplayerBuilders
{
    public class PpiBuilder : DisplayerBuilder
    {
        public PpiBuilder(Control p, IScreenToCoordinateMapper mapper, ReferenceSystem referenceSystem) : base(p, mapper, referenceSystem)
        {
        }

        public PpiRotationController UseRotator()
        {
            AddExclusiveManager();
            var rotator = new PpiRotationController() { TextColor = Color.White };
            ExclusiveSwitchableManager.Add(rotator);
            return rotator;
        }

        public MultiMarker<PolarAngleMarker> UseAngleMarker(int markCount = 12)
        {
            var marker = new MultiMarker<PolarAngleMarker>(markCount) { IteratorType = MarkerIteratorType.NoRight };
            displayer.Elements.Add(4, marker);
            return marker;
        }

        public MultiMarker<PolarDistanceMarker> UseDistanceMarker(int markerCount = 5)
        {
            MultiMarker<PolarDistanceMarker> marker = new MultiMarker<PolarDistanceMarker>(markerCount) { IteratorType = MarkerIteratorType.NoLeft };
            displayer.Elements.Add(5, marker);
            return marker;
        }

        public PolarSectionSweepController UseSweepController()
        {
            var SectionSweepController = new PolarSectionSweepController();
            AddExclusiveManager();
            ExclusiveSwitchableManager.Add(SectionSweepController);
            return SectionSweepController;
        }

        public WaveGateManager UseWaveGateMananger()
        {
            var waveGateManger = new WaveGateManager();
            AddExclusiveManager();
            ExclusiveSwitchableManager.Add(waveGateManger);
            return waveGateManger;
        }

        protected override SelectStrategy GetSelectStrategy() => new SquareSelection();
    }
}
