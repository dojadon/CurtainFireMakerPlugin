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
        private static List<World> worldList = new List<World>();
        public static List<World> WorldList => worldList;

        public static int MAX_FRAME = 1000;

        private List<Entity> addEntityList = new List<Entity>();
        private List<Entity> removeEntityList = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        internal readonly ShotManager shotManager;
        internal readonly CurtainFireModel model;
        internal readonly CurtainFireMotion motion;

        private readonly TaskManager taskManager = new TaskManager();

        public World()
        {
            worldList.Add(this);

            shotManager = new ShotManager(this);
            model = new CurtainFireModel();
            motion = new CurtainFireMotion();
        }

        internal void AddShot(EntityShot entity)
        {
            this.shotManager.AddEntity(entity);
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
            this.shotManager.Build();
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
