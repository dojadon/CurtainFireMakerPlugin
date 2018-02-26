using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class ProjectScriptControl : UserControl
    {
        public Dictionary<string, string> ScriptDict { get; } = new Dictionary<string, string>();

        public string SelectedScript => textBoxScript.Text;
        public string DefaultScript { get; }

        public string RootScript
        {
            get => textBoxRootScript.Text;
            set
            {
                textBoxRootScript.Text = value.Replace("\r\n", "\n").Replace("\n", "\r").Replace("\r", "\r\n");
            }
        }

        public ProjectScriptControl(string defaultScript)
        {
            InitializeComponent();

            DefaultScript = defaultScript;

            buttonRemove.ForeColor = Color.Black;
            SwitchEnable();
        }

        public void AddScript(string key, string script)
        {
            ScriptDict[key] = script.Replace("\r\n", "\n").Replace("\n", "\r").Replace("\r", "\r\n");

            if (!comboBoxScriptKey.Items.Contains(key))
            {
                comboBoxScriptKey.Items.Add(key);

                if (comboBoxScriptKey.Items.Count == 1)
                {
                    comboBoxScriptKey.SelectedIndex = 0;
                }
            }
            SwitchEnable();
        }

        public void RemoveScript(string key)
        {
            comboBoxScriptKey.Items.Remove(key);
            ScriptDict.Remove(key);

            if (comboBoxScriptKey.Items.Count == 0)
            {
                textBoxScript.Text = "";
            }
            SwitchEnable();
        }

        public void SwitchEnable()
        {
            bool enabled = comboBoxScriptKey.Items.Count > 0;

            comboBoxScriptKey.Enabled = enabled;
            textBoxScript.Enabled = enabled;
            buttonRemove.Enabled = enabled;
        }

        public string GetScript(string key)
        {
            if (!ScriptDict.ContainsKey(key))
            {
                return DefaultScript;
            }
            else
            {
                return ScriptDict[key];
            }
        }

        private void TextChanges_ScriptKey(object sender, EventArgs e)
        {
            if (ScriptDict.ContainsKey(comboBoxScriptKey.Text))
            {
                textBoxScript.Text = ScriptDict[comboBoxScriptKey.Text];
            }
        }

        private void TextChanged_Script(object sender, EventArgs e)
        {
            if (ScriptDict.ContainsKey(comboBoxScriptKey.Text))
            {
                ScriptDict[comboBoxScriptKey.Text] = textBoxScript.Text;
            }
        }

        private void Click_Remove(object sender, EventArgs e)
        {
            RemoveScript(comboBoxScriptKey.Text);
        }
    }
}
