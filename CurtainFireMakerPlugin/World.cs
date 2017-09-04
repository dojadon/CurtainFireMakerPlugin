using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MikuMikuPlugin;
using CurtainFireMakerPlugin.Entities;
using CurtainFireMakerPlugin.Entities.Models;
using CurtainFireMakerPlugin.Tasks;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        public int MaxFrame { get; set; } = 1000;
        public int StartFrame { get; set; } = 0;
        public Scene Scene => Plugin.Instance.Scene;

        private List<Entity> addEntityList = new List<Entity>();
        private List<Entity> removeEntityList = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        internal ShotManager ShotManager { get; }
        internal CurtainFireModel PmxModel { get; }
        internal CurtainFireMotion VmdMotion { get; }

        private TaskManager TaskManager { get; } = new TaskManager();

        public String ExportFileName { get; set; }

        public event EventHandler Export;

        internal World(string fileName)
        {
            ShotManager = new ShotManager(this);
            PmxModel = new CurtainFireModel(this);
            VmdMotion = new CurtainFireMotion(this);

            ExportFileName = fileName;

            Export += (obj, e) =>
            {
                PmxModel.Export(this);
                VmdMotion.Export(this);
            };
        }

        protected virtual void OnExport(EventArgs e)
        {
            Export?.Invoke(this, e);
        }

        internal ShotModelData AddShot(EntityShot entity)
        {
            return ShotManager.AddEntity(entity);
        }

        internal int AddEntity(Entity entity)
        {
            addEntityList.Add(entity);

            return FrameCount;
        }

        internal int RemoveEntity(Entity entity)
        {
            removeEntityList.Add(entity);

            return FrameCount;
        }

        internal void Init()
        {
            FrameCount = StartFrame;
        }

        internal void Frame()
        {
            TaskManager.Frame();

            EntityList.AddRange(addEntityList);
            removeEntityList.ForEach(e => EntityList.Remove(e));

            addEntityList.Clear();
            removeEntityList.Clear();

            EntityList.ForEach(e => e.Frame());

            FrameCount++;
        }

        internal void Finish()
        {
            EntityList.ForEach(e => e.OnDeath());
            PmxModel.Finish();

            OnExport(EventArgs.Empty);
        }

        public void AddTask(Task task)
        {
            TaskManager.AddTask(task);
        }

        public void AddTask(Action<Task> task, int interval, int executeTimes, int waitTime)
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
