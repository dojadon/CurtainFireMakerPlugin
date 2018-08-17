using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class SelectPresetForm : Form
    {
        public PresetEditorControl CurrentPreset => Presets[listBoxPresets.SelectedIndex];

        public List<PresetEditorControl> Presets { get; }

        public SelectPresetForm(IEnumerable<PresetEditorControl> controls)
        {
            InitializeComponent();

            Presets = controls.ToList();
            listBoxPresets.DataSource = controls;
            listBoxPresets.DisplayMember = "FileName";
        }

        private void ClickCancel(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void ClickExecute(object sender, EventArgs e)
        {
            DialogResult = (0 <= listBoxPresets.SelectedIndex && listBoxPresets.SelectedIndex < Presets.Count) ? DialogResult.OK : DialogResult.Cancel;
            Close();
        }
    }
}
