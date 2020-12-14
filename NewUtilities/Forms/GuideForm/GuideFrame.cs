using System;
using System.Windows.Forms;

namespace NewUtilities.Forms.GuideForm
{
    public partial class GuideFrame<T> : Form where T : new()
    {
        private GuidePage<T> currentPage;

        public GuideFrame()
        {
            InitializeComponent();
            Model = new T();
        }

        private void previous_btn_Click(object sender, EventArgs e)
        {
            CurrentPage = CurrentPage.Previous;
        }

        private void next_btn_Click(object sender, EventArgs e)
        {
            CurrentPage = CurrentPage.Next;
        }

        private void finish_btn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        public GuidePage<T> CurrentPage
        {
            get => currentPage;
            set
            {
                panel1.Controls.Clear();
                Model = currentPage.Model;
                currentPage = value;
                currentPage.SetModel(Model);
                panel1.Controls.Add(currentPage);
                currentPage.Dock = DockStyle.Fill;
                next_btn.Enabled = currentPage.Next != null;
                previous_btn.Enabled = currentPage.Previous != null;
                currentPage.Show();
            }
        }

        public T Model { get; private set; }
    }
}
