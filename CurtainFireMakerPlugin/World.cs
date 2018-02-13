using System;
using System.Linq;
using System.IO;
using System.Dynamic;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using CurtainFireMakerPlugin.Entities;
using IronPython.Runtime;
using IronPython.Runtime.Operations;
using MMDataIO.Vmd;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        public int MaxFrame { get; set; } = 1000;

        public Configuration Config { get; }
        internal PythonExecutor Executor { get; }
        internal IntPtr HandleToDrop { get; }

        public dynamic Script { get; set; }

        public List<StaticRigidObject> RigidObjectList { get; } = new List<StaticRigidObject>();

        private List<Entity> AddEntityList { get; } = new List<Entity>();
        private List<Entity> RemoveEntityList { get; } = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        public ShotTypeProvider ShotTypeProvider { get; }

        internal ShotModelDataProvider ShotModelProvider { get; }
        internal CurtainFireModel PmxModel { get; }
        internal CurtainFireMotion KeyFrames { get; }

        private TaskScheduler TaskScheduler { get; } = new TaskScheduler();

        internal string ExportFileName { get; set; }

        public event EventHandler ExportEvent;

        private Dictionary<string, object> AttributeDict { get; } = new Dictionary<string, object>();

        public string PmxExportPath => Config.PmxExportDirPath + "\\" + ExportFileName + ".pmx";
        public string VmdExportPath => Config.VmdExportDirPath + "\\" + ExportFileName + ".vmd";

        public string ModelName { get; set; }
        public string ModelDescription { get; set; } = "This model is created by Curtain Fire Maker Plugin";

        public World(ShotTypeProvider typeProvider, PythonExecutor executor, Configuration config, IntPtr handle, string fileName)
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
            TaskScheduler.Frame();

            EntityList.AddRange(AddEntityList);
            RemoveEntityList.ForEach(e => EntityList.Remove(e));

            AddEntityList.Clear();
            RemoveEntityList.Clear();

            EntityList.ForEach(e => e.Frame());

            FrameCount++;
        }

        public void Init()
        {
            InitPre();
            Executor.ExecuteFileOnNewScope(Config.ScriptPath);
            InitPost();
        }

        public void GenerateCurainFire(Action<int> onFrame, Func<bool> isEnd)
        {
            long time = Environment.TickCount;

            if (RunWorld(onFrame, isEnd))
            {
                Console.WriteLine((Environment.TickCount - time) + "ms");
                Console.Out.Flush();

                try { DropFileToHandle(); } catch { }
            }
        }

        public bool RunWorld(Action<int> onFrame, Func<bool> isEnd)
        {
            for (int i = 0; i < MaxFrame; i++)
            {
                Frame();
                onFrame(i);

                if (isEnd()) { return false; }
            }
            Export();
            return true;
        }

        internal void Export()
        {
            EntityList.ForEach(e => e.OnDeath());

            KeyFrames.AddPropertyKeyFrame(new VmdPropertyFrameData(FrameCount + 1, false));

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

        public void AddTask(ScheduledTask task)
        {
            TaskScheduler.AddTask(task);
        }

        public void AddTask(PythonFunction task, Func<int, int> interval, int executeTimes, int waitTime, bool withArg = false)
        {
            AddTask(new ScheduledTask(t => withArg ? PythonCalls.Call(task, t) : PythonCalls.Call(task), interval, executeTimes, waitTime));
        }

        public void AddTask(PythonFunction task, int interval, int executeTimes, int waitTime, bool withArg = false)
        {
            AddTask(task, i => interval, executeTimes, waitTime, withArg);
        }
    }
}
