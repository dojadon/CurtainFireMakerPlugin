using System.IO;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class ProgressForm : Form
    {
        public ProgressBar Progress => progressBar;
        public TextBox LogTextBox => logTextBox;

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
