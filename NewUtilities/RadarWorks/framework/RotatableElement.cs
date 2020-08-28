using Utilities.Mapper;

namespace Utilities.RadarWorks
{
    public abstract class RotatableElement<T> : DynamicElement<T>
    {
        protected PolarRotateDecorator rotateDecorator = null;
        protected string RotateDecoratorName;

        protected RotatableElement(string rotateDecoratorName = "default")
        {
            RotateDecoratorName = rotateDecoratorName;
        }

        public double RotateAngle => rotateDecorator.RotateAngle;

        public override void SetDisplayer(Displayer d)
        {
            if(rotateDecorator == null)
            {
                rotateDecorator = PolarRotateDecorator.GetInstance(d.GetHashCode().ToString() + RotateDecoratorName, d.Mapper);
            }
            base.SetDisplayer(d);
        }

        public override IScreenToCoordinateMapper Mapper => rotateDecorator;

        public IScreenToCoordinateMapper InnerMapper => rotateDecorator.Mapper;
    }
}
