namespace Utilities.RadarWorks
{
    public interface ISwtichable
    {
        void On();
        void Off();
        bool IsOn { get; }
        string Name { get; set; }
    }
}
