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
        public static Plugin Instance { get; set; }

        public Plugin()
        {
            Instance = this;
        }

        public Guid GUID => new Guid();

        public IWin32Window ApplicationForm { get; set; }

        public Scene Scene { get; set; }

        public string Description => "Curtain Fire Maker Plugin";

        public string Text => "弾幕生成";

        public string EnglishText => "Generate Curtain Fire";

        public Image Image => null;

        public Image SmallImage => null;

        public UserControl Control { get; set; }

        public UserControl CreateControl()
        {
            this.Control = new PluginControl(this.Scene);
            return this.Control;
        }

        public void Dispose()
        {
        }

        public void Run(CommandArgs e)
        {

        }
    }
}
