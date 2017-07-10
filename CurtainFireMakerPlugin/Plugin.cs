using System;
using System.Drawing;
using System.Windows.Forms;
using MikuMikuPlugin;
using CsPmx;
using CsPmx.Data;
using CsVmd;
using CsVmd.Data;
using System.IO;

namespace CurtainFireMakerPlugin
{
    public class Plugin : IHaveUserControl, ICommandPlugin, ICanSavePlugin
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
            PythonRunner.Init();

            this.Control = new PluginControl(this.ApplicationForm, this.Scene);
            return this.Control;
        }

        public void Dispose()
        {
            this.outStream.Dispose();
        }

        public void Run(CommandArgs args)
        {
            this.RunSpellScript(this.Control.SpellScriptPath);
        }

        public void RunSpellScript(string path)
        {
            World world = new World();

            PythonRunner.RunSpellScript(path, world);

            world.StartWorld();

            this.ExportPmx(world);
            this.ExportVmd(world);
        }

        private void ExportPmx(World world)
        {
            string exportPath = this.Control.ExportPmxPath;
            File.Delete(exportPath);

            var exporter = new PmxExporter(new FileStream(exportPath, FileMode.Create, FileAccess.Write));

            var data = new PmxModelData();
            world.model.GetData(data);

            data.Header.modelName = this.Control.ModelName;
            data.Header.description += this.Control.ModelDescription;

            exporter.Export(data);
        }

        private void ExportVmd(World world)
        {
            string exportPath = this.Control.ExportVmdPath;
            File.Delete(exportPath);

            var exporter = new VmdExporter(new FileStream(exportPath, FileMode.Create, FileAccess.Write));

            var data = new VmdMotionData();
            world.motion.GetData(data);

            data.Header.modelName = this.Control.ModelName;

            exporter.Export(data);
        }

        public Stream OnSaveProject()
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            this.Control.Export(writer);

            return stream;
        }

        public void OnLoadProject(Stream stream)
        {
            var reader = new BinaryReader(stream);

            this.Control.Parse(reader);
        }
    }
}
