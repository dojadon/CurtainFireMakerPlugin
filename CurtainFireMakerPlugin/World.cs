using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;
using CurtainFireMakerPlugin.Tasks;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        private List<Entity> addEntityList = new List<Entity>();
        private List<Entity> removeEntityList = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        private TaskManager taskManager = new TaskManager();

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
    }
}
