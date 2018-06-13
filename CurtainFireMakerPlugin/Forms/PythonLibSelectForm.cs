using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class PythonLibSelectForm : Form
    {
        public string Path { get => textBoxPath.Text; set => textBoxPath.Text = value; }

        public PythonLibSelectForm()
        {
            InitializeComponent();
        }

        private void ClickOK(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ClickOpenDialog(object sender, EventArgs e)
        {
            if (openFileDialogPython.ShowDialog() == DialogResult.OK)
            {
                Path = openFileDialogPython.FileName;
            }
        }
    }
}
