using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace CurtainFireMakerPlugin.Entities
{
    internal class ShotModelDataProvider
    {
        private HashSet<ShotGroup> GroupList { get; } = new HashSet<ShotGroup>();
        private Dictionary<int, ShotGroup[]> CurrentGroupDict { get; set; }

        public ShotModelDataProvider()
        {
            CurrentGroupDict = new Dictionary<int, ShotGroup[]>();
        }

        public void Frame()
        {
            CurrentGroupDict = GroupList.Where(g => g.ShotList.All(e => e.IsDeath))
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

            ShotGroup group = null;
            if (CurrentGroupDict.ContainsKey(hash))
            {
                group = CurrentGroupDict[hash].FirstOrDefault(g => g.IsAddable(entity));
            }
            if (group == null)
            {
                group = new ShotGroup(entity);
            }
            data = group.Data;
            group.AddEntity(entity);

            return GroupList.Add(group);
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
