using Microsoft.WindowsAPICodePack.DirectX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using Utilities.Mapper;
using Utilities.RadarWorks;

namespace NewUtilities.RadarWorks.Elements.Signal
{
    public class IFrequencyData
    {
        public double Frequency { get; set; }
        public double Am { get; set; }
    }

    public class FrequenctDataView
    {
        public double ScrX { get; set; }
        public double ScrY { get; set; }
        public ColorF Color { get; set; }
    }

    public class WaterFallPlotter : DynamicElement<List<IFrequencyData>>
    {
        public int TimeSpan { get; set; } = 10;
        private bool NeedRecalculatePosition = false;
        private bool firstTime = true;
        public double AmMax
        {
            get => RedMapper.Value1Right;
            set => RedMapper.SetRange1(RedMapper.Value1Left, value);
        }
        public double AmMin
        {
            get => RedMapper.Value1Left;
            set => RedMapper.SetRange1(value, RedMapper.Value1Right);
        }
        protected override void DrawDynamicElement(RenderTarget rt)
        {
            var xResolution = (float)Mapper.ScreenWidth / TimeLines[0].Count;
            var yResolution = (float)Mapper.ScreenHeight / TimeLines.Length;
            if (NeedRecalculatePosition)
            {
                for (int j = 0; j < TimeLines.Length; j++)
                {
                    if (HistoryData[j] != null)
                        TimeLines[j] = GetView(HistoryData[j]).ToList();
                }
                NeedRecalculatePosition = false;
            }
            int i = 0;
            foreach (var line in TimeLines)
            {
                foreach (var d in line)
                {
                    if (d.ScrX >= Mapper.ScreenLeft && d.ScrX <= Mapper.ScreenRight && d.ScrY >= Mapper.ScreenTop && d.ScrY <= Mapper.ScreenTop + Mapper.ScreenHeight)
                    {
                        using (var brush = rt.CreateSolidColorBrush(d.Color))
                        {
                            rt.DrawLine(new Point2F((float)d.ScrX, (float)(d.ScrY + yResolution * i)), new Point2F((float)d.ScrX, (float)(d.ScrY + yResolution * (i + 1))), brush, xResolution);
                        }
                    }
                }
                i++;
            }
        }

        protected override void DoUpdate(List<IFrequencyData> t)
        {
            if(firstTime)
            {
                ReferenceSystem.SetArea(t[0].Frequency, t[t.Count - 1].Frequency, ReferenceSystem.Top, ReferenceSystem.Bottom);
                firstTime = false;
            }
            //时间线下移，删除最后一行时间线
            for (int i = TimeLines.Length - 1; i >= 1; i--)
            {
                HistoryData[i] = HistoryData[i - 1];
                TimeLines[i] = TimeLines[i - 1];
            }

            //计算最新一行点的坐标
            HistoryData[0] = t;
            TimeLines[0] = GetView(t).ToList();

            Redraw();
        }

        public override void SetDisplayer(Displayer d)
        {
            base.SetDisplayer(d);
            Mapper.MapperStateChanged += Mapper_MapperStateChanged;
        }

        private void Mapper_MapperStateChanged(IScreenToCoordinateMapper obj)
        {
            NeedRecalculatePosition = true;
        }

        private IEnumerable<FrequenctDataView> GetView(IEnumerable<IFrequencyData> data)
        {
            foreach (var item in data)
            {
                var scrCoordinate = Mapper.GetScreenLocation(item.Frequency, 0);
                var color = GetColor(item.Am);
                yield return new FrequenctDataView() { ScrX = scrCoordinate.X, ScrY = scrCoordinate.Y, Color = color };
            }

            ColorF GetColor(double am)
            {
                var red = RedMapper.MapToValue2(am);
                var blue = BlueMapper.MapToValue2(am);
                float green = 0;
                if (am >= 5)
                {
                    green = (float)GreenMapper1.MapToValue2(am);
                }
                else
                {
                    green = (float)GreenMapper2.MapToValue2(am);
                }

                red = Math.Max(0, red);
                blue = Math.Max(0, blue);
                green = Math.Max(0, green);
                return new ColorF((float)red, (float)green, (float)blue);
            }
        }

        private readonly List<FrequenctDataView>[] TimeLines;
        private readonly ValueMapper RedMapper = new ValueMapper(7.5, 15, 0, 1);
        private readonly ValueMapper BlueMapper = new ValueMapper(7.5, 0, 0, 1);
        private readonly ValueMapper GreenMapper1 = new ValueMapper(15, 7.5, 0, 1);
        private readonly ValueMapper GreenMapper2 = new ValueMapper(7.5, 0, 1, 0);
        private readonly List<IFrequencyData>[] HistoryData;
        public WaterFallPlotter()
        {
            TimeLines = new List<FrequenctDataView>[TimeSpan];
            HistoryData = new List<IFrequencyData>[TimeSpan];
            for (int i = 0; i < TimeLines.Length; i++)
            {
                TimeLines[i] = new List<FrequenctDataView>();
            }
        }
    }
}
