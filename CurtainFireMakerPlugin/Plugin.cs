using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Forms;
using CurtainFireMakerPlugin.Forms.WinAPI;
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

        public Guid GUID => new Guid();
        public IWin32Window ApplicationForm { get; set; }
        public Scene Scene { get; set; }

        public string Description => "CurtainFireMaker Plugin by zyando";
        public string Text => "弾幕生成";
        public string EnglishText => "Generate Danmaku";

        public Image Image { get; set; } = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("CurtainFireMakerPlugin.icon.ico"));
        public Image SmallImage => Image;

        internal PythonExecutor Executor { get; } 
        public dynamic ScriptDynamic { get; private set; }
        private PresetEditorControl PresetEditorControl { get; set; }
        private ShotTypeProvider ShotTypeProvider { get; } = new ShotTypeProvider();
        private HookNativeWindow HookNativeWindow { get; }

        public Plugin()
        {
            Executor = new PythonExecutor();
            HookNativeWindow = new HookNativeWindow(this);
        }

        public UserControl CreateControl()
        {
            Init();
            return PresetEditorControl;
        }

        private void Init()
        {
            using (var writer = new StreamWriter(LogPath, false, Encoding.UTF8))
            {
                Console.SetOut(writer);
                try
                {
                    ScriptDynamic = Executor.Engine.ExecuteFile(SettingPythonFilePath, Executor.RootScope);

                    PresetEditorControl = new PresetEditorControl();

                    ShotTypeProvider.RegisterShotType(ScriptDynamic.init_shottype());
                    HookNativeWindow.RegisterHotKeys(ScriptDynamic.init_hotkeys());
                    HookNativeWindow.StartHook(Control.FromHandle(ApplicationForm.Handle));
                }
                catch (Exception e)
                {
                    using (var error_writer = new StreamWriter(ErrorLogPath, false, Encoding.UTF8))
                    {
                        try { error_writer.WriteLine(Executor.FormatException(e)); } catch { }
                        error_writer.WriteLine(e);
                    }
                }
            }
        }

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
            var progressForm = new ProgressForm()
            {
                Maximum = PresetEditorControl.EndFrame - PresetEditorControl.StartFrame
            };

            using (var writer = progressForm.CreateLogWriter())
            {
                Console.SetOut(writer);
                Executor.SetOut(writer);

                try
                {
                    System.Threading.Tasks.Task.Factory.StartNew(progressForm.ShowDialog);
                    RunWorld(progressForm);
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

        private void RunWorld(ProgressForm progress)
        {
            var worlds = new List<World>();

            Func<string, World> CreateWorld = (string name) =>
            {
                var world = new World(ShotTypeProvider, Executor, PresetEditorControl.StartFrame, PresetEditorControl.EndFrame)
                {
                    FrameCount = PresetEditorControl.StartFrame,
                    ExportedFileName = name,
                };
                worlds.Add(world);

                return world;
            };

            long time = Environment.TickCount;

            Executor.SetGlobalVariable(("SCENE", Scene), ("CreateWorld", CreateWorld), ("PRESET_FILENAME", PresetEditorControl.FileName));
            PresetEditorControl.RunScript(Executor.Engine, Executor.CreateScope());

            if (worlds.Count > 0)
            {
                for (int i = 0; i < progress.Maximum; i++)
                {
                    worlds.ForEach(w => w.Frame());
                    progress.Value = i;

                    Console.Out.Flush();

                    if (progress.IsCanceled) return;
                }
                progress.Text = "出力完了";

                worlds.ForEach(w => w.FinalizeWorld());
                worlds.ForEach(w => w.Export(ScriptDynamic, PresetEditorControl.ExportDirectory));
            }

            Console.WriteLine((Environment.TickCount - time) + "ms");
            Console.Out.Flush();

            var control = Control.FromHandle(ApplicationForm.Handle);

            foreach (var world in worlds)
            {
                try { world.DropFileToHandle(ApplicationForm.Handle, ScriptDynamic, PresetEditorControl.ExportDirectory); } catch { }
            }
        }
    }
}
