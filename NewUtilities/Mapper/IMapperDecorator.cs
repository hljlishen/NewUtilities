﻿using System;
using System.Drawing;
using Utilities.Models;

namespace Utilities.Mapper
{
    public class MapperDecorator : IScreenToCoordinateMapper
    {
        public MapperDecorator(IScreenToCoordinateMapper mapper)
        {
            Mapper = mapper;
        }
        public event Action<IScreenToCoordinateMapper> MapperStateChanged;
        protected void InvokeStateChanged() => MapperStateChanged?.Invoke(this);
        public IScreenToCoordinateMapper Mapper { get; private set; }
        public virtual double ScreenLeft => Mapper.ScreenLeft;
        public virtual double ScreenRight => Mapper.ScreenRight;
        public virtual double ScreenTop => Mapper.ScreenTop;
        public virtual double ScreenBottom => Mapper.ScreenBottom;
        public virtual double CoordinateLeft => Mapper.CoordinateLeft;
        public virtual double CoordinateRight => Mapper.CoordinateRight;
        public virtual double CoordinateTop => Mapper.CoordinateTop;
        public virtual double CoordinateBottom => Mapper.CoordinateBottom;
        public virtual PointF ScreenCenter => Mapper.ScreenCenter;
        public virtual double ScreenWidth => Mapper.ScreenWidth;
        public virtual double ScreenHeight => Mapper.ScreenHeight;
        public virtual PointD GetCoordinateLocation(double screenX, double screenY) => Mapper.GetCoordinateLocation(screenX, screenY);
        public virtual double GetCoordinateX(double screenX) => Mapper.GetCoordinateX(screenX);
        public virtual double GetCoordinateY(double screenY) => Mapper.GetCoordinateY(screenY);
        public virtual PointD GetScreenLocation(double coordinateX, double coordinateY) => Mapper.GetScreenLocation(coordinateX, coordinateY);
        public virtual double GetScreenX(double coordinateX) => Mapper.GetScreenX(coordinateX);
        public virtual double GetScreenY(double coordinateY) => Mapper.GetScreenY(coordinateY);
        public virtual void SetScreenArea(double left, double right, double top, double bottom)
        {
            Mapper.SetScreenArea(left, right, top, bottom);
            InvokeStateChanged();
        }

        public virtual void SetCoordinateArea(double left, double right, double top, double bottom)
        {
            Mapper.SetCoordinateArea(left, right, top, bottom);
            InvokeStateChanged();
        }
    }
}
