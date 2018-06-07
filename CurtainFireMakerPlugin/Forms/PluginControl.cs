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

            HandleDestroyed += (sender, e) => CloseAll();
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

            var editor = new PresetEditorControl(path, Config) { Location = new Point(0, 0), Size = new Size(1000, 1000) };

            var page = new TabPage
            {
                Name = "page" + TabControl.TabPages.Count,
                Text = Path.GetFileNameWithoutExtension(path),
                Font = Font,
            };
            page.Controls.Add(editor);

            TabControl.TabPages.Add(page);
        }

        private void ClickNewFile(object sender, EventArgs e) => AddPage("新規");

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

        private void Save() => CurrentPresetEditor?.SaveAs(CreateSaveFileDialog());
        private void ClickSave(object sender, EventArgs e) => SaveAs();

        private void SaveAs() => CurrentPresetEditor?.SaveAs(CreateSaveFileDialog());
        private void ClickSaveAs(object sender, EventArgs e) => SaveAs();

        private void SaveAll()
        {
            foreach (TabPage p in TabControl.TabPages)
            {
                ((PresetEditorControl)p.Controls[0]).Save(CreateSaveFileDialog());
            }
        }
        private void ClickSaveAll(object sender, EventArgs e) => SaveAll();

        private bool Close(PresetEditorControl editor) => editor.CheckSave(CreateSaveFileDialog());

        private void Close()
        {
            if (CurrentPresetEditor != null && !Close(CurrentPresetEditor))
            {
                TabControl.TabPages.Remove(TabControl.SelectedTab);
            }
        }
        private void ClickClose(object sender, EventArgs e) => Close();

        private void CloseAll()
        {
            foreach (TabPage page in TabControl.TabPages)
            {
                if (!Close((PresetEditorControl)page.Controls[0])) TabControl.TabPages.Remove(page);
            }
        }
        private void ClickCloseAll(object sender, EventArgs e) => CloseAll();

        private void OnTabClosing(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = Close((PresetEditorControl)e.TabPage.Controls[0]);
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

        private void ClickRecordedTime(object sender, EventArgs e)
        {
            SaveConfig();

            MessageBox.Show(Convert(Config.TotalTime));

            string Convert(int i)
            {
                int h = i / 3600;
                i %= 3600;
                return $"{h}時間 { i / 60}分 { i %= 60}秒";
            }
        }
    }
}
