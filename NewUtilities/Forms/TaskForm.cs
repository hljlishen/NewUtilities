using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utilities.Guards;

namespace Utilities.Forms
{
    public interface IProcesure
    {
        Action task { get; set; }
        event Action<int> ProgressChanged;
    }

    public partial class TaskForm : Form
    {
        private IProcesure procesure;
        public string Msg { get; set; } = "正在处理";
        public TaskForm(IProcesure p)
        {
            InitializeComponent();
            this.procesure = Guard.NullCheckAssignment(p);
            StartPosition = FormStartPosition.CenterParent;
            progressBar1.Maximum = 100;
            p.ProgressChanged += P_ProgressChanged;
        }

        private void P_ProgressChanged(int obj)
        {
            progressBar1.Value = obj;
        }

        private async void TaskForm_Load(object sender, EventArgs e)
        {
            msg_lab.Text = Msg;
            await Task.Run(procesure.task);
            Close();
        }
    }
}
