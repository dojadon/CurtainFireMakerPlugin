using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class PreScriptEditorControl : UserControl
    {
        public ProjectEditorControl ProjectEditor { get; set; }

        public List<ScriptFile> PreScripts => ProjectEditor.PreScripts;

        public int SelectedScriptIndex => ComboBoxScript.SelectedIndex;
        public bool IsScriptSelected => 0 <= ComboBoxScript.SelectedIndex && ComboBoxScript.SelectedIndex < PreScripts.Count;
        public ScriptFile SelectedPreScript => PreScripts[ComboBoxScript.SelectedIndex];

        public PreScriptEditorControl()
        {
            InitializeComponent();
        }

        public void UpdateDataSource()
        {
            if (!IsScriptSelected)
            {
                ComboBoxScript.SelectedIndex = -1;
            }

            ComboBoxScript.DataSource = null;
            ComboBoxScript.DataSource = PreScripts;
            ComboBoxScript.DisplayMember = "FileName";

            bool enabled = PreScripts.Count > 0;

            ComboBoxScript.Enabled = enabled;
            TextBoxPreScript.Enabled = enabled;
            buttonRemove.Enabled = enabled;
            button1.Enabled = enabled;
        }

        public void UpdatePreScript(string path, string script)
        {
            if (PreScripts.Any(s => s.Path == path))
            {
                PreScripts.First(s => s.Path == path).PreScript = script;
            }
            else
            {
                PreScripts.Add(new ScriptFile(path) { PreScript = script });

                UpdateDataSource();

                if (PreScripts.Count == 1)
                {
                    ComboBoxScript.SelectedIndex = 0;
                }
            }
        }

        public string GetPreScript(string path)
        {
            if (PreScripts.Any(s => s.Path == path))
            {
                return PreScripts.FirstOrDefault(s => s.Path == path).PreScript;
            }
            return Project.DefaultPreScript;
        }

        private void SelectedScriptIndexChanged(object sender, EventArgs e)
        {
            if (IsScriptSelected)
            {
                TextBoxPreScript.Text = SelectedPreScript.PreScript;
            }
            else
            {
                TextBoxPreScript.Text = "";
            }
        }

        private void LeaveTextBoxPreScript(object sender, EventArgs e)
        {
            if (IsScriptSelected) SelectedPreScript.PreScript = TextBoxPreScript.Text;
        }

        private void ClickRemoveScript(object sender, EventArgs e)
        {
            int index = SelectedScriptIndex;

            PreScripts.Remove(SelectedPreScript);

            UpdateDataSource();

            if (PreScripts.Count == 0)
            {
                TextBoxPreScript.Text = "";
            }
            else
            {
                ComboBoxScript.SelectedIndex = index;
            }
        }

        private void ClickGenerate(object sender, EventArgs e)
        {
            ProjectEditor.GenerateCurtainFire();
        }
    }
}
