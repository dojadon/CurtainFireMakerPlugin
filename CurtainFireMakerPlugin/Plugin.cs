﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Forms;
using IronPython.Runtime.Exceptions;

namespace CurtainFireMakerPlugin
{
    public class Plugin : ICommandPlugin
    {
        public static Plugin Instance { get; set; }

        internal Configuration Config { get; }
        internal PythonExecutor PythonExecutor { get; }

        public string PluginRootPath => Application.StartupPath + "\\CurtainFireMaker";

        public Plugin()
        {
            Instance = this;

            Config = new Configuration(PluginRootPath + "\\config.xml");
            PythonExecutor = new PythonExecutor();

            try
            {
                Config.Load();
                InitIronPython();
            }
            catch (Exception e)
            {
                using (StreamWriter sw = new StreamWriter("lastest.log", false, Encoding.UTF8))
                {
                    try { Console.WriteLine(PythonExecutor.FormatException(e)); } catch { }
                    sw.WriteLine(e);
                }
            }
        }

        public void InitIronPython()
        {
            PythonExecutor.Init(Config.ModullesDirPaths);
            PythonExecutor.ExecuteScriptOnNewScope(Config.SettingScriptPath);
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
            var form = new ExportSettingForm()
            {
                ScriptPath = Config.ScriptPath,
                ExportDirPath = Config.ExportDirPath,
                ModelName = Config.ModelName,
                ModelDescription = Config.ModelDescription,
                KeepLogOpen = Config.KeepLogOpen,
                DropPmxFile = Config.DropPmxFile,
                DropVmdFile = Config.DropVmdFile,
            };
            form.ShowDialog(ApplicationForm);

            if (form.DialogResult == DialogResult.OK)
            {
                Config.ScriptPath = form.ScriptPath;
                Config.ExportDirPath = form.ExportDirPath;
                Config.ModelName = form.ModelName;
                Config.ModelDescription = form.ModelDescription;
                Config.KeepLogOpen = form.KeepLogOpen;
                Config.DropPmxFile = form.DropPmxFile;
                Config.DropVmdFile = form.DropVmdFile;

                ProgressForm progressForm = new ProgressForm();

                Task.Factory.StartNew(progressForm.ShowDialog);

                using (StreamWriter sw = new StreamWriter("lastest.log", false, Encoding.UTF8))
                {
                    try
                    {
                        Console.SetOut(sw);
                        PythonExecutor.SetOut(sw.BaseStream);

                        RunScript(Config.ScriptPath, progressForm);

                        if (!Config.KeepLogOpen)
                        {
                            progressForm.Dispose();
                        }
                    }
                    catch (Exception e)
                    {
                        try { Console.WriteLine(PythonExecutor.FormatException(e)); } catch { }
                        Console.WriteLine(e);
                    }
                }
            }
        }

        public void RunScript(string path, ProgressForm form)
        {
            var world = new World(Path.GetFileNameWithoutExtension(Config.ScriptPath));

            PythonExecutor.ExecuteScriptOnNewScope(path, new Variable("world", world));

            form.Progress.Minimum = 0;
            form.Progress.Maximum = world.MaxFrame;
            form.Progress.Step = 1;

            world.Init();

            for (int i = 0; i < world.MaxFrame; i++)
            {
                world.Frame();
                form.Progress.PerformStep();

                if (form.DialogResult == DialogResult.Cancel)
                {
                    break;
                }
            }

            if (form.DialogResult != DialogResult.Cancel)
            {
                world.Export();

                Console.Out.Dispose();

                StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
                Console.SetOut(standardOutput);
                PythonExecutor.SetOut(standardOutput.BaseStream);

                if (!form.IsDisposed)
                    form.LogTextBox.Text += File.ReadAllText("lastest.log");

                world.Finish();
            }
        }
    }
}
