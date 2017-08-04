using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.ShotTypes;
using CurtainFireMakerPlugin.Collections;
using CsPmx.Data;
using CsPmx;
using CsVmd.Data;

namespace CurtainFireMakerPlugin.Entities
{
    internal class ShotManager
    {
        private Dictionary<ShotType, ShotTypeGroup> TypeGroupDict { get; } = new Dictionary<ShotType, ShotTypeGroup>();
        private World World { get; }

        public ShotManager(World world)
        {
            World = world;
        }

        public ShotModelData AddEntity(EntityShot entity)
        {
            if (!TypeGroupDict.ContainsKey(entity.Property.Type))
            {
                TypeGroupDict[entity.Property.Type] = new ShotTypeGroup(entity.Property.Type, World);
            }

            ShotTypeGroup typeGroup = TypeGroupDict[entity.Property.Type];

            ShotModelData data = typeGroup.AddEntityToGroup(entity);
            if (data == null)
            {
                data = typeGroup.CreateGroup(entity);
                World.PmxModel.InitShotModelData(data);
            }

            return data;
        }
    }

    internal class ShotTypeGroup
    {
        private ShotType Type { get; }
        private List<ShotGroup> GroupList { get; } = new List<ShotGroup>();

        private World World { get; }

        public ShotTypeGroup(ShotType type, World world)
        {
            Type = type;
            World = world;
        }

        public ShotModelData AddEntityToGroup(EntityShot entity)
        {
            foreach (ShotGroup group in GroupList)
            {
                if (group.IsAddable(entity))
                {
                    group.AddEntity(entity);
                    return group.Data;
                }
            }
            return null;
        }

        public ShotModelData CreateGroup(EntityShot entity)
        {
            ShotGroup group = new ShotGroup(entity.Property, World);
            group.AddEntity(entity);
            GroupList.Add(group);

            return group.Data;
        }
    }

    internal class ShotGroup
    {
        public List<EntityShot> ShotList { get; } = new List<EntityShot>();
        public ShotModelData Data { get; }

        private ShotProperty Property { get; }
        private World World { get; }

        public ShotGroup(ShotProperty property, World world)
        {
            Property = property;
            World = world;

            Data = new ShotModelData(World, Property);
        }

        public bool IsAddable(EntityShot entity)
        {
            return Property.Equals(entity.Property) && !ShotList.Exists(e => !e.IsDeath);
        }

        public void AddEntity(EntityShot entity)
        {
            ShotList.Add(entity);
        }
    }
}
