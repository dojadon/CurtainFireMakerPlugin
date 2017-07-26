using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using MikuMikuPlugin;
using CsPmx;
using CsPmx.Data;
using CsVmd;
using CsVmd.Data;
using CurtainFireMakerPlugin.Forms;

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
        public string ModullesDirPath { get; set; }
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

            try
            {
                PythonRunner.Init(this.SettingScriptPath, this.ModullesDirPath);
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new StreamWriter("lastest.log", false, Encoding.UTF8))
                {
                    sw.WriteLine(e);
                }
            }
        }

        public Guid GUID => new Guid();
        public IWin32Window ApplicationForm { get; set; }
        public Scene Scene { get; set; }

        public string Description => "Curtain Fire Maker Plugin by zyando";
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
                var form = new ExportSettingForm()
                {
                    ScriptPath = this.ScriptPath,
                    ExportDirPath = this.ExportDirPath,
                    ModelName = this.ModelName,
                    ModelDescription = this.ModelDescription,
                    KeepLogOpen = this.KeepLogOpen
                };
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
                        PythonRunner.SetOut(sw.BaseStream);

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

                            StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
                            Console.SetOut(standardOutput);
                            PythonRunner.SetOut(standardOutput.BaseStream);

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
            World.WorldList.Clear();

            PythonRunner.RunSpellScript(path);

            form.Progress.Minimum = 0;
            form.Progress.Maximum = World.MaxFrame;
            form.Progress.Step = 1;

            List<World> worldList = World.WorldList;

            for (int i = 0; i < World.MaxFrame; i++)
            {
                worldList.ForEach(w => w.Frame());
                form.Progress.PerformStep();
            }

            for (int i = 0; i < worldList.Count; i++)
            {
                var world = worldList[i];
                world.Finish();

                this.ExportPmx(world, worldList.Count > 1 ? (i + 1).ToString() : "");
                this.ExportVmd(world, worldList.Count > 1 ? (i + 1).ToString() : "");
            }
        }

        private void ExportPmx(World world, string text)
        {
            string fileName = ScriptFileName.Replace(".py", "");
            string exportPath = this.ExportDirPath + "\\" + fileName + text + ".pmx";
            File.Delete(exportPath);

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                var exporter = new PmxExporter(stream);

                var data = new PmxModelData();
                world.PmxModel.GetData(data);

                data.Header.ModelName = this.ModelName;
                data.Header.Description += this.ModelDescription;

                exporter.Export(data);

                Console.WriteLine("出力完了 : " + fileName);
                Console.WriteLine("頂点数 : " + String.Format("{0:#,0}", data.VertexArray.Length));
                Console.WriteLine("面数 : " + String.Format("{0:#,0}", data.VertexIndices.Length / 3));
                Console.WriteLine("材質数 : " + String.Format("{0:#,0}", data.MaterialArray.Length));
                Console.WriteLine("ボーン数 : " + String.Format("{0:#,0}", data.BoneArray.Length));
                Console.WriteLine("モーフ数 : " + String.Format("{0:#,0}", data.MorphArray.Length));
            }
        }

        private void ExportVmd(World world, string text)
        {
            string fileName = ScriptFileName.Replace(".py", "");
            string exportPath = this.ExportDirPath + "\\" + fileName + text + ".vmd";
            File.Delete(exportPath);

            using (var stream = new FileStream(exportPath, FileMode.Create, FileAccess.Write))
            {
                var exporter = new VmdExporter(stream);

                var data = new VmdMotionData();
                world.VmdMotion.GetData(data);

                data.Header.ModelName = this.ModelName;

                exporter.Export(data);
            }
        }
    }
}
