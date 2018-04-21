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
        private struct ScriptFile
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public string Script { get; set; }

            public ScriptFile(string name, string path, string script)
            {
                Name = name;
                Path = path;
                Script = script;
            }
        }

        private List<ScriptFile> ScriptSequence { get; set; } = new List<ScriptFile>();
        private int SequenceSelectedIndex { get => listBoxSequence.SelectedIndex; set => listBoxSequence.SelectedIndex = value; }
        private bool IsSequenceScriptSelected => 0 <= SequenceSelectedIndex && SequenceSelectedIndex < ScriptSequence.Count;
        private ScriptFile SelectedSequenceScript => ScriptSequence[SequenceSelectedIndex];

        public string SelectedScriptPath => IsSequenceScriptSelected ? SelectedSequenceScript.Path : "";
        public string RecentSelectedScriptPath
        {
            get => openFileDialogScript.FileName;
            set
            {
                openFileDialogScript.FileName = value;
                openFileDialogScript.InitialDirectory = Path.GetDirectoryName(value);
            }
        }

        private string SelectedScript
        {
            get => textBoxSelectedScript.Text;
            set => textBoxSelectedScript.Text = value.Replace("\r\n", "\r").Replace("\r", "\n").Replace("\n", "\r\n");
        }

        public PresetSequenceEditorControl()
        {
            InitializeComponent();

            SetTabLength(textBoxSelectedScript.Handle, 16);
        }

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, int[] lParam);
        private static void SetTabLength(IntPtr handle, int length) => SendMessage(handle, 0x00CB, 1, new int[] { length });

        public void LoadPreset(Preset preset)
        {
            List<string> sequence = preset.SequenceScripts.ToList();
            ScriptSequence = sequence.Select(s => CreateScript(s)).ToList();

            UpdateSequenceDataSource();

            SequenceSelectedIndex = ScriptSequence.Count == 0 ? -1 : 0;
        }

        public void SavePreset(Preset preset)
        {
            preset.SequenceScripts = ScriptSequence.Select(s => s.Path).ToArray();
        }

        public void UpdateSequenceDataSource()
        {
            listBoxSequence.DataSource = null;
            listBoxSequence.DataSource = ScriptSequence;
            listBoxSequence.DisplayMember = "Name";
            listBoxSequence.ValueMember = "Script";
        }

        private ScriptFile CreateScript(string path)
        {
            return new ScriptFile(Path.GetFileName(path), path, File.ReadAllText(path));
        }

        public void RunScript(ScriptEngine engine, ScriptScope scope)
        {
            ScriptSequence.ForEach(s => engine.ExecuteFile(s.Path, scope));
        }

        private void SelectedIndexChangedSequence(object sender, EventArgs e)
        {
            if (IsSequenceScriptSelected)
            {
                SelectedScript = SelectedSequenceScript.Script;
                labelPath.Text = SelectedSequenceScript.Path;
            }
            else
            {
                SelectedScript = labelPath.Text = "";
            }
        }

        private void ClickAdd(object sender, EventArgs e)
        {
            if (openFileDialogScript.ShowDialog() == DialogResult.OK)
            {
                ScriptSequence.Add(CreateScript(openFileDialogScript.FileName));
                UpdateSequenceDataSource();
                SequenceSelectedIndex = ScriptSequence.Count - 1;

                openFileDialogScript.InitialDirectory = Path.GetDirectoryName(openFileDialogScript.FileName);
            }
        }

        private void ClickRemove(object sender, EventArgs e)
        {
            if (!IsSequenceScriptSelected) return;

            int index = SequenceSelectedIndex;
            ScriptSequence.RemoveAt(SequenceSelectedIndex);
            UpdateSequenceDataSource();
            SequenceSelectedIndex = index - 1;

            if (index == 0 && ScriptSequence.Count > 0)
            {
                SequenceSelectedIndex = 0;
            }
        }

        private void ClickUp(object sender, EventArgs e)
        {
            if (!IsSequenceScriptSelected || SequenceSelectedIndex == 0 || ScriptSequence.Count < 2) return;

            var script = SelectedSequenceScript;
            int index = SequenceSelectedIndex;

            ScriptSequence[index] = ScriptSequence[index - 1];
            ScriptSequence[index - 1] = script;

            UpdateSequenceDataSource();

            SequenceSelectedIndex = index - 1;
        }

        private void ClickDown(object sender, EventArgs e)
        {
            if (!IsSequenceScriptSelected || SequenceSelectedIndex == ScriptSequence.Count - 1 || ScriptSequence.Count < 2) return;

            var script = SelectedSequenceScript;
            int index = SequenceSelectedIndex;

            ScriptSequence[index] = ScriptSequence[index + 1];
            ScriptSequence[index + 1] = script;

            UpdateSequenceDataSource();

            SequenceSelectedIndex = index + 1;
        }

        private void DragDropSequence(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

            ScriptSequence.AddRange(files.Select(CreateScript));

            UpdateSequenceDataSource();
            SequenceSelectedIndex = ScriptSequence.Count - 1;
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
    }
}
