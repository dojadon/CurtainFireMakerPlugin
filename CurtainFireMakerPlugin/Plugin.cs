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

            IronPythonControl = new IronPythonControl { ScriptText = Script.default_script, };
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

            Config.ScriptPath = form.ScriptPath;
            Config.PmxExportDirPath = form.PmxExportDirPath;
            Config.VmdExportDirPath = form.VmdExportDirPath;
            Config.KeepLogOpen = form.KeepLogOpen;
            Config.DropPmxFile = form.DropPmxFile;
            Config.DropVmdFile = form.DropVmdFile;

            if (form.DialogResult == DialogResult.OK)
            {
                PythonExecutor.SetGlobalVariable(("SCENE", Scene));

                var world = new World(ShotTypeProvider, PythonExecutor, Config, ApplicationForm.Handle, Path.GetFileNameWithoutExtension(Config.ScriptPath))
                {
                    Script = Script
                };
                world.GenerateCurainFire(IronPythonControl.ScriptText);
            }
        }
    }
}
