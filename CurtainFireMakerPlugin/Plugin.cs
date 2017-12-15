using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Forms;

namespace CurtainFireMakerPlugin
{
    public class Plugin : ICommandPlugin
    {
        public static Plugin Instance { get; set; }

        internal Configuration Config { get; }
        internal PythonExecutor PythonExecutor { get; }

        public Plugin()
        {
            Instance = this;

            Config = new Configuration(Configuration.SettingXmlFilePath);
            PythonExecutor = new PythonExecutor();

            try
            {
                Config.Load();
                InitIronPython();
            }
            catch (Exception e)
            {
                using (var sw = new StreamWriter(Config.LogPath, false, Encoding.UTF8) { AutoFlush = false })
                {
                    try { sw.WriteLine(PythonExecutor.FormatException(e)); } catch { }
                    sw.WriteLine(e);
                }
            }

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CurtainFireMakerPlugin.icon.ico");
            Image = Image.FromStream(stream);
        }

        internal void InitIronPython()
        {
            PythonExecutor.Init(Config.ModullesDirPaths);
            PythonExecutor.ExecuteScriptOnRootScope(Config.InitScriptPath);
        }

        public Guid GUID => new Guid();
        public IWin32Window ApplicationForm { get; set; }
        public Scene Scene { get; set; }

        public string Description => "Curtain Fire Maker Plugin by zyando";
        public string Text => "弾幕生成";
        public string EnglishText => "Generate Curtain Fire";

        public Image Image { get; set; }
        public Image SmallImage => Image;

        public void Dispose()
        {
            Config.Save();
        }

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

                GenerateCurainFire();
            }
        }

        private void GenerateCurainFire()
        {
            ProgressForm progressForm = new ProgressForm();

            System.Threading.Tasks.Task.Factory.StartNew(progressForm.ShowDialog);

            using (var sw = new StreamWriter(Config.LogPath, false, Encoding.UTF8) { AutoFlush = false })
            {
                Console.SetOut(sw);
                PythonExecutor.SetOut(sw.BaseStream);

                try
                {
                    var world = new World(this, Path.GetFileNameWithoutExtension(Config.ScriptPath));

                    if (RunWorld(world, progressForm))
                    {
                        Finalize();

                        try { world.DropFileToMMM(); } catch { }
                    }
                }
                catch (Exception e)
                {
                    try { sw.WriteLine(PythonExecutor.FormatException(e)); } catch { }
                    sw.WriteLine(e);

                    Finalize();
                }

                void Finalize()
                {
                    sw.Flush();
                    sw.Dispose();

                    if (!progressForm.IsDisposed)
                        progressForm.LogText = File.ReadAllText(Config.LogPath);
                }
            }

            if (!Config.KeepLogOpen)
            {
                progressForm.Dispose();
            }
        }

        private bool RunWorld(World world, ProgressForm form)
        {
            bool isNeededDroping = false;

            world.InitPre();

            PythonExecutor.SetGlobalVariable(new Dictionary<string, object> { { "WORLD", world } });
            PythonExecutor.ExecuteScriptOnNewScope(Config.ScriptPath);

            world.InitPost();

            form.ProgressBar.Minimum = 0;
            form.ProgressBar.Maximum = world.MaxFrame;
            form.ProgressBar.Step = 1;

            for (int i = 0; i < world.MaxFrame && form.DialogResult != DialogResult.Cancel; i++)
            {
                world.Frame();
                form.ProgressBar.PerformStep();
            }

            if ((isNeededDroping = form.DialogResult != DialogResult.Cancel))
            {
                world.Export();
            }

            return isNeededDroping;
        }
    }
}
