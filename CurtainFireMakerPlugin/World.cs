using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;
using CurtainFireMakerPlugin.Tasks;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        public static List<World> WorldList { get; } = new List<World>();

        public static int MaxFrame { get; set; } = 1000;

        private List<Entity> addEntityList = new List<Entity>();
        private List<Entity> removeEntityList = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        internal ShotManager ShotManager { get; }
        internal CurtainFireModel PmxModel { get; }
        internal CurtainFireMotion VmdMotion { get; }

        private readonly TaskManager taskManager = new TaskManager();

        public World()
        {
            WorldList.Add(this);

            ShotManager = new ShotManager(this);
            PmxModel = new CurtainFireModel();
            VmdMotion = new CurtainFireMotion();
        }

        internal ShotModelData AddShot(EntityShot entity)
        {
            return this.ShotManager.AddEntity(entity);
        }

        internal int AddEntity(Entity entity)
        {
            this.addEntityList.Add(entity);

            return this.FrameCount;
        }

        internal int RemoveEntity(Entity entity)
        {
            this.removeEntityList.Add(entity);

            return this.FrameCount;
        }

        internal void Frame()
        {
            this.taskManager.Frame();

            this.EntityList.AddRange(this.addEntityList);
            this.removeEntityList.ForEach(e => this.EntityList.Remove(e));

            this.addEntityList.Clear();
            this.removeEntityList.Clear();

            this.EntityList.ForEach(e => e.Frame());
            this.EntityList.RemoveAll(e => e.IsDeath);

            this.FrameCount++;
        }

        internal void Finish()
        {
            this.ShotManager.CompressMorph();
            this.VmdMotion.Finish(PmxModel);
        }

        public void AddTask(Task task)
        {
            this.taskManager.AddTask(task);
        }

        public void AddTask(Action<Task> task, int interval, int executeTimes, int waitTime)
        {
            this.AddTask(new Task(task, interval, executeTimes, waitTime));
        }

        public void AddTask(PythonFunction func, int interval, int executeTimes, int waitTime, bool withArg = false)
        {
            if (withArg)
            {
                this.AddTask(task => PythonCalls.Call(func, task), interval, executeTimes, waitTime);
            }
            else
            {
                this.AddTask(task => PythonCalls.Call(func), interval, executeTimes, waitTime);
            }
        }
    }
}
