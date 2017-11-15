using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Forms;

namespace CurtainFireMakerPlugin
{
    /// <summary>
    /// プラグイン
    /// </summary>
    public class Plugin : ICommandPlugin
    {
        internal static Plugin Instance { get; set; }

        internal Configuration Config { get; }
        internal PythonExecutor PythonExecutor { get; }

        internal string PluginRootPath => Application.StartupPath + "\\CurtainFireMaker";

        /// <summary>
        /// コンストラクタ
        /// </summary>
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
                    try { sw.WriteLine(PythonExecutor.FormatException(e)); } catch { }
                    sw.WriteLine(e);
                }
            }

            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("CurtainFireMakerPlugin.Resources.icon.ico");
            Image = Image.FromStream(stream);
        }

        internal void InitIronPython()
        {
            PythonExecutor.Init(Config.ModullesDirPaths);
            PythonExecutor.ExecuteScriptOnNewScope(Config.SettingScriptPath);
        }

        /// <summary>GUID</summary>
        public Guid GUID => new Guid();
        /// <summary>ApplicationForm</summary>
        public IWin32Window ApplicationForm { get; set; }
        /// <summary>Scene</summary>
        public Scene Scene { get; set; }

        /// <summary>Description</summary>
        public string Description => "Curtain Fire Maker Plugin by zyando";
        /// <summary>Text</summary>
        public string Text => "弾幕生成";
        /// <summary>EnglishText</summary>
        public string EnglishText => "Generate Curtain Fire";

        /// <summary>Image</summary>
        public Image Image { get; set; }
        /// <summary>SmallImage</summary>
        public Image SmallImage => Image;

        /// <summary>Dispose</summary>
        public void Dispose()
        {
            Config.Save();
        }

        /// <summary>
        /// コマンドが実行されたときに呼ばれる
        /// </summary>
        /// <param name="args"></param>
        public void Run(CommandArgs args)
        {
            var form = new ExportSettingForm()
            {
                ScriptPath = Config.ScriptPath,
                PmxExportDirPath = Config.PmxExportDirPath,
                VmdExportDirPath = Config.VmdExportDirPath,
                KeepLogOpen = Config.KeepLogOpen,
                DropPmxFile = Config.DropPmxFile,
                DropVmdFile = Config.DropVmdFile,
            };
            form.ShowDialog(ApplicationForm);

            if (form.DialogResult == DialogResult.OK)
            {
                Config.ScriptPath = form.ScriptPath;
                Config.PmxExportDirPath = form.PmxExportDirPath;
                Config.VmdExportDirPath = form.VmdExportDirPath;
                Config.KeepLogOpen = form.KeepLogOpen;
                Config.DropPmxFile = form.DropPmxFile;
                Config.DropVmdFile = form.DropVmdFile;

                ProgressForm progressForm = new ProgressForm();

                Task.Factory.StartNew(progressForm.ShowDialog);

                using (StreamWriter sw = new StreamWriter("lastest.log", false, Encoding.UTF8) { AutoFlush = true, })
                {
                    Console.SetOut(sw);
                    PythonExecutor.SetOut(sw.BaseStream);

                    RunScript(Config.ScriptPath, progressForm, Finalize);

                    void Finalize()
                    {
                        sw.Dispose();

                        StreamWriter standardOutput = new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true };
                        Console.SetOut(standardOutput);
                        PythonExecutor.SetOut(standardOutput.BaseStream);

                        if (!form.IsDisposed)
                            progressForm.LogTextBox.Text = File.ReadAllText("lastest.log");
                    }

                    if (!Config.KeepLogOpen)
                    {
                        progressForm.Dispose();
                    }
                }
            }
        }

        private void RunScript(string path, ProgressForm form, Action finalize)
        {
            var world = new World(this, Path.GetFileNameWithoutExtension(Config.ScriptPath));

            world.InitPre();

            bool dropFlag = false;

            try
            {
                long time = Environment.TickCount;

                PythonExecutor.ExecuteScriptOnNewScope(path, new Variable("world", world));

                form.Progress.Minimum = 0;
                form.Progress.Maximum = world.MaxFrame;
                form.Progress.Step = 1;

                world.InitPost();

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
                    dropFlag = true;
                }
                Console.WriteLine($"{Environment.TickCount - time}ms");
            }
            catch (Exception e)
            {
                try { Console.WriteLine(PythonExecutor.FormatException(e)); } catch { }
                Console.WriteLine(e);
            }
            finally
            {
                finalize();
            }

            if (dropFlag)
            {
                world.DropFileToMMM();
            }
        }
    }
}
