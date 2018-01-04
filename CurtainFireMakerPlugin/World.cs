using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Entities;
using IronPython.Runtime;
using IronPython.Runtime.Operations;
using CsMmdDataIO.Vmd;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        public int MaxFrame { get; set; } = 1000;

        public Plugin Plugin { get; }
        public Scene Scene { get; }
        public Configuration Config { get; }
        internal PythonExecutor Executor { get; }

        public List<EntityCollisionObject> CollisonObjectList { get; private set; }

        private List<Entity> AddEntityList { get; } = new List<Entity>();
        private List<Entity> RemoveEntityList { get; } = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        internal ShotModelDataProvider ShotModelProvider { get; }
        internal CurtainFireModel PmxModel { get; }
        internal CurtainFireMotion KeyFrames { get; }

        private TaskManager TaskManager { get; } = new TaskManager();

        internal String ExportFileName { get; set; }

        public event EventHandler ExportEvent;

        internal string PmxExportPath => Config.PmxExportDirPath + "\\" + ExportFileName + ".pmx";
        internal string VmdExportPath => Config.VmdExportDirPath + "\\" + ExportFileName + ".vmd";

        public string ModelName { get; set; }
        public string ModelDescription { get; set; } = "This model is created by Curtain Fire Maker Plugin";

        internal World(Plugin plugin, string fileName)
        {
            Plugin = plugin;

            Scene = Plugin.Scene;
            Config = Plugin.Config;
            Executor = Plugin.PythonExecutor;

            ExportFileName = fileName;
            ModelName = ExportFileName;

            ShotModelProvider = new ShotModelDataProvider();
            PmxModel = new CurtainFireModel(this);
            KeyFrames = new CurtainFireMotion(this);
        }

        private void OnExport(EventArgs e)
        {
            ExportEvent?.Invoke(this, e);
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
            foreach (var type in ShotType.ShotTypeList)
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

            CollisonObjectList = EntityList.Where(e => e is EntityCollisionObject).Select(e => e as EntityCollisionObject).ToList();

            EntityList.ForEach(e => e.PreFrame());
            EntityList.ForEach(e => e.Frame());
            EntityList.ForEach(e => e.PostFrame());

            FrameCount++;
        }

        internal void Export()
        {
            EntityList.ForEach(e => e.OnDeath());

            PmxModel.FinalizeModel(KeyFrames.MorphFrameDict.Values.Select(t => t.frame));
            KeyFrames.FinalizeKeyFrame(PmxModel.Morphs.MorphList);

            PmxModel.Export();
            KeyFrames.Export();

            OnExport(EventArgs.Empty);
        }

        internal void DropFileToMMM()
        {
            if (Config.DropPmxFile)
            {
                Drop(Plugin.ApplicationForm.Handle, new StringCollection() { PmxExportPath });
            }

            if (Config.DropVmdFile && (Config.DropPmxFile || Scene.Models.Count > 0))
            {
                Drop(Plugin.ApplicationForm.Handle, new StringCollection() { VmdExportPath });
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

    public class Task
    {
        private Action<Task> State { get; set; }
        private Action<Task> Action { get; set; }

        public int Interval { get; set; }
        public int UpdateCount { get; set; }

        public int ExecutionTimes { get; set; }
        public int RunCount { get; set; }

        public int WaitTime { get; }
        public int WaitCount { get; set; }

        public Task(Action<Task> task, int interval, int executionTimes, int waitTime)
        {
            Action = task;
            Interval = UpdateCount = interval;
            ExecutionTimes = executionTimes;
            WaitTime = waitTime;

            State = waitTime > 0 ? WAITING : ACTIVE;
        }

        public void Update() => State(this);

        private void Run() => Action(this);

        public Boolean IsFinished() => State == FINISHED;

        private static Action<Task> WAITING = (task) =>
        {
            if (++task.WaitCount >= task.WaitTime - 1)
            {
                task.State = ACTIVE;
            }
        };
        private static Action<Task> ACTIVE = (task) =>
        {
            if (++task.UpdateCount >= task.Interval)
            {
                task.UpdateCount = 0;

                if (task.RunCount + 1 > task.ExecutionTimes && task.ExecutionTimes != 0)
                {
                    task.State = FINISHED;
                }
                else
                {
                    task.Run();
                    task.RunCount++;
                }
            }
        };
        private static Action<Task> FINISHED = (task) => { };
    }

    internal class TaskManager
    {
        private List<Task> TaskList { get; } = new List<Task>();
        private List<Task> AddTaskList { get; } = new List<Task>();

        public void AddTask(Task task)
        {
            AddTaskList.Add(task);
        }

        public void Frame()
        {
            TaskList.AddRange(AddTaskList);
            AddTaskList.Clear();

            TaskList.ForEach(task => task.Update());
            TaskList.RemoveAll(task => task.IsFinished());
        }

        public bool IsEmpty() => TaskList.Count == 0 && AddTaskList.Count == 0;
    }
}
