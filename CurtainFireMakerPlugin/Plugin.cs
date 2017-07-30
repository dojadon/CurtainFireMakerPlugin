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

        internal Configuration Config { get; }
        internal PythonRunner PythonRunner { get; }

        public bool IsPlugin { get; }
        public string PluginRootPath => Application.StartupPath + (IsPlugin ? "\\CurtainFireMaker" : "");

        public string ScriptFileName
        {
            get
            {
                string[] split = Config.ScriptPath.Split('\\');
                return split[split.Length - 1];
            }
        }

        public Plugin() : this(true)
        {
        }

        public Plugin(bool isPlugin = true)
        {
            try
            {
                Instance = this;
                IsPlugin = isPlugin;

                Config = new Configuration(PluginRootPath + "\\config.xml");
                Config.Load();

                PythonRunner = new PythonRunner(Config.SettingScriptPath, Config.ModullesDirPath);
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new StreamWriter("lastest.log", false, Encoding.UTF8))
                {
                    sw.WriteLine(e);
                    sw.WriteLine(PythonRunner.FormatException(e));
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
            Config.Save();
        }

        public void Run(CommandArgs args)
        {
            if (IsPlugin)
            {
                var form = new ExportSettingForm()
                {
                    ScriptPath = Config.ScriptPath,
                    ExportDirPath = Config.ExportDirPath,
                    ModelName = Config.ModelName,
                    ModelDescription = Config.ModelDescription,
                    KeepLogOpen = Config.KeepLogOpen
                };
                form.ShowDialog(this.ApplicationForm);

                if (form.DialogResult == DialogResult.OK)
                {
                    Config.ScriptPath = form.ScriptPath;
                    Config.ExportDirPath = form.ExportDirPath;
                    Config.ModelName = form.ModelName;
                    Config.ModelDescription = form.ModelDescription;
                    Config.KeepLogOpen = form.KeepLogOpen;

                    var progressForm = new ProgressForm();

                    var task = new Task(() =>
                    {
                        StreamWriter sw = new StreamWriter("lastest.log", false, Encoding.UTF8);
                        Console.SetOut(sw);
                        PythonRunner.SetOut(sw.BaseStream);

                        try
                        {
                            this.RunScript(Config.ScriptPath, progressForm);

                            if (!Config.KeepLogOpen)
                            {
                                progressForm.Close();
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(PythonRunner.FormatException(e));
                            Console.WriteLine(e);
                        }
                        finally
                        {
                            sw.Dispose();

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

                world.OnExport(EventArgs.Empty);
            }
        }
    }
}
