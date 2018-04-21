using System;
using System.Linq;
using System.IO;
using System.Dynamic;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Threading.Tasks;
using CurtainFireMakerPlugin.Entities;
using IronPython.Runtime;
using IronPython.Runtime.Operations;
using MMDataIO.Vmd;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        public int MaxFrame { get; set; } = 1000;

        internal PythonExecutor Executor { get; }
        internal IntPtr HandleToDrop { get; }

        public dynamic Script { get; set; }

        public List<RigidObject> RigidObjectList { get; } = new List<RigidObject>();

        private List<Entity> AddEntityList { get; } = new List<Entity>();
        private List<Entity> RemoveEntityList { get; } = new List<Entity>();
        public HashSet<Entity> EntityList { get; } = new HashSet<Entity>();
        public int FrameCount { get; set; }

        public ShotTypeProvider ShotTypeProvider { get; }

        internal ShotModelDataProvider ShotModelProvider { get; }
        internal CurtainFireModel PmxModel { get; }
        internal CurtainFireMotion KeyFrames { get; }

        private ScheduledTaskManager TaskScheduler { get; } = new ScheduledTaskManager();

        public delegate void ExportEventHandler(object sender, ExportEventArgs args);
        public event ExportEventHandler ExportEvent;

        public World(ShotTypeProvider typeProvider, PythonExecutor executor, IntPtr handle)
        {
            ShotTypeProvider = typeProvider;
            Executor = executor;
            HandleToDrop = handle;

            ShotModelProvider = new ShotModelDataProvider();
            PmxModel = new CurtainFireModel(this);
            KeyFrames = new CurtainFireMotion(this);
        }

        public void AddRigidObject(RigidObject rigid)
        {
            RigidObjectList.Add(rigid);
        }

        private bool IsContainsShot { get; set; } = false;

        internal ShotModelData AddShot(EntityShotBase entity)
        {
            ShotModelProvider.AddEntity(entity, out ShotModelData data);

            if (!data.IsInitialized)
            {
                PmxModel.InitShotModelData(data);
            }

            if (!IsContainsShot)
            {
                KeyFrames.AddPropertyKeyFrame(new VmdPropertyFrameData(0, false));
                KeyFrames.AddPropertyKeyFrame(new VmdPropertyFrameData(FrameCount, true));
                IsContainsShot = true;
            }

            return data;
        }

        internal int AddEntity(Entity entity)
        {
            if (entity.IsNeededUpdate)
            {
                AddEntityList.Add(entity);
            }

            return FrameCount;
        }

        internal int RemoveEntity(Entity entity)
        {
            RemoveEntityList.Add(entity);

            return FrameCount;
        }

        private System.Diagnostics.Stopwatch Stopwatch { get; } = new System.Diagnostics.Stopwatch();

        internal void Frame()
        {
            ShotModelProvider.Frame();
            TaskScheduler.Frame();

            AddEntityList.ForEach(e => EntityList.Add(e));
            RemoveEntityList.ForEach(e => EntityList.Remove(e));

            AddEntityList.Clear();
            RemoveEntityList.Clear();

            EntityList.ForEach(e => e.Frame());

            FrameCount++;
        }

        public void Run(Action action, string msg)
        {
            Stopwatch.Reset();
            Stopwatch.Start();

            action();

            Stopwatch.Stop();
            Console.WriteLine(msg + " : {0:#,0}ns", Stopwatch.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency * 1E+06);
        }

        public void Init()
        {
            foreach (var type in ShotTypeProvider.ShotTypeDict.Values)
            {
                type.InitWorld(this);
            }
        }

        public void GenerateCurainFire(Func<int, bool> onFrame, string pmxExportPath, string vmdExportPath)
        {
            long time = Environment.TickCount;

            if (RunWorld(onFrame, pmxExportPath, vmdExportPath))
            {
                Console.WriteLine((Environment.TickCount - time) + "ms");
                Console.Out.Flush();

                try { DropFileToHandle(pmxExportPath, vmdExportPath); } catch { }
            }
        }

        public bool RunWorld(Func<int, bool> onFrame, string pmxExportPath, string vmdExportPath)
        {
            for (int i = 0; i < MaxFrame; i++)
            {
                Frame();

                if (onFrame(i)) { return false; }
            }
            Export(pmxExportPath, vmdExportPath);
            return true;
        }

        internal void Export(string pmxPath, string vmdPath)
        {
            EntityList.ForEach(e => e.Remove(true));

            KeyFrames.AddPropertyKeyFrame(new VmdPropertyFrameData(FrameCount + 1, false));

            PmxModel.FinalizeModel(KeyFrames.MorphFrameDict.Values.Select(t => t.frame));
            KeyFrames.FinalizeKeyFrame(PmxModel.Morphs.MorphList);

            PmxModel.Export(pmxPath, Path.GetFileNameWithoutExtension(pmxPath), "by CurtainFireMakerPlugin");
            KeyFrames.Export(vmdPath, Path.GetFileNameWithoutExtension(vmdPath));

            ExportEvent?.Invoke(this, new ExportEventArgs(pmxPath, vmdPath));
        }

        internal void DropFileToHandle(string pmxPath, string vmdPath)
        {
            if (PmxModel.ShouldDrop())
            {
                Drop(HandleToDrop, new StringCollection() { pmxPath });

                if (KeyFrames.ShouldDrop())
                {
                    Drop(HandleToDrop, new StringCollection() { vmdPath });
                }
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

        private void AddTask(ScheduledTask task)
        {
            TaskScheduler.AddTask(task);
        }

        private void AddTask(PythonFunction task, Func<int, int> interval, int executeTimes, int waitTime, bool withArg = false)
        {
            if (withArg)
            {
                AddTask(new ScheduledTask(t => PythonCalls.Call(task, t), interval, executeTimes, waitTime));
            }
            else
            {
                AddTask(new ScheduledTask(t => PythonCalls.Call(task), interval, executeTimes, waitTime));
            }
        }

        public void AddTask(PythonFunction task, PythonFunction interval, int executeTimes, int waitTime, bool withArg = false)
        {
            AddTask(task, i => (int)PythonCalls.Call(interval, i), executeTimes, waitTime, withArg);
        }

        public void AddTask(PythonFunction task, int interval, int executeTimes, int waitTime, bool withArg = false)
        {
            AddTask(task, i => interval, executeTimes, waitTime, withArg);
        }
    }

    public class ExportEventArgs : EventArgs
    {
        public string PmxExportPath { get; }
        public string VmdExportPath { get; }

        public ExportEventArgs(string pmx, string vmd)
        {
            PmxExportPath = pmx;
            VmdExportPath = vmd;
        }
    }
}
