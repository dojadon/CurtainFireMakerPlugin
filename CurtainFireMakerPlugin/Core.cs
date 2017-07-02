using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DxMath;
using MikuMikuPlugin;

namespace CurtainFireMakerPlugin
{
    public class Core : IHaveUserControl
    {
        public Guid GUID => new Guid();

        public IWin32Window ApplicationForm { get; set; }

        public Scene Scene { get; set; }

        public UserControl CreateControl()
        {
            return null;
        }

        public void Dispose()
        {
        }
    }
}
