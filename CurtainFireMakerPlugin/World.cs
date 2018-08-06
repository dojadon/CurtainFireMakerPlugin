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
using VecMath;
using VecMath.Geometry;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        internal PythonExecutor Executor { get; }

        public RigidNode RootRigid { get; } = new RigidNode(AABoundingBox.Root);

        private List<Entity> AddEntityList { get; } = new List<Entity>();
        private List<Entity> RemoveEntityList { get; } = new List<Entity>();
        public HashSet<Entity> EntityList { get; } = new HashSet<Entity>();
        public int FrameCount { get; set; }

        public int StartFrame { get; }
        public int EndFrame { get; }

        public ShotTypeProvider ShotTypeProvider { get; }

        internal ShotModelDataProvider ShotModelProvider { get; }
        internal CurtainFireModel PmxModel { get; }
        internal CurtainFireSequence VmdSequence { get; }

        private ScheduledTaskManager TaskScheduler { get; } = new ScheduledTaskManager();

        public delegate void ExportEventHandler(object sender, ExportEventArgs args);
        public event ExportEventHandler ExportEvent;

        public string ExportFileName { get; set; }

        public World(ShotTypeProvider typeProvider, PythonExecutor executor, int startframe, int endframe)
        {
            ShotTypeProvider = typeProvider;
            Executor = executor;
            StartFrame = startframe;
            EndFrame = endframe;

            ShotModelProvider = new ShotModelDataProvider();
            PmxModel = new CurtainFireModel(this);
            VmdSequence = new CurtainFireSequence(this);

            foreach (var type in ShotTypeProvider.ShotTypeDict.Values)
            {
                type.InitWorld(this);
            }

            if (0 < StartFrame)
            {
                VmdSequence.AddPropertyKeyFrame(new VmdPropertyFrameData(0, false));
                VmdSequence.AddPropertyKeyFrame(new VmdPropertyFrameData(StartFrame, true));
            }

            ExportEvent += (sender, e) =>
            {
                PmxModel.Export(e.Script, e.PmxExportPath, ExportFileName, "by CurtainFireMakerPlugin");
                VmdSequence.Export(e.Script, e.VmdExportPath, ExportFileName);
            };
        }

        public void AddRigidNode(RigidNode rigid)
        {
            RootRigid.ChildList.Add(rigid);
        }

        internal ShotModelData AddShot(EntityShotBase entity)
        {
            ShotModelProvider.AddEntity(entity, out ShotModelData data);

            if (!data.IsInitialized)
            {
                PmxModel.InitShotModelData(data);
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
            if (entity.IsNeededUpdate)
            {
                RemoveEntityList.Add(entity);
            }

            return FrameCount;
        }

        private System.Diagnostics.Stopwatch Stopwatch { get; } = new System.Diagnostics.Stopwatch();

        public void Run(Action action, string msg)
        {
            Stopwatch.Reset();
            Stopwatch.Start();

            action();

            Stopwatch.Stop();
            Console.WriteLine(msg + " : {0:#,0}ns", Stopwatch.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency * 1E+06);
        }

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

        public void FinalizeWorld()
        {
            EntityList.ForEach(e => e.Remove(true));
            VmdSequence.AddPropertyKeyFrame(new VmdPropertyFrameData(FrameCount + 1, false));
        }

        internal void Export(dynamic script, string directory)
        {
            PmxModel.FinalizeModel(VmdSequence.MorphFrameDict.Values.Select(t => t.frame));
            VmdSequence.FinalizeKeyFrame(PmxModel.ModelData.MorphArray);

            ExportEvent?.Invoke(this, new ExportEventArgs(Path.Combine(directory, ExportFileName + ".pmx"), Path.Combine(directory, ExportFileName + ".vmd")) { Script = script });
        }

        internal void DropFileToHandle(IntPtr handle, dynamic script, string directory)
        {
            if (PmxModel.ShouldDrop(script))
            {
                Drop(handle, new StringCollection() { Path.Combine(directory, ExportFileName + ".pmx") });

                if (VmdSequence.ShouldDrop(script))
                {
                    Drop(handle, new StringCollection() { Path.Combine(directory, ExportFileName + ".vmd") });
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
        public dynamic Script { get; set; }
        public string PmxExportPath { get; }
        public string VmdExportPath { get; }

        public ExportEventArgs(string pmx, string vmd)
        {
            PmxExportPath = pmx;
            VmdExportPath = vmd;
        }
    }
}
