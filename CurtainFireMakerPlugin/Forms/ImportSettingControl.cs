using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MikuMikuPlugin;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class ImportSettingControl : UserControl
    {
        public Scene Scene { get; set; }
        public Form Form { get; }

        public string ModelDir { get { return this.shotModelText.Text; } set { this.shotModelText.Text = value; } }
        public string SpellScript { get { return this.spellScriptText.Text; } set { this.spellScriptText.Text = value; } }

        public ImportSettingControl(Form form, Scene scene)
        {
            InitializeComponent();

            this.Form = form;
            this.Scene = scene;
        }

        private void Click_OK(object sender, EventArgs e)
        {
            this.Form.Close();
            this.Form.DialogResult = DialogResult.OK;
        }

        private void Click_Cancel(object sender, EventArgs e)
        {
            this.Form.Close();
            this.Form.DialogResult = DialogResult.Cancel;
        }

        private void Click_ShotModel(object sender, EventArgs e)
        {
            DialogResult result = this.modelDirBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.shotModelText.Text = this.modelDirBrowserDialog.SelectedPath;
            }
        }

        private void Click_SpellScript(object sender, EventArgs e)
        {
            DialogResult result = this.spellScriptFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.spellScriptText.Text = this.spellScriptFileDialog.FileName;
            }
        }
    }
}
