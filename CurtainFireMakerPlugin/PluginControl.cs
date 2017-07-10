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
using System.IO;

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
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }

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
            control.SpellScript = this.SpellScriptPath;
            control.ModelDir = this.ModelDir;

            form.Controls.Add(control);
            form.Size = new Size(control.Size.Width, control.Size.Height + 40);
            form.Name = "入力設定";

            form.ShowDialog(this.ApplicationForm);

            if (form.DialogResult == DialogResult.OK)
            {
                this.SpellScriptPath = control.SpellScript;
                this.ModelDir = control.ModelDir;
            }
        }

        private void Click_ExportSetting(object sender, EventArgs e)
        {
            var form = new Form();

            var control = new ExportSettingControl(form);
            control.ExportPmx = this.ExportPmxPath;
            control.ExportVmd = this.ExportVmdPath;
            control.ModelName = this.ModelName;
            control.ModelDescription = this.ModelDescription;

            form.Controls.Add(control);
            form.Size = new Size(control.Size.Width, control.Size.Height + 40);
            form.Name = "出力設定";

            form.ShowDialog(this.ApplicationForm);

            if (form.DialogResult == DialogResult.OK)
            {
                this.ExportPmxPath = control.ExportPmx;
                this.ExportVmdPath = control.ExportVmd;
                this.ModelName = control.ModelName;
                this.ModelDescription = control.ModelDescription;
            }
        }

        private void Click_GenerateCurtainFire(object sender, EventArgs e)
        {
            Plugin.Instance.RunSpellScript(this.SpellScriptPath);
        }

        public void Export(BinaryWriter writer)
        {
            writer.Write(this.SpellScriptPath);
            writer.Write(this.ShotTypeScriptPath);
            writer.Write(this.ReferenceScriptPath);
            writer.Write(this.ModelDir);
            writer.Write(this.ExportPmxPath);
            writer.Write(this.ExportVmdPath);
            writer.Write(this.ModelName);
            writer.Write(this.ModelDescription);
        }

        public void Parse(BinaryReader reader)
        {
            this.SpellScriptPath = reader.ReadString();
            this.ShotTypeScriptPath = reader.ReadString();
            this.ReferenceScriptPath = reader.ReadString();
            this.ModelDir = reader.ReadString();
            this.ExportPmxPath = reader.ReadString();
            this.ExportVmdPath = reader.ReadString();
            this.ModelName = reader.ReadString();
            this.ModelDescription = reader.ReadString();
        }
    }
}
