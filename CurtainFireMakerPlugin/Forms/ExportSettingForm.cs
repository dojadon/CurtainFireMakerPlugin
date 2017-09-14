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
        public string ModelName { get => modelNameText.Text; set => modelNameText.Text = value; }
        public string ModelDescription { get => modelDescriptionText.Text; set => modelDescriptionText.Text = value; }
        public string ExportDirPath
        {
            get => exportDirText.Text; set
            {
                exportDirText.Text = value;
                exportDirDialog.SelectedPath = value;
            }
        }
        public bool KeepLogOpen { get => keepLogOpenCheckBox.Checked; set => keepLogOpenCheckBox.Checked = value; }
        public int StartFrame { get => (int)StartFrameNumeric.Value; set => StartFrameNumeric.Value = value; }

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

        private void Click_ExportDir(object sender, EventArgs e)
        {
            var result = exportDirDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                ExportDirPath = exportDirDialog.SelectedPath;
            }
        }

        private void Click_InitIronPython(object sender, EventArgs e)
        {
            Plugin.Instance.InitIronPython();
        }
    }
}
