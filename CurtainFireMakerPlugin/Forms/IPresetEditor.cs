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
        bool IsUpdated(Preset preset);
    }
}
