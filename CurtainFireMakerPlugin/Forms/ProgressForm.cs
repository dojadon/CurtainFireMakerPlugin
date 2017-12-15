using System.IO;
using System.Text;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class ProgressForm : Form
    {
        public ProgressBar ProgressBar => progressBar;
        public string LogText { get => logTextBox.Text; set => logTextBox.Text = value; }

        public ProgressForm()
        {
            InitializeComponent();
        }

        private void ProgressForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
