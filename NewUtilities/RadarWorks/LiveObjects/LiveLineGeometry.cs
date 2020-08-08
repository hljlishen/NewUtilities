using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;

namespace Utilities.RadarWorks
{
    public class LiveLineGeometry : LiveGeometry
    {
        private Point2F[] Points;

        public LiveLineGeometry(params Point2F[] points)
        {
            Points = points;
        }

        protected override Geometry GetGeometry(RenderTarget rt)
        {
            if (Points == null || Points.Length < 3)
                return null;
            PathGeometry geo = rt.Factory.CreatePathGeometry();
            GeometrySink gs = geo.Open();
            gs.BeginFigure(Points[0], FigureBegin.Filled);
            gs.AddLines(Points);
            //gs.AddLine(Points[0]);
            gs.EndFigure(FigureEnd.Closed);
            gs.Close();
            gs.Dispose();

            return geo;
        }
    }
}
