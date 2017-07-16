using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MikuMikuPlugin;
using CsPmx;
using CsPmx.Data;
using CsVmd;
using CsVmd.Data;
using System.IO;
using CurtainFireMakerPlugin.Forms;
using System.Threading.Tasks;

namespace CurtainFireMakerPlugin
{
    public class Plugin : ICommandPlugin
    {
        public static Plugin Instance { get; set; }

        public bool IsPlugin { get; }
        public string CurtainFireMakerPath => Application.StartupPath + (IsPlugin ? "\\CurtainFireMaker" : "");

        public string ScriptFileName
        {
            get
            {
                string[] split = ScriptPath.Split('\\');
                return split[split.Length - 1];
            }
        }
        public string ScriptPath { get; set; }
        public string SettingScriptPath { get; set; }
        public string ExportPmxPath { get; set; }
        public string ExportVmdPath { get; set; }
        public string ExportDirPath { get; set; }
        public string ModelName { get; set; }
        public string ModelDescription { get; set; }
        public bool KeepLogOpen { get; set; }

        private bool running;
        public bool Running { get; }

        public Plugin() : this(true)
        {
        }

        public Plugin(bool isPlugin = true)
        {
            Instance = this;
            IsPlugin = isPlugin;

            Configuration.Load();

            PythonRunner.Init(this.SettingScriptPath);
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
            if (this.running)
            {
                return;
            }

            if (IsPlugin)
            {
                var form = new ExportSettingForm();

                form.ScriptPath = this.ScriptPath;
                form.ExportDirPath = this.ExportDirPath;
                form.ModelName = this.ModelName;
                form.ModelDescription = this.ModelDescription;
                form.KeepLogOpen = this.KeepLogOpen;

                form.ShowDialog(this.ApplicationForm);

                if (form.DialogResult == DialogResult.OK)
                {
                    this.ScriptPath = form.ScriptPath;
                    this.ExportDirPath = form.ExportDirPath;
                    this.ModelName = form.ModelName;
                    this.ModelDescription = form.ModelDescription;
                    this.KeepLogOpen = form.KeepLogOpen;

                    var progressForm = new ProgressForm();

                    var task = new Task(() =>
                    {
                        StreamWriter sw = new StreamWriter("lastest.log", false, Encoding.UTF8);
                        Console.SetOut(sw);

                        this.running = true;

                        try
                        {
                            this.RunScript(this.ScriptPath, progressForm);

                            if (!this.KeepLogOpen)
                            {
                                progressForm.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            sw.Dispose();
                            this.running = false;

                            StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput());
                            standardOutput.AutoFlush = true;
                            Console.SetOut(standardOutput);

                            progressForm.LogTextBox.Text = File.ReadAllText("lastest.log");
                        }
                    });
                    task.Start();

                    progressForm.ShowDialog();
                }
            }
            else
            {
            }
        }

        public void RunScript(string path, ProgressForm form)
        {
            World world = new World();

            PythonRunner.RunSpellScript(path, world);

            form.Progress.Minimum = 0;
            form.Progress.Maximum = World.MAX_FRAME;
            form.Progress.Step = 1;
            world.StartWorld(i => form.Progress.PerformStep());

            this.ExportVmd(world);
            this.ExportPmx(world);
        }

        private void ExportPmx(World world)
        {
            string fileName = ScriptFileName.Replace(".py", "");
            string exportPath = this.ExportDirPath + "\\" + fileName + ".pmx";
            File.Delete(exportPath);

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                var exporter = new PmxExporter(stream);

                var data = new PmxModelData();
                world.model.GetData(data);

                data.Header.modelName = this.ModelName;
                data.Header.description += this.ModelDescription;

                exporter.Export(data);
            }
        }

        private void ExportVmd(World world)
        {
            string fileName = ScriptFileName.Replace(".py", "");
            string exportPath = this.ExportDirPath + "\\" + fileName + ".vmd";
            File.Delete(exportPath);

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                var exporter = new VmdExporter(stream);

                var data = new VmdMotionData();
                world.motion.GetData(data);

                data.Header.modelName = this.ModelName;

                exporter.Export(data);
            }
        }
    }
}
