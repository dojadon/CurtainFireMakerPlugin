using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurtainFireMakerPlugin.Forms
{
    interface IPresetEditor
    {
        void LoadPreset(Preset preset);
        void SavePreset(Preset preset);
        void LoadConfig(PluginConfig config);
        void SaveConfig(PluginConfig config);
        bool IsUpdated(Preset preset);

        event EventHandler ValueChangedEvent;
    }
}
