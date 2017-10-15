using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class ExportSettingForm : Form
    {
        private string scriptPath;

        public string ScriptPath
        {
            get => scriptPath;
            set
            {
                scriptPath = value;
                scriptFileDialog.FileName = value;
                scriptFileDialog.InitialDirectory = Path.GetDirectoryName(value);
                scriptText.Text = Path.GetFileNameWithoutExtension(value);
            }
        }

        public string PmxExportDirPath
        {
            get => pmxExportPathTextBox.Text; set
            {
                pmxExportPathTextBox.Text = value;
                pmxExportDirDialog.SelectedPath = value;
            }
        }

        public string VmdExportDirPath
        {
            get => vmdExportPathTextBox.Text; set
            {
                vmdExportPathTextBox.Text = value;
                vmdExportDirDialog.SelectedPath = value;
            }
        }

        public bool KeepLogOpen { get => keepLogOpenCheckBox.Checked; set => keepLogOpenCheckBox.Checked = value; }
        public bool DropPmxFile { get => checkBoxDropPmxFile.Checked; set => checkBoxDropPmxFile.Checked = value; }
        public bool DropVmdFile { get => checkBoxDropVmdFile.Checked; set => checkBoxDropVmdFile.Checked = value; }

        public ExportSettingForm()
        {
            InitializeComponent();
        }

        private void Click_OK(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.OK;
        }

        private void Click_Cancel(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.Cancel;
        }

        private void Click_Script(object sender, EventArgs e)
        {
            var result = scriptFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                ScriptPath = scriptFileDialog.FileName;
            }
        }

        private void Click_PmxExportDir(object sender, EventArgs e)
        {
            var result = pmxExportDirDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                PmxExportDirPath = pmxExportDirDialog.SelectedPath;
            }
        }

        private void Click_VmdExportDir(object sender, EventArgs e)
        {
            var result = vmdExportDirDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                VmdExportDirPath = vmdExportDirDialog.SelectedPath;
            }
        }

        private void Click_InitIronPython(object sender, EventArgs e)
        {
            Plugin.Instance.InitIronPython();
        }

        private void LoadForm(object sender, EventArgs e)
        {
            ActiveControl = label1;
        }
    }
}
