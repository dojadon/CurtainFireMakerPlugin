using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.ShotTypes;

namespace CurtainFireMakerPlugin.Entities
{
    internal class ShotManager
    {
        private Dictionary<ShotType, TimeLine> timelineMap = new Dictionary<ShotType, TimeLine>();
        private readonly World world;

        public ShotManager(World world)
        {
            this.world = world;
        }

        public void AddEntity(EntityShot entity)
        {
            if (!this.timelineMap.ContainsKey(entity.Property.Type))
            {
                this.timelineMap[entity.Property.Type] = new TimeLine(entity.Property.Type, this.world);
            }
            this.timelineMap[entity.Property.Type].AddEntity(entity);
        }
    }

    class TimeLine
    {
        private readonly ShotType type;
        private List<TimeLineRow> rowList = new List<TimeLineRow>();

        private readonly World world;

        public TimeLine(ShotType type, World world)
        {
            this.type = type;
            this.world = world;
        }

        public void AddEntity(EntityShot entity)
        {
            if (this.type.Equals(entity.Property.Type))
            {
                foreach (TimeLineRow r in this.rowList)
                {
                    if (r.AddEntity(entity)) { return; }
                }

                TimeLineRow row = new TimeLineRow(entity.Property, this.world);
                row.AddEntity(entity);
                this.rowList.Add(row);
            }
        }
    }

    class TimeLineRow
    {
        public readonly List<EntityShot> shotList = new List<EntityShot>();
        public readonly ShotModelData data;

        private readonly ShotProperty property;

        private readonly World world;

        public TimeLineRow(ShotProperty property, World world)
        {
            this.property = property;
            this.world = world;

            this.data = new ShotModelData(this.property);
            this.world.model.InitShotModelData(this.data);
        }

        private void SetupShot(EntityShot entity)
        {
            entity.rootBone = this.data.bones[0];
            entity.bones = this.data.bones;
            entity.materialMorph = this.data.morph;
        }

        public bool AddEntity(EntityShot entity)
        {
            if (this.property.Equals(entity.Property) && !this.shotList.Exists(e => !e.IsDeath))
            {
                this.shotList.Add(entity);
                this.SetupShot(entity);
                return true;
            }

            return false;
        }
    }
}
