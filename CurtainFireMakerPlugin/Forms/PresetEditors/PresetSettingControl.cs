using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CurtainFireMakerPlugin.Forms
{
    public partial class PresetSettingControl : UserControl, IPresetEditor
    {
        public int StartFrame { get => (int)numericUpDownStartFrame.Value; set => numericUpDownStartFrame.Value = value; }
        public int EndFrame { get => (int)numericUpDownEndFrame.Value; set => numericUpDownEndFrame.Value = value; }

        public bool BackGround { get => checkBoxBackGround.Checked; set => checkBoxBackGround.Checked = value; }

        public event EventHandler ValueChangedEvent;

        public PresetSettingControl()
        {
            InitializeComponent();
        }

        public void LoadPreset(Preset preset, string path)
        {
            StartFrame = preset.StartFrame;
            EndFrame = preset.EndFrame;
            BackGround = preset.BackGround;
        }

        public void SavePreset(Preset preset, string path)
        {
            preset.StartFrame = StartFrame;
            preset.EndFrame = EndFrame;
            preset.BackGround =BackGround;
        }

        public bool IsUpdated(Preset preset, string path)
        {
            return preset.StartFrame != StartFrame || preset.EndFrame != EndFrame || preset.BackGround != BackGround;
        }

        public void LoadConfig(PluginConfig config)
        {
        }

        public void SaveConfig(PluginConfig config)
        {
        }

        private void ValueChanged(object sender, EventArgs e)
        {
            ValueChangedEvent(this, EventArgs.Empty);
        }
    }
}
