namespace Utilities.RadarWorks
{
    public interface IDynamicElement<T> : IGraphic
    {
        T Model { get; set; }

        void Update(T t);
    }
}