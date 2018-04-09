using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.IO;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class PresetEditorControl : UserControl
    {
        public List<Preset> Presets { get; } = new List<Preset>();

        public int SelectedPresetIndex => ComboBoxPresets.SelectedIndex;
        public bool IsPresetSelected => 0 <= SelectedPresetIndex && SelectedPresetIndex < Presets.Count;
        public Preset SelectedPreset => Presets[SelectedPresetIndex];

        public List<ScriptFile> PreScripts => SelectedPreset.Scripts;

        public string RootScript => TextBoxRootScript.Text;

        private Plugin Plugin { get; }

        public PresetEditorControl(Plugin plugin)
        {
            Plugin = plugin;

            InitializeComponent();

            PreScriptEditor.PresetEditor = this;

            foreach (var path in Directory.GetDirectories(Configuration.PresetsDirPath))
            {
                if (Guid.TryParse(Path.GetFileName(path), out Guid guid))
                {
                    Presets.Add(new Preset(guid));
                }
            }

            UpdateDataSource();
            PreScriptEditor.UpdateDataSource();
            UpdateEnable();

            if (Presets.Count > 0)
            {
                ComboBoxPresets.SelectedIndex = 0;
            }
        }

        public void UpdatePreScript(string path, string script)
        {
            PreScriptEditor.UpdatePreScript(path, script);
        }

        public string GetPreScript(string path)
        {
            return PreScriptEditor.GetPreScript(path);
        }

        public void GenerateCurtainFire()
        {
            if (PreScriptEditor.IsScriptSelected)
            {
                Plugin.Config.ScriptPath = PreScriptEditor.SelectedPreScript.Path;
            }

       //     if (IsPresetSelected)
            {
                Plugin.Config.PmxExportDirPath = SelectedPreset.ExportDirPmx;
                Plugin.Config.VmdExportDirPath = SelectedPreset.ExportDirVmd;
            }

            Plugin.Run(null);
        }

        private bool IsUpdating { get; set; }

        private void UpdateDataSource()
        {
            IsUpdating = true;

            Presets.Sort((p1, p2) => string.Compare(p1.PresetName, p2.PresetName));

            ComboBoxPresets.DataSource = null;
            ComboBoxPresets.DataSource = Presets;
            ComboBoxPresets.DisplayMember = "PresetName";

            IsUpdating = false;
        }

        private void UpdateEnable()
        {
            TextBoxPresetName.Enabled = IsPresetSelected;
            TextBoxExportDirPmx.Enabled = IsPresetSelected;
            TextBoxExportDirVmd.Enabled = IsPresetSelected;
            ButtonReferencePmx.Enabled = IsPresetSelected;
            ButtonReferenceVmd.Enabled = IsPresetSelected;
        }

        private void SelectedPresetIndexChanged(object sender, EventArgs e)
        {
            if (IsUpdating) return;

            if (IsPresetSelected)
            {
                TextBoxPresetName.Text = SelectedPreset.PresetName;
                TextBoxRootScript.Text = SelectedPreset.RootScript.Replace("\r\n", "\n").Replace("\n", "\r").Replace("\r", "\r\n");
                TextBoxExportDirPmx.Text = SelectedPreset.ExportDirPmx;
                TextBoxExportDirVmd.Text = SelectedPreset.ExportDirVmd;
            }
            else
            {
                TextBoxPresetName.Text = TextBoxExportDirPmx.Text = TextBoxExportDirVmd.Text = TextBoxRootScript.Text = "";
            }
            UpdateEnable();
            PreScriptEditor.UpdateDataSource();
        }

        private void ReferenceExportDirPmx(object sender, EventArgs e)
        {
            FolderBrowserDialog.SelectedPath = TextBoxExportDirPmx.Text;

            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TextBoxExportDirPmx.Text = FolderBrowserDialog.SelectedPath;
            }
        }

        private void ReferenceExportDirVmd(object sender, EventArgs e)
        {
            FolderBrowserDialog.SelectedPath = TextBoxExportDirVmd.Text;

            if (FolderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                TextBoxExportDirVmd.Text = FolderBrowserDialog.SelectedPath;
            }
        }

        private void TextChangedRootScript(object sender, EventArgs e)
        {
            if (IsPresetSelected)
            {
                SelectedPreset.RootScript = TextBoxRootScript.Text;
            }
        }

        private void TextChangedPresetName(object sender, EventArgs e)
        {
            if (IsPresetSelected)
            {
                SelectedPreset.PresetName = TextBoxPresetName.Text;
            }
        }

        private void TextChangedExportDirPmx(object sender, EventArgs e)
        {
            if (IsPresetSelected)
            {
                SelectedPreset.ExportDirPmx = TextBoxExportDirPmx.Text;
            }
        }

        private void TextChangedExportDirVmd(object sender, EventArgs e)
        {
            if (IsPresetSelected)
            {
                SelectedPreset.ExportDirVmd = TextBoxExportDirVmd.Text;
            }
        }

        private void AddPreset(object sender, EventArgs e)
        {
            Presets.Add(new Preset(Guid.NewGuid()) { PresetName = "NewPreset" + Enumerable.Range(0, Int32.MaxValue).First(i => Presets.All(p => p.PresetName != ("NewPreset" + i))) });

            UpdateDataSource();

            ComboBoxPresets.SelectedIndex = Presets.Count - 1;
        }

        private void LeaveTextBoxPresetName(object sender, EventArgs e)
        {
            UpdateDataSource();
        }

        public void Save() => Presets.ForEach(p => p.Save());
    }
}