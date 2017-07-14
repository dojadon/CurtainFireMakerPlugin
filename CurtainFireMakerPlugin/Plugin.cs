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
using CurtainFireMakerPlugin.IO;
using System.Threading.Tasks;

namespace CurtainFireMakerPlugin
{
    public class Plugin : ICommandPlugin
    {
        public static Plugin Instance { get; set; }

        public bool IsPlugin { get; }
        public string CurtainFireMakerPath => Application.StartupPath + (IsPlugin ? "\\CurtainFireMaker" : "");

        public string ScriptPath { get; set; }
        public string ExportPmxPath { get; set; }
        public string ExportVmdPath { get; set; }
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }
        public bool KeepLogOpen { get; set; }

        public Plugin() : this(true)
        {
        }

        public Plugin(bool isPlugin = true)
        {
            Instance = this;
            IsPlugin = isPlugin;

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
        }

        public void Run(CommandArgs args)
        {
            if (IsPlugin)
            {
                var form = new ExportSettingForm();

                form.ScriptPath = this.ScriptPath;
                form.ExportPmx = this.ExportPmxPath;
                form.ExportVmd = this.ExportVmdPath;
                form.ModelName = this.ModelName;
                form.ModelDescription = this.ModelDescription;
                form.KeepLogOpen = this.KeepLogOpen;

                form.ShowDialog(this.ApplicationForm);

                if (form.DialogResult == DialogResult.OK)
                {
                    this.ScriptPath = form.ScriptPath;
                    this.ExportPmxPath = form.ExportPmx;
                    this.ExportVmdPath = form.ExportVmd;
                    this.ModelName = form.ModelName;
                    this.ModelDescription = form.ModelDescription;
                    this.KeepLogOpen = form.KeepLogOpen;

                    var progressForm = new ProgressForm();

                    this.RunScript(this.ScriptPath, progressForm);

                    progressForm.ShowDialog();
                }
            }
            else
            {
                var progressForm = new ProgressForm();

                this.RunScript(this.ScriptPath, progressForm);

                progressForm.ShowDialog();
            }
        }

        public void RunScript(string path, ProgressForm form)
        {
            Task task = new Task(() =>
            {
                var console = Console.Out;

                var writer = new ActionTextWriter(s => form.LogText += s);
                Console.SetOut(writer);

                World world = new World();

                PythonRunner.RunSpellScript(path, world);

                form.Progress.Minimum = 0;
                form.Progress.Maximum = World.MAX_FRAME;
                form.Progress.Step = 1;
                world.StartWorld(i => form.Progress.PerformStep());

                this.ExportVmd(world);
                this.ExportPmx(world);

                Console.SetOut(console);
                if (!this.KeepLogOpen)
                {
                    form.Close();
                }
            });
            task.Start();
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
