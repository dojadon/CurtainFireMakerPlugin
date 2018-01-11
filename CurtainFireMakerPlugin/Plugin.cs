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

        private IronPythonControl IronPythonControl { get; }

        private ShotTypeProvider ShotTypeProvider { get; } = new ShotTypeProvider();

        public Plugin()
        {
            Config = new Configuration(Configuration.SettingXmlFilePath);
            PythonExecutor = new PythonExecutor();

            try
            {
                Config.Load();

                PythonExecutor.Init(Config.ModullesDirPaths);
                Script = PythonExecutor.ExecuteFileOnRootScope(Configuration.SettingPythonFilePath);
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

            IronPythonControl = new IronPythonControl { ScriptText = File.ReadAllText(Config.CommonScriptPath), };
            Script.init_shottype(ShotTypeProvider);
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

        public UserControl CreateControl()
        {
            return IronPythonControl;
        }

        public Stream OnSaveProject()
        {
            var bytes = Encoding.Unicode.GetBytes(IronPythonControl.ScriptText);

            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            writer.Write(bytes.Length);
            writer.Write(bytes);
            return stream;
        }

        public void OnLoadProject(Stream stream)
        {
            var reader = new BinaryReader(stream);
            IronPythonControl.ScriptText = Encoding.Unicode.GetString(reader.ReadBytes(reader.ReadInt32()));
        }

        public void Run(CommandArgs args)
        {
            var form = new ExportSettingForm(Config);
            form.ShowDialog(ApplicationForm);

            if (form.DialogResult == DialogResult.OK)
            {
                PythonExecutor.SetGlobalVariable(("SCENE", Scene));

                var progressForm = new ProgressForm();

                var world = new World(ShotTypeProvider, PythonExecutor, Config, ApplicationForm.Handle, Path.GetFileNameWithoutExtension(Config.ScriptPath))
                {
                    Script = Script
                };

                System.Threading.Tasks.Task.Factory.StartNew(progressForm.ShowDialog);

                using (var writer = progressForm.CreateLogWriter())
                {
                    Console.SetOut(writer);
                    PythonExecutor.SetOut(writer);

                    world.GenerateCurainFire((max, i) =>
                    {
                        progressForm.ProgressBar.Maximum = max;
                        progressForm.ProgressBar.PerformStep();
                        return progressForm.DialogResult == DialogResult.Cancel;

                    }, IronPythonControl.ScriptText);

                    Console.Out.Flush();
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                    PythonExecutor.SetOut(Console.OpenStandardOutput());
                }
                File.WriteAllText(Config.LogPath, progressForm.LogText);
            }
        }
    }
}
