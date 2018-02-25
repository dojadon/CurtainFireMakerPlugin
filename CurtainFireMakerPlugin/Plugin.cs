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
        public static Version Version = new Version(1, 0);

        internal Configuration Config { get; }
        internal PythonExecutor PythonExecutor { get; }

        public dynamic Script { get; private set; }

        private ProjectScriptControl ProjectScriptControl { get; }

        private ShotTypeProvider ShotTypeProvider { get; } = new ShotTypeProvider();

        public Plugin()
        {
            Config = new Configuration(Configuration.SettingXmlFilePath);
            Config.Load();

            try
            {
                PythonExecutor = new PythonExecutor(Config.ModullesDirPaths);
                Script = PythonExecutor.ExecuteFileOnRootScope(Configuration.SettingPythonFilePath);

                ProjectScriptControl = new ProjectScriptControl(File.ReadAllText(Configuration.CommonRootScriptPath), File.ReadAllText(Configuration.CommonScriptPath));

                ShotTypeProvider.RegisterShotType(Script.init_shottype());
            }
            catch (Exception e)
            {
                using (var sw = new StreamWriter(Configuration.ErrorLogPath, false, Encoding.UTF8))
                {
                    try { sw.WriteLine(PythonExecutor.FormatException(e)); } catch { }
                    sw.WriteLine(e);
                }
            }

            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("CurtainFireMakerPlugin.icon.ico");
            Image = Image.FromStream(stream);
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
        }

        public UserControl CreateControl()
        {
            return ProjectScriptControl;
        }

        public Stream OnSaveProject()
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);

            WriteString(Version.ToString());

            writer.Write(ProjectScriptControl.ScriptDict.Count);
            ProjectScriptControl.ScriptDict.ForEach(p =>
            {
                WriteString(p.Key);
                WriteString(p.Value);
            });

            void WriteString(string s)
            {
                var bytes = Encoding.Unicode.GetBytes(s);
                writer.Write(bytes.Length);
                writer.Write(bytes);
            }

            return stream;
        }

        public void OnLoadProject(Stream stream)
        {
            var reader = new BinaryReader(stream);

            if (Version.Parse(ReadString()) != Version)
            {
                return;
            }

            for (int i = 0, len = reader.ReadInt32(); i < len; i++)
            {
                ProjectScriptControl.ScriptDict.Add(ReadString(), ReadString());
            }

            string ReadString()
            {
                return Encoding.Unicode.GetString(reader.ReadBytes(reader.ReadInt32()));
            }
        }

        public void Run(CommandArgs args)
        {
            var form = new ExportSettingForm(Config, ProjectScriptControl);
            form.ShowDialog(ApplicationForm);

            if (form.DialogResult == DialogResult.OK)
            {
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
        }

        private void RunWorld(ProgressBar bar, Func<bool> isEnd)
        {
            var world = new World(ShotTypeProvider, PythonExecutor, Config, ApplicationForm.Handle, Path.GetFileNameWithoutExtension(Config.ScriptPath))
            {
                Script = Script
            };

            PythonExecutor.SetGlobalVariable(("SCENE", Scene));
            PythonExecutor.SetGlobalVariable(("WORLD", world));
            PythonExecutor.ExecuteOnRootScope(ProjectScriptControl.RootScript);
            PythonExecutor.ExecuteOnRootScope(ProjectScriptControl.GetScript(Path.GetFileNameWithoutExtension(Config.ScriptPath)));

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
