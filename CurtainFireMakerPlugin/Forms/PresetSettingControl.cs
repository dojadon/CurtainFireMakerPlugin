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
    public partial class PresetSettingControl : UserControl
    {
        public int StartFrame { get => (int)numericUpDownStartFrame.Value; set => numericUpDownStartFrame.Value = value; }
        public int EndFrame { get => (int)numericUpDownEndFrame.Value; set => numericUpDownEndFrame.Value = value; }

        public PresetSettingControl()
        {
            InitializeComponent();
        }

        public void LoadPreset(Preset preset)
        {
            StartFrame = preset.StartFrame;
            EndFrame = preset.EndFrame;
        }

        public void SavePreset(Preset preset)
        {
            preset.StartFrame = StartFrame;
            preset.EndFrame = EndFrame;
        }
    }
}
