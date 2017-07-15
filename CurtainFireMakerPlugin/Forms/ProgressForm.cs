using System.IO;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class ProgressForm : Form
    {
        public ProgressBar Progress => this.progressBar;

        public string LogText { get { return this.logTextBox.Text; } set { this.logTextBox.Text = value; } }

        public ProgressForm()
        {
            InitializeComponent();
        }
    }
}
