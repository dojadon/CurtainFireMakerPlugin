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
            foreach (var editor in PresetEditors)
            {
                editor.ValueChangedEvent += new EventHandler(Changed);

                void Changed(Object o, EventArgs e)
                {
                    if (o is IPresetEditor sender)
                    {
                        if (IsUpdated())
                        {
                            if (!Parent.Text.EndsWith("*")) Parent.Text += "*";
                        }
                        else
                        {
                            if (Parent.Text.EndsWith("*")) Parent.Text = Parent.Text.Trim('*');
                        }
                    }
                }
            }

            LoadConfig(config);
            LoadPreset(Preset);
        }

        public void LoadConfig(PluginConfig config) => PresetEditors.ForEach(p => p.LoadConfig(config));

        public void SaveConfig(PluginConfig config) => PresetEditors.ForEach(p => p.SaveConfig(config));

        public bool IsUpdated() => PresetEditors.Any(c => c.IsUpdated(Preset));

        public void LoadPreset(Preset preset) => PresetEditors.ForEach(c => c.LoadPreset(Preset));

        public void SavePreset(Preset preset) => PresetEditors.ForEach(c => c.SavePreset(Preset));

        public void RunScript(ScriptEngine engine, ScriptScope scope) => PresetSequenceEditorControl.RunScript(engine, scope);

        public bool CheckSave(Microsoft.Win32.SaveFileDialog dialog)
        {
            if (IsUpdated())
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
                SavePreset(Preset);
                Preset.Save(PresetPath);
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
