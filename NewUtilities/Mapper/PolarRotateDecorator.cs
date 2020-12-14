using System;
using System.Drawing;
using Utilities.Tools;
using Utilities.Coordinates;
using System.Collections.Generic;
using Utilities.Models;

namespace Utilities.Mapper
{
    public class PolarRotateDecorator : MapperDecorator
    {
        private double rotateAngle = 0;

        private static readonly Dictionary<string, PolarRotateDecorator> instances = new Dictionary<string, PolarRotateDecorator>();
        
        public static PolarRotateDecorator GetInstance(string instanceName, IScreenToCoordinateMapper mapper)
        {
            if(!instances.ContainsKey(instanceName))
            {
                PolarRotateDecorator p = new PolarRotateDecorator(mapper);
                instances.Add(instanceName, p);
            }
            return instances[instanceName];
        }

        public double RotateAngle
        {
            get => rotateAngle;
            set
            {
                rotateAngle = value;
                InvokeStateChanged();
            }
        }
        public PointD Center { get; set; }
        private PolarRotateDecorator(IScreenToCoordinateMapper mapper) : base(mapper)
        {
            mapper.MapperStateChanged += Mapper_MapperStateChanged;
            Center = mapper.GetScreenLocation(0, 0);
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj)
        {
            Center = Mapper.GetScreenLocation(0, 0);
            InvokeStateChanged();
        }

        public override PointD GetScreenLocation(double coordinateX, double coordinateY)
        {
            var p = Mapper.GetScreenLocation(coordinateX, coordinateY);
            if (Math.Abs(RotateAngle - 0) < 0.001)
                return p;

            PolarCoordinate p1 = new RectangularCoordinate(p.X - Center.X, Center.Y - p.Y, 0).Polar;
            p1.Az += RotateAngle;
            p1.Az = Functions.StandardAngle(p1.Az);
            var r = p1.Rectangular;
            return new PointD(r.X + Center.X, Center.Y - r.Y);
        }

        public override PointD GetCoordinateLocation(double screenX, double screenY)
        {
            if (Math.Abs(RotateAngle - 0) < 0.001)
                return Mapper.GetCoordinateLocation(screenX, screenY);
            PolarCoordinate p1 = new RectangularCoordinate(screenX - Center.X, Center.Y - screenY, 0).Polar;
            p1.Az -= RotateAngle;
            p1.Az = Functions.StandardAngle(p1.Az);
            var r = p1.Rectangular;
            return base.GetCoordinateLocation((float)r.X + Center.X, Center.Y - (float)r.Y);
        }

        public override double GetCoordinateX(double screenX)
        {
            throw new Exception("PolarRotateDecorator不实现该函数");
        }

        public override double GetCoordinateY(double screenY)
        {
            throw new Exception("PolarRotateDecorator不实现该函数");
        }

        public override double GetScreenX(double coordinateX)
        {
            throw new Exception("PolarRotateDecorator不实现该函数");
        }

        public override double GetScreenY(double coordinateY)
        {
            throw new Exception("PolarRotateDecorator不实现该函数");
        }
    }
}
