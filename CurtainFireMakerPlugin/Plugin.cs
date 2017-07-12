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
        public string CurtainFireMakerPath => Application.StartupPath + (IsPlugin ? @"\CurtainFireMaker" : "");

        public string ScriptPath { get; set; }
        public string ExportPmxPath { get; set; }
        public string ExportVmdPath { get; set; }
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }

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

                form.ShowDialog(this.ApplicationForm);

                if (form.DialogResult == DialogResult.OK)
                {
                    this.ScriptPath = form.ScriptPath;
                    this.ExportPmxPath = form.ExportPmx;
                    this.ExportVmdPath = form.ExportVmd;
                    this.ModelName = form.ModelName;
                    this.ModelDescription = form.ModelDescription;

                    var progressForm = new ProgressForm();
                    var writer = new ActionTextWriter(s => progressForm.LogText += s);
                    Console.SetOut(writer);

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
                World world = new World();

                PythonRunner.RunSpellScript(path, world);

                form.Progress.Minimum = 0;
                form.Progress.Maximum = World.MAX_FRAME;
                form.Progress.Step = 1;
                world.StartWorld(i => form.Progress.PerformStep());

                this.ExportPmx(world);
                this.ExportVmd(world);

                Console.SetOut(Console.Out);
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

            Console.WriteLine("出力完了");
            Console.WriteLine("頂点数 : " + data.VertexArray.Length);
            Console.WriteLine("材質数 : " + data.MaterialArray.Length);
            Console.WriteLine("ボーン数 : " + data.BoneArray.Length);
            Console.WriteLine("モーフ数 : " + data.MorphArray.Length);
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
