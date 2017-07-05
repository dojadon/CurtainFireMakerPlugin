using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Forms;

namespace CurtainFireMakerPlugin
{
    public partial class PluginControl : UserControl
    {
        public Scene Scene { get; set; }
        public IWin32Window ApplicationForm { get; }

        public string SpellScriptPath { get; set; }
        public string ShotTypeScriptPath { get; set; }
        public string ReferenceScriptPath { get; set; }
        public string ModelDir { get; set; }
        public string ExportPmxPath { get; set; }
        public string ExportVmdPath { get; set; }

        public PluginControl(IWin32Window form, Scene scene)
        {
            InitializeComponent();

            this.ApplicationForm = form;
            this.Scene = scene;
        }

        private void Click_ImportSetting(object sender, EventArgs e)
        {
            var form = new Form();

            var control = new ImportSettingControl(form, this.Scene);
            form.Controls.Add(control);
            form.Size = control.Size;

            form.Show(this.ApplicationForm);

            if(form.DialogResult == DialogResult.OK)
            {
                this.SpellScriptPath = control.SpellScript;
                this.ShotTypeScriptPath = control.ShotTypeScript;
                this.ReferenceScriptPath = control.ReferenceScript;
                this.ModelDir = control.ModelDir;
            }
        }
    }
}
