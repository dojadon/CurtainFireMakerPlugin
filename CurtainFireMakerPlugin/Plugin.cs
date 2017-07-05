using System;
using System.Drawing;
using System.Windows.Forms;
using MikuMikuPlugin;
using CPmx;
using CPmx.Data;
using System.IO;
using System.Threading.Tasks;

namespace CurtainFireMakerPlugin
{
    public class Plugin : IHaveUserControl, ICommandPlugin
    {
        public static Plugin Instance { get; set; }

        private StreamWriter outStream;

        public Plugin()
        {
            Instance = this;

            this.outStream = new StreamWriter("log.txt", true, System.Text.Encoding.GetEncoding("Shift_JIS"));
            Console.SetOut(outStream);
            Console.WriteLine("start plugin");
        }

        public Guid GUID => new Guid();

        public IWin32Window ApplicationForm { get; set; }

        public Scene Scene { get; set; }

        public string Description => "Curtain Fire Maker Plugin";

        public string Text => "弾幕生成";

        public string EnglishText => "Generate Curtain Fire";

        public Image Image => null;

        public Image SmallImage => null;

        public PluginControl Control { get; set; }

        public UserControl CreateControl()
        {
            this.Control = new PluginControl(this.Scene);
            return this.Control;
        }

        public void Dispose()
        {
            this.outStream.Dispose();
        }

        public void Run(CommandArgs args)
        {
            World world = new World();

            PythonRunner.Init(this.Control.ReferenceScriptPath);
            PythonRunner.RunShotTypeScript(this.Control.ShotTypeScriptPath);
            PythonRunner.RunSpellScript(this.Control.SpellScriptPath, world);
            Console.WriteLine("run");
            world.StartWorld();

            this.ExportPmx(world);

            Console.WriteLine("finish");
        }

        private void ExportPmx(World world)
        {
            string exportPath = this.Control.ExportPath;
            File.Delete(exportPath);

            var pmxExporter = new PmxExporter(new FileStream(this.Control.ExportPath, FileMode.Create, FileAccess.Write));

            var data = new PmxModelData();
            world.model.GetData(data);

            pmxExporter.Export(data);
        }
    }
}
