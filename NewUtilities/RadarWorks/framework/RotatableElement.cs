﻿using Utilities.Mapper;

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
            rotateDecorator.MapperStateChanged += RotateDecorator_MapperStateChanged;
            base.SetDisplayer(d);
        }

        private void RotateDecorator_MapperStateChanged(IScreenToCoordinateMapper obj) => Redraw();

        public override IScreenToCoordinateMapper Mapper => rotateDecorator;

        public IScreenToCoordinateMapper InnerMapper => rotateDecorator.Mapper;

        public override void Dispose()
        {
            base.Dispose();
            rotateDecorator.MapperStateChanged -= RotateDecorator_MapperStateChanged;
        }
    }
}
