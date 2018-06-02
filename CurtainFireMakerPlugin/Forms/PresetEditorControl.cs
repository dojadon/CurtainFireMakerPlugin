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
        private static string ConfigPath => Plugin.PluginRootPath + "config.xml";

        private ControlConfig Config { get; } = new ControlConfig();
        private Preset Preset { get; } = new Preset();

        private string presetPath;
        public string PresetPath
        {
            get => presetPath;
            private set
            {
                presetPath = value;
                labelPath.Text = "Path : " + value;

                if (Path.IsPathRooted(PresetPath))
                {
                    openFileDialogPreset.InitialDirectory = Path.GetDirectoryName(PresetPath);
                    saveFileDialogNewPreset.InitialDirectory = Path.GetDirectoryName(PresetPath);
                }
            }
        }

        public string ExportDirectory => Path.GetDirectoryName(PresetPath);
        public string FileName => Path.GetFileNameWithoutExtension(PresetPath);

        public int StartFrame => PresetSettingControl.StartFrame;
        public int EndFrame => PresetSettingControl.EndFrame;

        public PresetEditorControl()
        {
            Preset.Init();

            InitializeComponent();

            PresetPath = "新規";

            Config.Init();
            if (File.Exists(ConfigPath))
            {
                Config.Load(ConfigPath);
                LoadConfig();
            }
            else
            {
                Config.Save(ConfigPath);
            }

            if (File.Exists(Config.RecentSelectedPresetPath))
            {
                PresetPath = Config.RecentSelectedPresetPath;
                Preset.Load(PresetPath);
            }
            LoadPreset();
        }

        public void Save()
        {
            SaveConfig();
            Config.Save(ConfigPath);

            if (Path.IsPathRooted(PresetPath))
            {
                SavePreset();
                Preset.Save(PresetPath);
            }
        }

        private void LoadConfig()
        {
            PresetSequenceEditorControl.LoadConfig(Config);
            openFileDialogPreset.FileName = saveFileDialogNewPreset.FileName = Config.RecentSelectedPresetPath;
            openFileDialogPreset.InitialDirectory = saveFileDialogNewPreset.InitialDirectory = Path.GetDirectoryName(Config.RecentSelectedPresetPath);
        }

        private void SaveConfig()
        {
            PresetSequenceEditorControl.SaveConfig(Config);
            Config.RecentSelectedPresetPath = PresetPath;
        }

        private void LoadPreset()
        {
            PresetSettingControl.LoadPreset(Preset);
            PresetSequenceEditorControl.LoadPreset(Preset);
        }

        private void SavePreset()
        {
            PresetSettingControl.SavePreset(Preset);
            PresetSequenceEditorControl.SavePreset(Preset);
        }

        public void RunScript(ScriptEngine engine, ScriptScope scope)
        {
            PresetSequenceEditorControl.RunScript(engine, scope);
        }

        private void ClickNewFile(object sender, EventArgs e)
        {
            Preset.Init();
            LoadPreset();
            PresetPath = "新規";
        }

        private void ClickOpen(object sender, EventArgs e)
        {
            if (openFileDialogPreset.ShowDialog() == DialogResult.OK)
            {
                if (Preset.IsFormated(openFileDialogPreset.FileName))
                {
                    PresetPath = openFileDialogPreset.FileName;
                    Preset.Load(PresetPath);
                    LoadPreset();
                }
                else
                {
                    MessageBox.Show($"Format Error：{openFileDialogPreset.FileName}");
                }
            }
        }

        private void ClickSave(object sender, EventArgs e)
        {
            if (Path.IsPathRooted(PresetPath))
            {
                SavePreset();
                Preset.Save(PresetPath);
            }
            else
            {
                ClickSaveAs(sender, e);
            }
        }

        private void ClickSaveAs(object sender, EventArgs e)
        {
            if (!Path.IsPathRooted(PresetPath))
            {
                saveFileDialogNewPreset.FileName = saveFileDialogNewPreset.InitialDirectory + "\\" + Path.GetFileNameWithoutExtension(PresetSequenceEditorControl.SelectedFilePath) + ".xml";
            }

            if (saveFileDialogNewPreset.ShowDialog() == DialogResult.OK)
            {
                PresetPath = saveFileDialogNewPreset.FileName;

                ClickSave(sender, e);
            }
        }

        private void DragDropPreset(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            PresetPath = files[0];
            Preset.Load(PresetPath);
            LoadPreset();
        }

        private void DragEnterPreset(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] drags = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (drags.All(f => File.Exists(f) && f.EndsWith("xml")))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }
    }
}
