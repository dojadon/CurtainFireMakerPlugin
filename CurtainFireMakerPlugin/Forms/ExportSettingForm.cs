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
                TextBoxProjectScript.Text = ProjectScriptControl.GetScript(Path.GetFileNameWithoutExtension(TextBoxScriptPath.Text));
            }
        }

        public string PmxExportDirPath
        {
            get => TextBoxPmxExportPath.Text;
            set
            {
                TextBoxPmxExportPath.Text = value;
                FolderBrowserDialogPmx.SelectedPath = value;
            }
        }

        public string VmdExportDirPath
        {
            get => TextBoxVmdExportPath.Text;
            set
            {
                TextBoxVmdExportPath.Text = value;
                FolderBrowserDialogVmd.SelectedPath = value;
            }
        }

        public bool DropPmxFile { get => CheckBoxDropPmxFile.Checked; set => CheckBoxDropPmxFile.Checked = value; }
        public bool DropVmdFile { get => CheckBoxDropVmdFile.Checked; set => CheckBoxDropVmdFile.Checked = value; }

        private Configuration Config { get; }

        private ProjectScriptControl ProjectScriptControl { get; }

        public ExportSettingForm(Configuration config, ProjectScriptControl projectScriptControl)
        {
            Config = config;
            ProjectScriptControl = projectScriptControl;

            InitializeComponent();

            ScriptPath = Config.ScriptPath;
            PmxExportDirPath = Config.PmxExportDirPath;
            VmdExportDirPath = Config.VmdExportDirPath;
            DropPmxFile = Config.ShouldDropPmxFile;
            DropVmdFile = Config.ShouldDropVmdFile;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Config.ScriptPath = ScriptPath;
            Config.PmxExportDirPath = PmxExportDirPath;
            Config.VmdExportDirPath = VmdExportDirPath;
            Config.ShouldDropPmxFile = DropPmxFile;
            Config.ShouldDropVmdFile = DropVmdFile;
        }

        private void Click_OK(object sender, EventArgs e)
        {
            Close();
            DialogResult = DialogResult.OK;
            ProjectScriptControl.AddScript(Path.GetFileNameWithoutExtension(TextBoxScriptPath.Text), TextBoxProjectScript.Text);
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

        private void LoadForm(object sender, EventArgs e)
        {
            ActiveControl = label1;
        }
    }
}
