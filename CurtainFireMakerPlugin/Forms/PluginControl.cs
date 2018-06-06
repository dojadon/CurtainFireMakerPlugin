using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Scripting.Hosting;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class PluginControl : UserControl
    {
        private PluginConfig Config { get; }

        private List<string> RecentDirectories { get; set; } = new List<string>();
        private string InitialDirectory { get; set; }

        private long LastTime { get; set; } = Environment.TickCount;

        private PresetEditorControl CurrentPresetEditor => (PresetEditorControl)TabControl.SelectedTab?.Controls[0];
        private PresetEditorControl GetPresetEditor(int idx) => (PresetEditorControl)TabControl.TabPages[idx].Controls[0];

        public bool IsSelectPreset => TabControl.SelectedTab != null;

        public string ExportDirectory => CurrentPresetEditor.ExportDirectory;
        public string FileName => CurrentPresetEditor.FileName;

        public int StartFrame => CurrentPresetEditor.StartFrame;
        public int EndFrame => CurrentPresetEditor.EndFrame;

        public PluginControl(PluginConfig config)
        {
            Config = config;
            LoadConfig();

            InitializeComponent();

            TabControl.DisplayStyleProvider.ShowTabCloser = true;
        }

        private void LoadConfig()
        {
            RecentDirectories = Config.RecentPresetDirectories.ToList();
        }

        private void SaveConfig()
        {
            Config.RecentPresetDirectories = RecentDirectories.Distinct().Where(Directory.Exists).ToArray();
            Config.TotalTime += (int)(Environment.TickCount - LastTime) / 1000;
            LastTime = Environment.TickCount;
        }

        public void Save()
        {
            SaveConfig();
        }

        public void RunScript(ScriptEngine engine, ScriptScope scope)
        {
            CurrentPresetEditor.RunScript(engine, scope);
        }

        private void AddPage(string path)
        {
            foreach (TabPage p in TabControl.TabPages)
            {
                if (((PresetEditorControl)p.Controls[0]).PresetPath == path) return;
            }

            var editor = new PresetEditorControl(path, Config)
            {
                Location = new Point(0, 0),
                Size = new Size(1000, 1000),
            };

            var page = new TabPage
            {
                Name = "page" + TabControl.TabPages.Count,
                Text = Path.GetFileNameWithoutExtension(path),
                Font = Font,
            };
            page.Controls.Add(editor);

            TabControl.TabPages.Add(page);
        }

        private void ClickNewFile(object sender, EventArgs e)
        {
            AddPage("新規");
        }

        private void ClickOpen(object sender, EventArgs e)
        {
            if (ShowFileDialog(CreateOpenFileDialog(), out string path))
            {
                if (Preset.IsFormated(path))
                {
                    RecentDirectories.Add(Path.GetDirectoryName(path));
                    AddPage(path);
                }
                else
                {
                    MessageBox.Show($"Format Error：{path}");
                }
            }
        }

        private void ClickSave(object sender, EventArgs e)
        {
            CurrentPresetEditor?.Save(CreateSaveFileDialog());
        }

        private void ClickSaveAs(object sender, EventArgs e)
        {
            CurrentPresetEditor?.SaveAs(CreateSaveFileDialog());
        }

        private void ClickSaveAll(object sender, EventArgs e)
        {
            foreach (TabPage p in TabControl.TabPages)
            {
                ((PresetEditorControl)p.Controls[0]).Save(CreateSaveFileDialog());
            }
        }

        private void ClickClose(object sender, TabControlCancelEventArgs e)
        {
            var editor = (PresetEditorControl)e.TabPage.Controls[0];

            if (editor.CheckSave(CreateSaveFileDialog()))
            {
                e.Cancel = true;
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
            FileName = CurrentPresetEditor.SavePresetFileName,
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
    }
}
