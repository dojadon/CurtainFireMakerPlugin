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
        public string ScriptPath
        {
            get => TextBoxScriptPath.Text;
            set
            {
                TextBoxScriptPath.Text = value;
                OpenFileDialogScript.FileName = value;
                OpenFileDialogScript.InitialDirectory = Path.GetDirectoryName(value);
            }
        }

        public string PmxExportDirPath
        {
            get => TextBoxPmxExportPath.Text; set
            {
                TextBoxPmxExportPath.Text = value;
                FolderBrowserDialogPmx.SelectedPath = value;
            }
        }

        public string VmdExportDirPath
        {
            get => TextBoxVmdExportPath.Text; set
            {
                TextBoxVmdExportPath.Text = value;
                FolderBrowserDialogVmd.SelectedPath = value;
            }
        }

        public bool KeepLogOpen { get => CheckBoxKeepLog.Checked; set => CheckBoxKeepLog.Checked = value; }
        public bool DropPmxFile { get => CheckBoxDropPmxFile.Checked; set => CheckBoxDropPmxFile.Checked = value; }
        public bool DropVmdFile { get => CheckBoxDropVmdFile.Checked; set => CheckBoxDropVmdFile.Checked = value; }

        private Plugin Plugin { get; }

        public ExportSettingForm(Plugin plugin)
        {
            Plugin = plugin;

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
            var result = OpenFileDialogScript.ShowDialog();

            if (result == DialogResult.OK)
            {
                ScriptPath = OpenFileDialogScript.FileName;
            }
        }

        private void Click_PmxExportDir(object sender, EventArgs e)
        {
            var result = FolderBrowserDialogPmx.ShowDialog();

            if (result == DialogResult.OK)
            {
                PmxExportDirPath = FolderBrowserDialogPmx.SelectedPath;
            }
        }

        private void Click_VmdExportDir(object sender, EventArgs e)
        {
            var result = FolderBrowserDialogVmd.ShowDialog();

            if (result == DialogResult.OK)
            {
                VmdExportDirPath = FolderBrowserDialogVmd.SelectedPath;
            }
        }

        private void Click_InitIronPython(object sender, EventArgs e)
        {
            Plugin.InitIronPython();
        }

        private void LoadForm(object sender, EventArgs e)
        {
            ActiveControl = label1;
        }
    }
}
