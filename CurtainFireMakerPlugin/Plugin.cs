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

        public Plugin() : this(true)
        {
        }

        public Plugin(bool isPlugin = true)
        {
            Instance = this;
            IsPlugin = isPlugin;

            Config = new Configuration(PluginRootPath + "\\config.xml");
            Config.Load();

            try
            {
                PythonRunner = new PythonRunner(Config.SettingScriptPath, Config.ModullesDirPath);
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new StreamWriter("lastest.log", false, Encoding.UTF8))
                {
                    try { sw.WriteLine(PythonRunner.FormatException(e)); } catch { }
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

                    ProgressForm progressForm = new ProgressForm();

                    Task.Factory.StartNew(progressForm.ShowDialog);

                    StreamWriter sw = new StreamWriter("lastest.log", false, Encoding.UTF8);
                    try
                    {
                        Console.SetOut(sw);
                        PythonRunner.SetOut(sw.BaseStream);

                        RunScript(Config.ScriptPath, progressForm);

                        if (!Config.KeepLogOpen)
                        {
                            progressForm.Dispose();
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

                        if(!progressForm.IsDisposed)
                        progressForm.LogTextBox.Text = File.ReadAllText("lastest.log");
                    }
                }
            }
            else
            {
            }
        }

        public void RunScript(string path, ProgressForm form)
        {
            var world = new World(Path.GetFileNameWithoutExtension(Config.ScriptPath));

            PythonRunner.RunScript(path, world);

            form.Progress.Minimum = 0;
            form.Progress.Maximum = world.MaxFrame;
            form.Progress.Step = 1;

            for (int i = 0; i < world.MaxFrame; i++)
            {
                world.Frame();
                form.Progress.PerformStep();
            }

            world.Finish();
        }
    }
}
