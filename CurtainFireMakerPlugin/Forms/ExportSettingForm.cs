using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class ExportSettingForm : Form
    {
        public string ScriptPath
        {
            get { return this.scriptText.Text; }
            set
            {
                this.scriptText.Text = value;
                this.scriptFileDialog.FileName = value;
            }
        }
        public string ModelName { get { return this.modelNameText.Text; } set { this.modelNameText.Text = value; } }
        public string ModelDescription { get { return this.modelDescriptionText.Text; } set { this.modelDescriptionText.Text = value; } }
        public string ExportDirPath
        {
            get { return this.exportDirText.Text; }
            set
            {
                this.exportDirText.Text = value;
                this.exportDirDialog.SelectedPath = value;
            }
        }
        public bool KeepLogOpen { get { return this.checkBox1.Checked; } set { this.checkBox1.Checked = value; } }

        public ExportSettingForm()
        {
            InitializeComponent();
        }

        private void Click_OK(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.OK;
        }

        private void Click_Cancel(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void Click_Script(object sender, EventArgs e)
        {
            var result = this.scriptFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.scriptText.Text = this.scriptFileDialog.FileName;
            }
        }

        private void Click_ExportDir(object sender, EventArgs e)
        {
            var result = this.exportDirDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.exportDirText.Text = this.exportDirDialog.SelectedPath;
            }
        }
    }
}
