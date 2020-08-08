using System.Drawing;

namespace Utilities.RadarWorks
{
    public interface IButtonLayout
    {
        int ButtonAlignmentH { get; set; }
        int ButtonAlignmentV { get; set; }
        Size ButtonSize { get; set; }
        uint ColumnSize { get; set; }
        Point NextLocation();
        void Reset();
        Displayer Displayer { get; set; }
    }

    public abstract class ButtonLayoutBase : IButtonLayout
    {
        public int ButtonAlignmentH { get; set; } = 10;
        public int ButtonAlignmentV { get; set; } = 10;
        public Size ButtonSize { get; set; } = new Size(80, 40);
        public uint ColumnSize { get; set; } = 1;
        public Displayer Displayer { get; set; }

        protected int CurrentRowIndex = 0;
        protected int CurrentColumnIndex = 0;

        public abstract Point NextLocation();

        public virtual void Reset()
        {
            CurrentColumnIndex = 0;
            CurrentRowIndex = 0;
        }

        protected void IncreaseIndex()
        {
            CurrentColumnIndex++;
            if(CurrentColumnIndex == ColumnSize)
            {
                CurrentColumnIndex = 0;
                CurrentRowIndex++;
            }    
        }
    }
}