using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace CurtainFireMakerPlugin.Entities
{
    internal class ShotModelDataProvider
    {
        private List<ShotGroup> GroupList { get; } = new List<ShotGroup>();
        private Dictionary<int, ShotGroup[]> CurrentGroupDict { get; set; }

        public void Frame()
        {
            CurrentGroupDict = GroupList.Where(g => g.ShotList.All(e => e.IsDeath && e.DeathFrameNo < e.World.FrameCount))
            .ToLookup(g => GetPropertyHashCode(g.Data.Property)).ToDictionary(g => g.Key, g => g.ToArray());
        }

        private int GetPropertyHashCode(ShotProperty property)
        {
            int result = 17;
            result = result * 23 + property.Color;
            result = result * 23 + property.Type.Id;
            return result;
        }

        public bool AddEntity(EntityShot entity, out ShotModelData data)
        {
            int hash = GetPropertyHashCode(entity.Property);

            if (CurrentGroupDict.ContainsKey(hash))
            {
                foreach (ShotGroup group in CurrentGroupDict[hash])
                {
                    if (group.IsAddable(entity))
                    {
                        group.AddEntity(entity);
                        data = group.Data;
                        return false;
                    }
                }
            }
            data = CreateGroup(entity);
            return true;
        }

        private ShotModelData CreateGroup(EntityShot entity)
        {
            ShotGroup group = new ShotGroup(entity);
            group.AddEntity(entity);
            GroupList.Add(group);

            return group.Data;
        }
    }

    internal class ShotGroup
    {
        public List<EntityShot> ShotList { get; } = new List<EntityShot>();
        public ShotModelData Data { get; }

        public ShotGroup(EntityShot entity)
        {
            Data = new ShotModelData(entity.World, entity.Property);
        }

        public bool IsAddable(EntityShot entity)
        {
            return ShotList.All(entity.IsGroupable);
        }

        public void AddEntity(EntityShot entity)
        {
            ShotList.Add(entity);
        }
    }
}
