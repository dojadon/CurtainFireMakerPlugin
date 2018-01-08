using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using CurtainFireMakerPlugin.Entities;
using CurtainFireMakerPlugin.Forms;
using IronPython.Runtime;
using IronPython.Runtime.Operations;
using CsMmdDataIO.Vmd;
using Microsoft.WindowsAPICodePack.Taskbar;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        public int MaxFrame { get; set; } = 1000;

        public Configuration Config { get; }
        internal PythonExecutor Executor { get; }
        internal IntPtr HandleToDrop { get; }

        public dynamic Script { get; internal set; }

        public List<StaticRigidObject> RigidObjectList { get; } = new List<StaticRigidObject>();

        private List<Entity> AddEntityList { get; } = new List<Entity>();
        private List<Entity> RemoveEntityList { get; } = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        public ShotTypeProvider ShotTypeProvider { get; }

        internal ShotModelDataProvider ShotModelProvider { get; }
        internal CurtainFireModel PmxModel { get; }
        internal CurtainFireMotion KeyFrames { get; }

        private TaskManager TaskManager { get; } = new TaskManager();

        internal string ExportFileName { get; set; }

        public event EventHandler ExportEvent;

        internal string PmxExportPath => Config.PmxExportDirPath + "\\" + ExportFileName + ".pmx";
        internal string VmdExportPath => Config.VmdExportDirPath + "\\" + ExportFileName + ".vmd";

        public string ModelName { get; set; }
        public string ModelDescription { get; set; } = "This model is created by Curtain Fire Maker Plugin";

        internal World(ShotTypeProvider typeProvider, PythonExecutor executor, Configuration config, IntPtr handle, string fileName)
        {
            ShotTypeProvider = typeProvider;
            Executor = executor;
            Config = config;
            HandleToDrop = handle;

            ExportFileName = ModelName = fileName;

            ShotModelProvider = new ShotModelDataProvider();
            PmxModel = new CurtainFireModel(this);
            KeyFrames = new CurtainFireMotion(this);
        }

        public void AddRigidObject(StaticRigidObject rigid)
        {
            RigidObjectList.Add(rigid);
        }

        internal ShotModelData AddShot(EntityShot entity)
        {
            if (ShotModelProvider.AddEntity(entity, out ShotModelData data))
            {
                PmxModel.InitShotModelData(data);
            }
            return data;
        }

        internal int AddEntity(Entity entity)
        {
            AddEntityList.Add(entity);

            return FrameCount;
        }

        internal int RemoveEntity(Entity entity)
        {
            RemoveEntityList.Add(entity);

            return FrameCount;
        }

        internal void InitPre()
        {
            foreach (var type in ShotTypeProvider.ShotTypeDict.Values)
            {
                type.InitWorld(this);
            }
        }

        internal void InitPost()
        {
            if (FrameCount > 0)
            {
                KeyFrames.AddPropertyKeyFrame(new VmdPropertyFrameData(0, false));
                KeyFrames.AddPropertyKeyFrame(new VmdPropertyFrameData(FrameCount, true));
            }
        }

        internal void Frame()
        {
            ShotModelProvider.Frame();
            TaskManager.Frame();

            EntityList.AddRange(AddEntityList);
            RemoveEntityList.ForEach(e => EntityList.Remove(e));

            AddEntityList.Clear();
            RemoveEntityList.Clear();

            EntityList.ForEach(e => e.Frame());

            FrameCount++;
        }

        public void GenerateCurainFire(string script)
        {
            ProgressForm progressForm = new ProgressForm();

            System.Threading.Tasks.Task.Factory.StartNew(progressForm.ShowDialog);

            using (var writer = progressForm.CreateLogWriter())
            {
                Console.SetOut(writer);
                Executor.SetOut(writer);

                try
                {
                    long time = Environment.TickCount;

                    if (RunWorld(script, progressForm))
                    {
                        Console.WriteLine((Environment.TickCount - time) + "ms");
                        Console.Out.Flush();

                        try { DropFileToHandle(); } catch { }
                    }
                }
                catch (Exception e)
                {
                    try { Console.WriteLine(Executor.FormatException(e)); } catch { }
                    Console.WriteLine(e);
                }
                finally
                {
                    File.WriteAllText(Config.LogPath, progressForm.LogText);
                    Console.SetOut(new StreamWriter(Console.OpenStandardOutput()));
                    Executor.SetOut(Console.OpenStandardOutput());
                }
            }
        }

        public bool RunWorld(string script, ProgressForm form)
        {
            InitPre();

            Executor.SetGlobalVariable(("WORLD", this));
            Executor.ExecuteOnRootScope(script);
            Executor.ExecuteFileOnNewScope(Config.ScriptPath);

            InitPost();

            form.ProgressBar.Maximum = MaxFrame;
            form.ProgressBar.Step = 1;

            for (int i = 0; i < MaxFrame && form.DialogResult != DialogResult.Cancel; i++)
            {
                Frame();
                form.ProgressBar.PerformStep();
                form.Text = "生成中［" + i + " / " + MaxFrame + "］";
                Console.Out.Flush();
            }
            form.Text = "生成完了";

            if (form.DialogResult != DialogResult.Cancel)
            {
                Export();
                return true;
            }
            return false;
        }

        internal void Export()
        {
            EntityList.ForEach(e => e.OnDeath());

            PmxModel.FinalizeModel(KeyFrames.MorphFrameDict.Values.Select(t => t.frame));
            KeyFrames.FinalizeKeyFrame(PmxModel.Morphs.MorphList);

            PmxModel.Export();
            KeyFrames.Export();

            ExportEvent?.Invoke(this, EventArgs.Empty);
        }

        internal void DropFileToHandle()
        {
            if (Config.ShouldDropPmxFile)
            {
                Drop(HandleToDrop, new StringCollection() { PmxExportPath });
            }

            if (Config.ShouldDropVmdFile && Config.ShouldDropPmxFile)
            {
                Drop(HandleToDrop, new StringCollection() { VmdExportPath });
            }

            void Drop(IntPtr hWnd, StringCollection filePaths)
            {
                Clipboard.Clear();
                Clipboard.SetFileDropList(filePaths);
                var data = Clipboard.GetDataObject();

                IDropTarget target = Control.FromHandle(hWnd);
                DragDropEffects dwEffect = DragDropEffects.Copy | DragDropEffects.Link;

                target.OnDragDrop(new DragEventArgs(data, 0, 0, 0, dwEffect, dwEffect));

                Clipboard.Clear();
            }
        }

        internal void AddTask(Task task)
        {
            TaskManager.AddTask(task);
        }

        internal void AddTask(Action<Task> task, int interval, int executeTimes, int waitTime)
        {
            AddTask(new Task(task, interval, executeTimes, waitTime));
        }

        public void AddTask(PythonFunction func, int interval, int executeTimes, int waitTime, bool withArg = false)
        {
            if (withArg)
            {
                AddTask(task => PythonCalls.Call(func, task), interval, executeTimes, waitTime);
            }
            else
            {
                AddTask(task => PythonCalls.Call(func), interval, executeTimes, waitTime);
            }
        }
    }
}
