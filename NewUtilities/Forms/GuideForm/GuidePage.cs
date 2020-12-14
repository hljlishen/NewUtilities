using System.Windows.Forms;

namespace NewUtilities.Forms.GuideForm
{
    public abstract partial class GuidePage<T> : Form
    {
        private GuidePage<T> next;

        public GuidePage()
        {
            InitializeComponent();
            TopLevel = false;
        }

        public GuidePage<T> Next
        {
            get => next;
            set
            {
                next = value;
                next.Previous = this;
            }
        }
        public GuidePage<T> Previous { get; set; }
        public virtual void SetModel(T model) => Model = model;
        public T Model { get; private set; }
    }
}
