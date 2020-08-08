namespace Utilities.IniBind.InterfaceInterception
{
    public abstract class IIniBindInterface
    {
        [NotIniKey]
        public abstract string FilePath { get; }
        [NotIniKey]
        public abstract string Section { get; }
    }
}
