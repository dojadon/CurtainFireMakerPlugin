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

        private PluginConfig Config { get; }
        private Preset Preset { get; } = new Preset();

        private List<string> RecentDirectories { get; set; } = new List<string>();
        private string InitialDirectory { get; set; }

        private IPresetEditor[] PresetEditors { get; }
        private bool IsUpdated => PresetEditors.Any(c => c.IsUpdated(Preset));

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
                    InitialDirectory = Path.GetDirectoryName(PresetPath);
                }
            }
        }

        public string ExportDirectory => Path.GetDirectoryName(PresetPath);
        public string FileName => Path.GetFileNameWithoutExtension(PresetPath);

        public int StartFrame => PresetSettingControl.StartFrame;
        public int EndFrame => PresetSettingControl.EndFrame;

        private long LastTime { get; set; } = Environment.TickCount;

        public PresetEditorControl(PluginConfig config)
        {
            Config = config;
            Preset.Init();

            InitializeComponent();

            PresetEditors = new IPresetEditor[] { PresetSequenceEditorControl, PresetSettingControl };

            PresetPath = "新規";
            LoadConfig();

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
            RecentDirectories = Config.RecentPresetDirectories.ToList();
            InitialDirectory = Path.GetDirectoryName(Config.RecentSelectedPresetPath);
        }

        private void SaveConfig()
        {
            PresetSequenceEditorControl.SaveConfig(Config);
            Config.RecentPresetDirectories = RecentDirectories.Distinct().ToArray();
            Config.RecentSelectedPresetPath = PresetPath;
            Config.TotalTime += (int)(Environment.TickCount - LastTime) / 1000;
            LastTime = Environment.TickCount;
        }

        private void LoadPreset()
        {
            PresetEditors.ForEach(c => c.LoadPreset(Preset));

            if (File.Exists(PresetPath))
            {
                RecentDirectories.Add(Path.GetDirectoryName(PresetPath));
            }
        }

        private void SavePreset()
        {
            PresetEditors.ForEach(c => c.SavePreset(Preset));
        }

        public void RunScript(ScriptEngine engine, ScriptScope scope)
        {
            PresetSequenceEditorControl.RunScript(engine, scope);
        }

        private bool CheckSave()
        {
            if (IsUpdated)
            {
                var result = MessageBox.Show("保存されてない変更があります。\r\n変更を保存しますか？", "閉じる", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Exclamation);

                switch (result)
                {
                    case DialogResult.Yes:
                        ClickSave(this, EventArgs.Empty);
                        break;

                    case DialogResult.No:
                        break;

                    case DialogResult.Cancel:
                        return true;
                }
            }
            return false;
        }

        private void ClickNewFile(object sender, EventArgs e)
        {
            if (CheckSave())
            {
                return;
            }

            Preset.Init();
            LoadPreset();
            PresetPath = "新規";
        }

        private void ClickOpen(object sender, EventArgs e)
        {
            if (!CheckSave() && ShowFileDialog(CreateOpenFileDialog(), out string path))
            {
                if (Preset.IsFormated(path))
                {
                    RecentDirectories.Add(path);
                    PresetPath = path;
                    Preset.Load(PresetPath);
                    LoadPreset();
                }
                else
                {
                    MessageBox.Show($"Format Error：{path}");
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
            if (ShowFileDialog(CreateSaveFileDialog(), out string path))
            {
                PresetPath = path;
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

        private Microsoft.Win32.OpenFileDialog CreateOpenFileDialog() => new Microsoft.Win32.OpenFileDialog()
        {
            Filter = "Xml File|*.xml",
            DefaultExt = ".xml",
            InitialDirectory = InitialDirectory,
            CustomPlaces = RecentDirectories.Select(s => new Microsoft.Win32.FileDialogCustomPlace(s)).ToList(),
        };

        private Microsoft.Win32.SaveFileDialog CreateSaveFileDialog() => new Microsoft.Win32.SaveFileDialog()
        {
            Filter = "Xml File|*.xml",
            DefaultExt = ".xml",
            InitialDirectory = InitialDirectory,
            FileName = Path.IsPathRooted(PresetPath) ? PresetPath : $"{InitialDirectory}\\{Path.GetFileNameWithoutExtension(PresetSequenceEditorControl.SelectedFilePath)}.xml",
            CustomPlaces = RecentDirectories.Select(s => new Microsoft.Win32.FileDialogCustomPlace(s)).ToList(),
        };

        public static bool ShowFileDialog(Microsoft.Win32.FileDialog dialog, out string path)
        {
            if (dialog.ShowDialog() ?? false)
            {
                path = dialog.FileName;
                return true;
            }
            else
            {
                path = "";
                return false;
            }
        }

        private void ClickRecordedTime(object sender, EventArgs e)
        {
            SaveConfig();

            MessageBox.Show(Convert(Config.TotalTime), "記録時間");

            string Convert(int i)
            {
                int h, m, s;

                h = i / 3600;
                i %= 3600;

                m = i / 60;
                i %= 60;

                s = i;

                return $"{h}時間 {m}分 {s}秒";
            }
        }
    }
}
