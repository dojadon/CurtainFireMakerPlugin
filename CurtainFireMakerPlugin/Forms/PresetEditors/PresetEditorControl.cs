using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Scripting.Hosting;

namespace CurtainFireMakerPlugin.Forms
{
    interface IPresetEditor
    {
        void LoadPreset(Preset preset, string path);
        void SavePreset(Preset preset, string path);
        void LoadConfig(PluginConfig config);
        void SaveConfig(PluginConfig config);
        bool IsUpdated(Preset preset, string path);

        event EventHandler ValueChangedEvent;
    }

    public partial class PresetEditorControl : UserControl
    {
        private Preset Preset { get; } = new Preset();

        private IPresetEditor[] PresetEditors { get; }

        public string PresetPath { get; private set; }

        public string SavePresetFileName =>
          Path.IsPathRooted(PresetPath) ? PresetPath : $"{Path.GetFileNameWithoutExtension(PresetSequenceEditorControl.SelectedFilePath)}.xml";

        public string ExportDirectory => Path.GetDirectoryName(PresetPath);
        public string FileName => Path.GetFileNameWithoutExtension(PresetPath);

        public int StartFrame => PresetSettingControl.StartFrame;
        public int EndFrame => PresetSettingControl.EndFrame;

        public PresetEditorControl(string path, PluginConfig config)
        {
            PresetPath = path;

            Preset.Init();

            if (File.Exists(path)) Preset.Load(path);

            InitializeComponent();

            PresetEditors = new IPresetEditor[] { PresetSequenceEditorControl, PresetSettingControl };
            PresetEditors.ForEach(e => e.ValueChangedEvent += (o, a) => UpdateWeatherChanged());

            LoadConfig(config);
            LoadPreset(Preset, PresetPath);
        }

        private void UpdateWeatherChanged()
        {
            const string UpdatedChar = "*";

            if (Parent != null)
            {
                if (IsUpdated(PresetPath))
                {
                    if (!Parent.Text.EndsWith(UpdatedChar)) Parent.Text += UpdatedChar;
                }
                else
                {
                    if (Parent.Text.EndsWith(UpdatedChar)) Parent.Text = Parent.Text.TrimEnd(char.Parse(UpdatedChar));
                }
            }

            Invalidate();
        }

        public void LoadConfig(PluginConfig config) => PresetEditors.ForEach(p => p.LoadConfig(config));

        public void SaveConfig(PluginConfig config) => PresetEditors.ForEach(p => p.SaveConfig(config));

        public bool IsUpdated(string path) => PresetEditors.Any(c => c.IsUpdated(Preset, path));

        public void LoadPreset(Preset preset, string path) => PresetEditors.ForEach(c => c.LoadPreset(preset, path));

        public void SavePreset(Preset preset, string path) => PresetEditors.ForEach(c => c.SavePreset(preset, path));

        public void RunScript(ScriptEngine engine, ScriptScope scope) => PresetSequenceEditorControl.RunScript(engine, scope);

        public bool CheckSave(Microsoft.Win32.SaveFileDialog dialog)
        {
            if (IsUpdated(PresetPath))
            {
                var result = MessageBox.Show("保存されてない変更があります。\r\n変更を保存しますか？", Path.GetFileNameWithoutExtension(PresetPath), MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                switch (result)
                {
                    case DialogResult.Yes:
                        Save(dialog);
                        break;

                    case DialogResult.No:
                        break;

                    case DialogResult.Cancel:
                        return true;
                }
            }
            return false;
        }

        public void Save(Microsoft.Win32.SaveFileDialog dialog)
        {
            if (Path.IsPathRooted(PresetPath))
            {
                SavePreset(Preset, PresetPath);
                Preset.Save(PresetPath);
                UpdateWeatherChanged();
            }
            else
            {
                SaveAs(dialog);
            }
        }

        public void SaveAs(Microsoft.Win32.SaveFileDialog dialog)
        {
            if (PluginControl.ShowFileDialog(dialog, out string path))
            {
                PresetPath = path;
                Parent.Text = Path.GetFileNameWithoutExtension(PresetPath);
                Save(dialog);
            }
        }
    }
}
