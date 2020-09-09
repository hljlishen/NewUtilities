using System;
using System.Drawing;
using System.Windows.Forms;

namespace Utilities.Forms
{
    public delegate void SlashWindowCloseHandler();
    public partial class SlashForm : Form
    {
        public int FadeInInterval { get; set; } = 2000;
        private float OpacityStep = 0.05f;
        public string SlashText { get; set; } = "正在加载...";
        public Color TextColor { get; set; } = Color.White;
        public Font TextFont { get; set; } = new Font("微软雅黑", 16);
        public SlashForm()
        {
            InitializeComponent();

            info_lab.ForeColor = TextColor;
            info_lab.Font = TextFont;
            info_lab.Text = SlashText;

            StartPosition = FormStartPosition.CenterScreen;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Opacity += 0.05;
            if(Opacity  >= 1)
            {
                showTimer.Stop();
                showTimer.Dispose();
            }
        }

        public void SetSlashImg(Image img)
        {
            Size = img.Size;
            BackgroundImage = img;
        }

        public void SetText(string text) => info_lab.Text = text;

        private readonly Timer showTimer = new Timer();
        private void SlashForm_Load(object sender, EventArgs e)
        {
            Opacity = 0;
            showTimer.Interval = (int)(FadeInInterval * OpacityStep);
            showTimer.Tick += Timer_Tick;
            showTimer.Start();
        }
    }
}
