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
    public class Plugin : IHaveUserControl, ICommandPlugin
    {
        public Guid GUID => new Guid();

        public IWin32Window ApplicationForm { get; set; }

        public Scene Scene { get; set; }

        public string Description => "Curtain Fire Maker Plugin";

        public string Text => "弾幕生成";

        public string EnglishText => "Generate Curtain Fire";

        public Image Image => null;

        public Image SmallImage => null;

        public UserControl CreateControl()
        {
            return new PluginControl(this.Scene);
        }

        public void Dispose()
        {
        }

        public void Run(CommandArgs e)
        {

        }
    }
}
