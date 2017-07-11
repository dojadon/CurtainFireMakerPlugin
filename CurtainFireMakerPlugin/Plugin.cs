using System;
using System.Drawing;
using System.Windows.Forms;
using MikuMikuPlugin;
using CsPmx;
using CsPmx.Data;
using CsVmd;
using CsVmd.Data;
using System.IO;
using CurtainFireMakerPlugin.Forms;

namespace CurtainFireMakerPlugin
{
    public class Plugin : ICommandPlugin
    {
        public static Plugin Instance { get; set; }

        private StreamWriter outStream;

        public string CurtainFireMakerPath { get; } = Application.StartupPath + @"\CurtainFireMaker";

        public string ScriptPath { get; set; }
        public string ExportPmxPath { get; set; }
        public string ExportVmdPath { get; set; }
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }

        public Plugin()
        {
            Instance = this;

            this.outStream = new StreamWriter("log.txt", true, System.Text.Encoding.GetEncoding("Shift_JIS"));
            Console.SetOut(outStream);
            Console.WriteLine("start plugin");

            Configuration.Load();
        }

        public Guid GUID => new Guid();
        public IWin32Window ApplicationForm { get; set; }
        public Scene Scene { get; set; }

        public string Description => "Curtain Fire Maker Plugin";
        public string Text => "弾幕生成";
        public string EnglishText => "Generate Curtain Fire";

        public Image Image => null;
        public Image SmallImage => null;

        public void Dispose()
        {
            Configuration.Save();

            this.outStream.Dispose();
        }

        public void Run(CommandArgs args)
        {
            var form = new ExportSettingForm();

            form.ScriptPath = this.ScriptPath;
            form.ExportPmx = this.ExportPmxPath;
            form.ExportVmd = this.ExportVmdPath;
            form.ModelName = this.ModelName;
            form.ModelDescription = this.ModelDescription;

            form.ShowDialog(this.ApplicationForm);

            if (form.DialogResult == DialogResult.OK)
            {
                this.ScriptPath = form.ScriptPath;
                this.ExportPmxPath = form.ExportPmx;
                this.ExportVmdPath = form.ExportVmd;
                this.ModelName = form.ModelName;
                this.ModelDescription = form.ModelDescription;

                this.RunSpellScript(this.ScriptPath);
            }
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
            string exportPath = this.ExportPmxPath;
            File.Delete(exportPath);

            var exporter = new PmxExporter(new FileStream(exportPath, FileMode.Create, FileAccess.Write));

            var data = new PmxModelData();
            world.model.GetData(data);

            data.Header.modelName = this.ModelName;
            data.Header.description += this.ModelDescription;

            exporter.Export(data);
        }

        private void ExportVmd(World world)
        {
            string exportPath = this.ExportVmdPath;
            File.Delete(exportPath);

            var exporter = new VmdExporter(new FileStream(exportPath, FileMode.Create, FileAccess.Write));

            var data = new VmdMotionData();
            world.motion.GetData(data);

            data.Header.modelName = this.ModelName;

            exporter.Export(data);
        }
    }
}
