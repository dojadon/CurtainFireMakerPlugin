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
        public ListBox[] Indices { get; set; }
        public Scene Scene { get; set; }

        public PluginControl(Scene scene)
        {
            InitializeComponent();

            this.Scene = scene;
            this.Indices = new ListBox[] { listBox1, listBox2, listBox3, listBox4, listBox5, listBox6, listBox7, listBox8 };
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

        private void ListBox_Click(object sender, MouseEventArgs e)
        {
            Array.ForEach(this.Indices, t => t.Items.Clear());
            foreach (Model model in this.Scene.Models)
            {
                Array.ForEach(this.Indices, t => t.Items.Add(model.Name));
            }
        }
    }
}
