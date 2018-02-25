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
        public const string RootKey = "Root";

        public Dictionary<string, string> ScriptDict { get; } = new Dictionary<string, string>();

        public string SelectedScript => textBoxScript.Text;

        public string RootScript => ScriptDict[RootKey];

        public ProjectScriptControl(string rootScript)
        {
            InitializeComponent();

            AddScript(RootKey, rootScript);
            comboBoxScriptKey.SelectedIndex = 0;
        }

        public void AddScript(string key, string script)
        {
            ScriptDict[key] = script.Replace("\r\n", "\n").Replace("\n", "\r").Replace("\r", "\r\n");

            if (!comboBoxScriptKey.Items.Contains(key))
            {
                comboBoxScriptKey.Items.Add(key);
            }
        }

        public string GetScript(string key)
        {
            if (!ScriptDict.ContainsKey(key))
            {
                AddScript(key, RootScript);
            }
            return ScriptDict[key];
        }

        private void TextChanges_ScriptKey(object sender, EventArgs e)
        {
            textBoxScript.Text = ScriptDict[comboBoxScriptKey.Text];
        }

        private void TextChanged_Script(object sender, EventArgs e)
        {
            ScriptDict[comboBoxScriptKey.Text] = textBoxScript.Text;
        }
    }
}
