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
    public partial class PresetSequenceEditorControl : UserControl
    {
        private Dictionary<string, FileSystemWatcher> FileWatcherDict { get; } = new Dictionary<string, FileSystemWatcher>();

        private List<string> Sequence { get; set; } = new List<string>();
        private int SelectedIndex { get => listBoxSequence.SelectedIndex; set => listBoxSequence.SelectedIndex = value; }
        private bool IsSelected => 0 <= SelectedIndex && SelectedIndex < Sequence.Count;

        public string SelectedFilePath => IsSelected ? Sequence[SelectedIndex] : "";

        private string SectedScriptText { set => textBoxSelectedScript.Text = value; }

        public List<string> RecentDirectories { get; set; }

        public PresetSequenceEditorControl()
        {
            InitializeComponent();

            Win32Wrapper.SetTabLength(textBoxSelectedScript.Handle, 16);
        }

        public void LoadConfig(PluginConfig config)
        {
            RecentDirectories = config.RecentScriptDirectories.ToList();
        }

        public void SaveConfig(PluginConfig config)
        {
            config.RecentScriptDirectories = RecentDirectories.Distinct().ToArray();
        }

        public void LoadPreset(Preset preset)
        {
            List<string> sequence = preset.SequenceScripts.ToList();
            Sequence = sequence.ToList();

            UpdateSequenceDataSource();

            SelectedIndex = Sequence.Count == 0 ? -1 : 0;
        }

        public void SavePreset(Preset preset)
        {
            preset.SequenceScripts = Sequence.ToArray();
        }

        private void AddScript(string path)
        {
            Sequence.Add(path);
            UpdateSequenceDataSource();
            SelectedIndex = Sequence.Count - 1;
        }

        private void MoveScript(int count)
        {
            if (!IsSelected || SelectedIndex + count >= Sequence.Count || 0 > SelectedIndex + count || Sequence.Count < 2) return;

            var script = SelectedFilePath;
            int index = SelectedIndex;

            Sequence[index] = Sequence[index + count];
            Sequence[index + count] = script;

            UpdateSequenceDataSource();

            SelectedIndex = index + count;
        }

        private void UpdateSequenceDataSource()
        {
            listBoxSequence.DataSource = null;
            listBoxSequence.DataSource = Sequence.Select(Path.GetFileNameWithoutExtension).ToList();

            List<string> directories = Sequence.Select(Path.GetDirectoryName).Select(Path.GetFullPath).ToList();

            foreach (string dir in directories.Where(d => !FileWatcherDict.ContainsKey(d)))
            {
                FileWatcherDict[dir] = CreateFileSystemWatcher(dir);
            }

            foreach (var pair in FileWatcherDict.Where(p => !directories.Any(d => d == p.Value.Path)).ToList())
            {
                pair.Value.EnableRaisingEvents = false;
                pair.Value.Dispose();
                FileWatcherDict.Remove(pair.Key);
            }
        }

        private FileSystemWatcher CreateFileSystemWatcher(string path)
        {
            var watcher = new FileSystemWatcher()
            {
                Path = path,
                Filter = "*.py",
                SynchronizingObject = this,
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite,
            };
            watcher.Deleted += FileDeleted;
            watcher.Renamed += FileRenamed;
            watcher.Changed += FileChanged;

            return watcher;
        }

        private void FileDeleted(object sender, FileSystemEventArgs e)
        {
            int idx = Sequence.IndexOf(e.FullPath);
            int newIdx = SelectedIndex < idx ? SelectedIndex : Math.Max(-1, SelectedIndex - 1);

            if (idx >= 0)
            {
                Sequence.RemoveAt(idx);

                UpdateSequenceDataSource();

                SelectedIndex = newIdx;
            }
        }

        private void FileRenamed(object sender, RenamedEventArgs e)
        {
            int idx = Sequence.IndexOf(e.OldFullPath);

            if (idx >= 0)
            {
                int newIdx = SelectedIndex;

                Sequence[idx] = e.FullPath;
                UpdateSequenceDataSource();

                SelectedIndex = newIdx;
            }
        }

        private void FileChanged(object sender, FileSystemEventArgs e)
        {
            if (SelectedFilePath == e.FullPath)
            {
                try
                {
                    SectedScriptText = File.ReadAllText(SelectedFilePath);
                }
                catch { }
            }
        }

        public void RunScript(ScriptEngine engine, ScriptScope scope)
        {
            Sequence.ForEach(s => engine.ExecuteFile(s, scope));
        }

        private void SelectedIndexChangedSequence(object sender, EventArgs e)
        {
            if (IsSelected)
            {
                SectedScriptText = File.ReadAllText(SelectedFilePath);
                labelPath.Text = SelectedFilePath;
            }
            else
            {
                SectedScriptText = labelPath.Text = "";
            }
        }

        private void ClickAdd(object sender, EventArgs e)
        {
            if (PresetEditorControl.ShowFileDialog(CreateOpenFileDialog(), out string path))
            {
                RecentDirectories.Add(path);
                AddScript(path);
            }
        }

        private void ClickRemove(object sender, EventArgs e)
        {
            if (!IsSelected) return;

            int index = SelectedIndex;
            Sequence.RemoveAt(SelectedIndex);
            UpdateSequenceDataSource();
            SelectedIndex = index - 1;

            if (index == 0 && Sequence.Count > 0)
            {
                SelectedIndex = 0;
            }
        }

        private void ClickUp(object sender, EventArgs e) => MoveScript(-1);
        private void ClickDown(object sender, EventArgs e) => MoveScript(1);

        private void CreateNewFile(object sender, EventArgs e)
        {
            if (PresetEditorControl.ShowFileDialog(CreateSaveFileDialog(), out string path))
            {
                File.WriteAllText(path, "# -*- coding: utf-8 -*-", System.Text.Encoding.UTF8);
                AddScript(path);
            }
        }

        private void OpenWithExplorer(object sender, EventArgs e)
        {
            if (!IsSelected) return;

            System.Diagnostics.Process.Start(Path.GetDirectoryName(SelectedFilePath));
        }

        private void OpenWithAtom(object sender, EventArgs e)
        {
            if (!IsSelected) return;

            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = "atom";
            process.StartInfo.Arguments = $"\"{SelectedFilePath}\"";
            process.Start();
        }

        private void DragDropSequence(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            Sequence.AddRange(files);

            UpdateSequenceDataSource();
            SelectedIndex = Sequence.Count - 1;
        }

        private void DragEnterSequence(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] drags = (string[])e.Data.GetData(DataFormats.FileDrop);

                if (drags.All(f => File.Exists(f) && f.EndsWith("py")))
                {
                    e.Effect = DragDropEffects.Copy;
                }
            }
        }

        private void ListBoxMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = listBoxSequence.IndexFromPoint(e.Location);

                if (index >= 0)
                {
                    listBoxSequence.ClearSelected();
                    listBoxSequence.SelectedIndex = index;
                }
            }
        }

        private void OpeningContextMenu(object sender, CancelEventArgs e)
        {
            ToolStripMenuItemRemove.Enabled = ToolStripMenuItemOpen.Enabled = IsSelected;
        }

        private void TextChangedScript(object sender, EventArgs e)
        {
            textBoxSelectedScript.Text = textBoxSelectedScript.Text.Replace("\r\n", "\r").Replace("\r", "\n").Replace("\n", "\r\n");
        }

        private Microsoft.Win32.OpenFileDialog CreateOpenFileDialog() => new Microsoft.Win32.OpenFileDialog()
        {
            Filter = "Python Script|*.py",
            DefaultExt = ".py",
            CustomPlaces = RecentDirectories.Select(s => new Microsoft.Win32.FileDialogCustomPlace(s)).ToList(),
        };

        private Microsoft.Win32.SaveFileDialog CreateSaveFileDialog() => new Microsoft.Win32.SaveFileDialog()
        {
            Filter = "Python Script|*.py",
            DefaultExt = ".py",
            CustomPlaces = RecentDirectories.Select(s => new Microsoft.Win32.FileDialogCustomPlace(s)).ToList(),
        };
    }
}
