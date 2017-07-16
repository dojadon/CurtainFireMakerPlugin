using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;

namespace CurtainFireMakerPlugin
{
    public class World
    {
        private static List<World> worldList = new List<World>();
        public static List<World> WorldList => WorldList;

        public static int MAX_FRAME = 1000;

        private List<Entity> addEntityList = new List<Entity>();
        private List<Entity> removeEntityList = new List<Entity>();
        public List<Entity> EntityList { get; } = new List<Entity>();
        public int FrameCount { get; set; }

        internal readonly ShotManager shotManager;
        internal readonly CurtainFireModel model;
        internal readonly CurtainFireMotion motion;

        public World()
        {
            worldList.Add(this);

            shotManager = new ShotManager(this);
            model = new CurtainFireModel();
            motion = new CurtainFireMotion();
        }

        internal void StartWorld(Action<int> action)
        {
            this.shotManager.Build();
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

        internal bool Frame()
        {
            this.EntityList.AddRange(this.addEntityList);
            this.removeEntityList.ForEach(e => this.EntityList.Remove(e));

            this.addEntityList.Clear();
            this.removeEntityList.Clear();

            this.EntityList.ForEach(e => e.Frame());
            this.EntityList.RemoveAll(e => e.IsDeath);

            this.FrameCount++;

            return this.EntityList.Count != 0 && this.addEntityList.Count != 0;
        }

        internal void Finish()
        {
            this.shotManager.Build();
        }
    }
}
