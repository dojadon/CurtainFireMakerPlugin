using System.IO;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class ProgressForm : Form
    {
        public ProgressBar Progress => this.progressBar;
        public TextBox LogTextBox => this.logTextBox;

        public ProgressForm()
        {
            InitializeComponent();
        }
    }
}
