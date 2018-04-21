using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Forms;
using CurtainFireMakerPlugin.Entities;

namespace CurtainFireMakerPlugin
{
    public class Plugin : ICanSavePlugin, ICommandPlugin, IHaveUserControl
    {
        public static string PluginRootPath => Application.StartupPath + "\\CurtainFireMaker\\";

        public static string SettingPythonFilePath => PluginRootPath + "config.py";
        public static string CommonScriptPath => PluginRootPath + "common.py";
        public static string ResourceDirPath => PluginRootPath + "Resource\\";
        public static string LogPath => PluginRootPath + "lastest.log";
        public static string ErrorLogPath => PluginRootPath + "error.log";

        internal PythonExecutor Executor { get; }

        public dynamic ScriptDynamic { get; private set; }

        private PresetEditorControl PresetEditorControl { get; set; }

        private ShotTypeProvider ShotTypeProvider { get; } = new ShotTypeProvider();

        public Plugin()
        {
            using (var writer = new StreamWriter(LogPath, false, Encoding.UTF8))
            {
                using (var error_writer = new StreamWriter(ErrorLogPath, false, Encoding.UTF8))
                {
                    Console.SetOut(writer);
                    Console.SetError(error_writer);

                    try
                    {
                        Executor = new PythonExecutor();
                        ScriptDynamic = Executor.Engine.ExecuteFile(SettingPythonFilePath, Executor.RootScope);

                        PresetEditorControl = new PresetEditorControl();

                        ShotTypeProvider.RegisterShotType(ScriptDynamic.init_shottype());
                    }
                    catch (Exception e)
                    {
                        try { error_writer.WriteLine(Executor.FormatException(e)); } catch { }
                        error_writer.WriteLine(e);
                    }

                    Image = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("CurtainFireMakerPlugin.icon.ico"));
                }
            }
        }

        public Guid GUID => new Guid();
        public IWin32Window ApplicationForm { get; set; }
        public Scene Scene { get; set; }

        public string Description => "CurtainFireMaker Plugin by zyando";
        public string Text => "弾幕生成";
        public string EnglishText => "Generate Danmaku";

        public Image Image { get; set; }
        public Image SmallImage => Image;

        public void Dispose()
        {
            PresetEditorControl.Save();
        }

        public Stream OnSaveProject()
        {
            PresetEditorControl.Save();
            return new MemoryStream();
        }

        public void OnLoadProject(Stream stream)
        {

        }

        public void Run(CommandArgs args)
        {
            var progressForm = new ProgressForm();

            using (var writer = progressForm.CreateLogWriter())
            {
                Console.SetOut(writer);
                Executor.SetOut(writer);

                try
                {
                    System.Threading.Tasks.Task.Factory.StartNew(progressForm.ShowDialog);
                    RunWorld(progressForm.ProgressBar, () => progressForm.DialogResult == DialogResult.Cancel);
                }
                catch (Exception e)
                {
                    try { Console.WriteLine(Executor.FormatException(e)); } catch { }
                    Console.WriteLine(e);
                }
                Console.Out.Flush();
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                Executor.SetOut(Console.OpenStandardOutput());
            }
            File.WriteAllText(LogPath, progressForm.LogText);
        }

        private void RunWorld(ProgressBar bar, Func<bool> isEnd)
        {
            var world = new World(ShotTypeProvider, Executor, ApplicationForm.Handle)
            {
                Script = ScriptDynamic,
                FrameCount = PresetEditorControl.StartFrame,
                MaxFrame = PresetEditorControl.EndFrame - PresetEditorControl.StartFrame,
            };

            Executor.SetGlobalVariable(("SCENE", Scene), ("WORLD", world));

            world.Init();

            PresetEditorControl.RunScript(Executor.Engine, Executor.CreateScope());

            bar.Maximum = world.MaxFrame;

            world.GenerateCurainFire(i =>
            {
                bar.Value = i;
                Console.Out.Flush();

                return isEnd();
            }, PresetEditorControl.PmxExportDirectory, PresetEditorControl.VmdExportDirectory);
        }

        public UserControl CreateControl()
        {
            return PresetEditorControl;
        }
    }
}
