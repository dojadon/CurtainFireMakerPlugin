using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using MikuMikuPlugin;
using CurtainFireCore;
using CurtainFireMakerPlugin.Forms;
using CurtainFireMakerPlugin.Entities;

namespace CurtainFireMakerPlugin
{
    public class Plugin : ICommandPlugin, IHaveUserControl
    {
        public static string PluginRootPath => Application.StartupPath + "\\CurtainFireMaker\\";

        public static string SettingPythonFilePath => PluginRootPath + "config.py";
        public static string CommonScriptPath => PluginRootPath + "common.py";
        public static string ResourceDirPath => PluginRootPath + "Resource\\";
        public static string LogPath => PluginRootPath + "lastest.log";
        public static string ErrorLogPath => PluginRootPath + "error.log";
        public static string ConfigPath => PluginRootPath + "config.xml";

        public Guid GUID => new Guid();
        public IWin32Window ApplicationForm { get; set; }
        public Scene Scene { get; set; }

        public string Description => "CurtainFireMaker Plugin by zyando";
        public string Text => "弾幕生成";
        public string EnglishText => "Generate Danmaku";

        public Image Image { get; set; } = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("CurtainFireMakerPlugin.icon.ico"));
        public Image SmallImage => Image;

        private PluginConfig Config { get; } = new PluginConfig();
        internal PythonExecutor Executor { get; } = new PythonExecutor();
        private PluginControl PluginControl { get; set; }
        private ShotTypeProvider ShotTypeProvider { get; } = new ShotTypeProvider();
        public dynamic ScriptDynamic { get; private set; }

        public Plugin()
        {
            InitScriptEngine();
        }

        public UserControl CreateControl()
        {
            Init();
            return PluginControl;
        }

        public void Init()
        {
            using (var writer = new StreamWriter(LogPath, false, Encoding.UTF8))
            {
                SetOut(writer);
                try
                {
                    Config.Init();

                    if (File.Exists(ConfigPath)) Config.Load(ConfigPath);
                    else Config.Save(ConfigPath);

                    PluginControl = new PluginControl(Config);
                    PluginControl.InitScriptEngineEvent += (sender, e) => InitScriptEngine();
                }
                catch (Exception e)
                {
                    using (var error_writer = new StreamWriter(ErrorLogPath, false, Encoding.UTF8))
                    {
                        try { error_writer.WriteLine(Executor.FormatException(e)); } catch { }
                        error_writer.WriteLine(e);
                    }
                    MessageBox.Show(File.ReadAllText(ErrorLogPath), "CurtainFireMakerPlugin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Console.Out.Flush();
                SetOut(new StreamWriter(Console.OpenStandardOutput()));
            }
        }

        private void SetOut(TextWriter writer)
        {
            Console.SetOut(writer);
            Executor.SetOut(writer);
        }

        public void InitScriptEngine()
        {
            using (var writer = new StreamWriter(LogPath, false, Encoding.UTF8))
            {
                SetOut(writer);
                try
                {
                    Executor.Init();
                    ScriptDynamic = Executor.Engine.ExecuteFile(SettingPythonFilePath, Executor.RootScope);
                    ShotTypeProvider.RegisterShotType(ScriptDynamic.init_shottype());
                }
                catch (Exception e)
                {
                    using (var error_writer = new StreamWriter(ErrorLogPath, false, Encoding.UTF8))
                    {
                        try { error_writer.WriteLine(Executor.FormatException(e)); } catch { }
                        error_writer.WriteLine(e);
                    }
                    MessageBox.Show(File.ReadAllText(ErrorLogPath), "CurtainFireMakerPlugin", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Console.Out.Flush();
                SetOut(new StreamWriter(Console.OpenStandardOutput()));
            }
        }

        public void Dispose()
        {
            Config.Save(ConfigPath);
        }

        public void Run(CommandArgs args)
        {
            if (!PluginControl.GetSelectedPreset(out var preset) || preset == null) return;

            var progressForm = new ProgressForm()
            {
                Maximum = PluginControl.EndFrame - PluginControl.StartFrame
            };

            using (var writer = progressForm.CreateLogWriter())
            {
                SetOut(writer);

                try
                {
                    System.Threading.Tasks.Task.Factory.StartNew(progressForm.ShowDialog);
                    RunWorld(progressForm, preset);
                }
                catch (Exception e)
                {
                    try { Console.WriteLine(Executor.FormatException(e)); } catch { }
                    Console.WriteLine(e);
                }
                Console.Out.Flush();
                SetOut(new StreamWriter(Console.OpenStandardOutput()));
            }
            File.WriteAllText(LogPath, progressForm.LogText);
        }

        private void RunWorld(ProgressForm progress, PresetEditorControl preset)
        {
            var addWorlds = new List<World>();
            int startFrame = preset.StartFrame;
            int endFrame = preset.EndFrame;

            Func<string, World> CreateWorld = (string name) =>
            {
                var world = new World(ShotTypeProvider, Executor, startFrame, endFrame)
                {
                    FrameCount = startFrame,
                    ExportFileName = name,
                };
                addWorlds.Add(world);

                return world;
            };

            long time = Environment.TickCount;

            Executor.SetGlobalVariable(("SCENE", Scene), ("CreateWorld", CreateWorld), ("PRESET_FILENAME", preset.FileName), ("EXPORT_DIRECTORY", preset.ExportDirectory),
            ("STARTFRAME", startFrame), ("ENDFRAME", endFrame));
            PluginControl.RunScript(preset, Executor.Engine, Executor.CreateScope());

            var worlds = new List<World>(addWorlds);
            addWorlds.Clear();

            if (worlds.Count > 0)
            {
                for (int i = 0; i < progress.Maximum; i++)
                {
                    worlds.ForEach(w => w.Frame());

                    worlds.AddRange(addWorlds);
                    addWorlds.Clear();

                    progress.Value = i;

                    Console.Out.Flush();

                    if (progress.IsCanceled) return;
                }
                progress.Text = "出力完了";

                worlds.ForEach(w => w.FinalizeWorld());
                worlds.ForEach(w => w.Export(ScriptDynamic, preset.ExportDirectory));
            }

            Console.WriteLine((Environment.TickCount - time) + "ms");
            Console.Out.Flush();

            foreach (var world in worlds)
            {
                try { world.DropFileToHandle(ApplicationForm.Handle, ScriptDynamic, preset.ExportDirectory); } catch { }
            }
        }
    }
}
