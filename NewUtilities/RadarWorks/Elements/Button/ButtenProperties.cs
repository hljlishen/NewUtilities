﻿using System.Drawing;

namespace Utilities.RadarWorks
{
    public struct ButtenProperties
    {
        public Point Location;
        public Size ForeSize;
        public Color ForeColor;
        public Color SelectedColor;
        public Color ForeFrameColor;
        public Color SelectedFrameColor;
        public int FrameWidth;
        public string Text;
        public string ForeFontName;
        public Color ForeFontColor;
        public int ForeFontSize;
        public string SelectedFontName;
        public Color SelectedFontColor;
        public int SelectedFontSize;

        public ButtenProperties(Point location,Size foreSize, string text) : this(location, foreSize, text, Color.Gray, Color.Yellow, Color.White, Color.Black, 2, foreFontName: "微软雅黑", foreFontColor: Color.Black, foreFontSize: 15, selectedFontName: "微软雅黑", selectedFontColor: Color.Black, selectedFontSize: 15)
        {
        }

        public ButtenProperties(Point location, Size foreSize, string text, Color foreColor, Color selectedColor, Color frameColor, Color selectedFrameColor, int frameWidth, string foreFontName, Color foreFontColor, int foreFontSize, string selectedFontName, Color selectedFontColor, int selectedFontSize)
        {
            Location = location;
            ForeSize = foreSize;
            ForeColor = foreColor;
            SelectedColor = selectedColor;
            ForeFrameColor = frameColor;
            FrameWidth = frameWidth;
            Text = text;
            ForeFontName = foreFontName;
            ForeFontColor = foreFontColor;
            ForeFontSize = foreFontSize;
            SelectedFontName = selectedFontName;
            SelectedFontColor = selectedFontColor;
            SelectedFontSize = selectedFontSize;
            SelectedFrameColor = selectedFrameColor;
        }
    }
}
