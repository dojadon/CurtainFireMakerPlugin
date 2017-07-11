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
        public static World Instance { get; set; }

        public static int MAX_FRAME = 1000;

        private List<Entity> addEntityList = new List<Entity>();
        private List<Entity> removeEntityList = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        private TaskManager taskManager = new TaskManager();

        internal readonly ShotManager shotManager;
        internal readonly CurtainFireModel model;
        internal readonly CurtainFireMotion motion;

        public World()
        {
            Instance = this;

            shotManager = new ShotManager(this);
            model = new CurtainFireModel();
            motion = new CurtainFireMotion();
        }

        public void StartWorld(Action<int> action)
        {
            for (int i = 0; i < MAX_FRAME; i++)
            {
                this.Frame();
                action(this.FrameCount);
            }
            this.EntityList.ForEach(e => e.OnDeath());

            this.shotManager.Build();
        }

        public void AddShot(EntityShot entity)
        {
            this.shotManager.AddEntity(entity);
        }

        public int AddEntity(Entity entity)
        {
            this.addEntityList.Add(entity);

            return this.FrameCount;
        }

        public int RemoveEntity(Entity entity)
        {
            this.removeEntityList.Add(entity);

            return this.FrameCount;
        }

        public void Frame()
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

        public void AddTask(Task task)
        {
            this.taskManager.AddTask(task);
        }

        public void AddTask(Action task, int interval, int executeTimes, int waitTime)
        {
            this.AddTask(new Task(task, interval, executeTimes, waitTime));
        }

        public void AddTask(PythonFunction task, int interval, int executeTimes, int waitTime)
        {
            this.AddTask(() => PythonCalls.Call(task), interval, executeTimes, waitTime);
        }
    }
}
