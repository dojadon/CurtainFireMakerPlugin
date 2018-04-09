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
    public class Plugin : ICommandPlugin, IHaveUserControl, ICanSavePlugin
    {
        internal Configuration Config { get; }
        internal PythonExecutor PythonExecutor { get; }

        public dynamic Script { get; private set; }

        private PresetEditorControl PresetEditorControl { get; }

        private ShotTypeProvider ShotTypeProvider { get; } = new ShotTypeProvider();

        public Plugin()
        {
            using (var writer = new StreamWriter(Configuration.LogPath, false, Encoding.UTF8))
            {
                using (var error_writer = new StreamWriter(Configuration.ErrorLogPath, false, Encoding.UTF8))
                {
                    Console.SetOut(writer);
                    Console.SetError(error_writer);

                    try
                    {
                        Config = new Configuration(Configuration.SettingXmlFilePath);
                        Config.Load();

                        PythonExecutor = new PythonExecutor(Config.ModullesDirPaths);
                        Script = PythonExecutor.ExecuteFileOnRootScope(Configuration.SettingPythonFilePath);

                        PresetEditorControl = new PresetEditorControl(this);

                        ShotTypeProvider.RegisterShotType(Script.init_shottype());
                    }
                    catch (Exception e)
                    {
                        try { error_writer.WriteLine(PythonExecutor.FormatException(e)); } catch { }
                        error_writer.WriteLine(e);
                    }

                    Image = Image.FromStream(Assembly.GetExecutingAssembly().GetManifestResourceStream("CurtainFireMakerPlugin.icon.ico"));
                }
            }
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
            Config?.Save();
            PresetEditorControl.Save();
        }

        public UserControl CreateControl()
        {
            return PresetEditorControl;
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
            var form = new ExportSettingForm(Config, PresetEditorControl);
            form.ShowDialog(ApplicationForm);

            if (form.DialogResult != DialogResult.OK) return;

            var progressForm = new ProgressForm();

            using (var writer = progressForm.CreateLogWriter())
            {
                Console.SetOut(writer);
                PythonExecutor.SetOut(writer);

                try
                {
                    System.Threading.Tasks.Task.Factory.StartNew(progressForm.ShowDialog);
                    RunWorld(progressForm.ProgressBar, () => progressForm.DialogResult == DialogResult.Cancel);
                }
                catch (Exception e)
                {
                    try { Console.WriteLine(PythonExecutor.FormatException(e)); } catch { }
                    Console.WriteLine(e);
                }
                Console.Out.Flush();
                Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                PythonExecutor.SetOut(Console.OpenStandardOutput());
            }
            File.WriteAllText(Configuration.LogPath, progressForm.LogText);
        }

        private void RunWorld(ProgressBar bar, Func<bool> isEnd)
        {
            var world = new World(ShotTypeProvider, PythonExecutor, Config, ApplicationForm.Handle, Path.GetFileNameWithoutExtension(Config.ScriptPath))
            {
                Script = Script
            };

            PythonExecutor.SetGlobalVariable(("SCENE", Scene));
            PythonExecutor.SetGlobalVariable(("WORLD", world));

            if (PresetEditorControl.IsPresetSelected)
            {
                PythonExecutor.ExecuteOnRootScope(PresetEditorControl.RootScript);
            }
            else
            {
                PythonExecutor.ExecuteOnRootScope(File.ReadAllText(Configuration.CommonScriptPath));
            }
            PythonExecutor.ExecuteOnRootScope(PresetEditorControl.GetPreScript(Config.ScriptPath));

            world.Init();

            bar.Maximum = world.MaxFrame;

            world.GenerateCurainFire(i =>
            {
                bar.Value = i;
                Console.Out.Flush();
            }, isEnd);
        }
    }
}
