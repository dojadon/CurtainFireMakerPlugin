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
    public partial class ExportSettingControl : UserControl
    {
        public Form Form { get; set; }

        public string ScriptPath { get { return this.scriptText.Text; } set { this.scriptText.Text = value; } }
        public string ModelName { get { return this.modelNameText.Text; } set { this.modelNameText.Text = value; } }
        public string ModelDescription { get { return this.modelDescriptionText.Text; } set { this.modelDescriptionText.Text = value; } }
        public string ExportPmx { get { return this.exportPmxText.Text; } set { this.exportPmxText.Text = value; } }
        public string ExportVmd { get { return this.exportVmdText.Text; } set { this.exportVmdText.Text = value; } }

        public ExportSettingControl(Form form)
        {
            InitializeComponent();

            this.Form = form;
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

        private void Click_ExportPmx(object sender, EventArgs e)
        {
            var result = this.pmxFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.exportPmxText.Text = this.pmxFileDialog.FileName;
            }
        }

        private void Click_ExportVmd(object sender, EventArgs e)
        {
            var result = this.vmdFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.exportVmdText.Text = this.vmdFileDialog.FileName;
            }
        }

        private void Click_Script(object sender, EventArgs e)
        {
            var result = this.scriptFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                this.scriptText.Text = this.scriptFileDialog.FileName;
            }
        }
    }
}
