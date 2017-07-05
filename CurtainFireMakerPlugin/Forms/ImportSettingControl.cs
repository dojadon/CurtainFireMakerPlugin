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

        public string ModelDir { get; set; }
        public string SpellScript { get; set; }
        public string ShotTypeScript { get; set; }
        public string ReferenceScript { get; set; }

        public ImportSettingControl(Form form, Scene scene)
        {
            InitializeComponent();

            this.Form = form;
            this.Scene = scene;
        }

        private void Click_OK(object sender, EventArgs e)
        {
            this.Form.DialogResult = DialogResult.OK;
            this.Form.Close();
        }

        private void Click_Cancel(object sender, EventArgs e)
        {
            this.Form.DialogResult = DialogResult.Cancel;
            this.Form.Close();
        }

        private void Click_ShotModel(object sender, EventArgs e)
        {
            DialogResult result = this.modelDirBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.ModelDir = this.modelDirBrowserDialog.SelectedPath;
            }
        }

        private void Click_SpellScript(object sender, EventArgs e)
        {
            DialogResult result = this.spellScriptFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.SpellScript = this.spellScriptFileDialog.FileName;
            }
        }

        private void Click_ShotTypeScript(object sender, EventArgs e)
        {
            DialogResult result = this.shottypeScriptFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.ShotTypeScript = this.shottypeScriptFileDialog.FileName;
            }
        }

        private void Click_ReferenceScript(object sender, EventArgs e)
        {
            DialogResult result = this.referenceScriptFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                this.ReferenceScript = this.referenceScriptFileDialog.FileName;
            }
        }
    }
}
