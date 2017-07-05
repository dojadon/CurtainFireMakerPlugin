using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MikuMikuPlugin;

namespace CurtainFireMakerPlugin
{
    public partial class PluginControl : UserControl
    {
        public Scene Scene { get; set; }

        public string ReferenceScriptPath => this.textBox2.Text + "\\reference.py";
        public string SpellScriptPath => this.textBox2.Text + "\\spell.py";
        public string ShotTypeScriptPath => this.textBox2.Text + "\\shottype.py";
        public string ModelDir => this.textBox1.Text + "\\import\\resource";
        public string ExportPath => this.textBox4.Text;

        public PluginControl(Scene scene)
        {
            InitializeComponent();

            this.Scene = scene;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                this.textBox1.Text = this.folderBrowserDialog1.SelectedPath;
                this.textBox2.Text = this.textBox1.Text + "\\import\\script";
                this.textBox4.Text = this.textBox1.Text + "\\export\\export.pmx";
            }
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            DialogResult result = this.folderBrowserDialog2.ShowDialog();

            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                this.textBox2.Text = this.folderBrowserDialog2.SelectedPath;
            }
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            DialogResult result = this.openFileDialog1.ShowDialog();

            if (result == DialogResult.OK || result == DialogResult.Yes)
            {
                this.textBox4.Text = this.openFileDialog1.SafeFileName;
            }
        }
    }
}
