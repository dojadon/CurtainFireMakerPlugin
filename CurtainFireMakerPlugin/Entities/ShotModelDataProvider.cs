using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace CurtainFireMakerPlugin.Entities
{
    internal class ShotModelDataProvider
    {
        private HashSet<ShotGroup> GroupList { get; } = new HashSet<ShotGroup>();

        private Dictionary<int, List<ShotGroup>> ReusableGroupDict { get; set; } = new Dictionary<int, List<ShotGroup>>();

        private bool IsUpdated { get; set; }

        public ShotModelDataProvider()
        {
        }

        public void Frame()
        {
            IsUpdated = false;
        }

        public void Update()
        {
            GroupList.RemoveWhere(g =>
            {
                if (g.IsReusable)
                {
                    int hash = GetPropertyHashCode(g.Data.Property, g.ParentEntity);

                    if (!ReusableGroupDict.ContainsKey(hash))
                    {
                        ReusableGroupDict[hash] = new List<ShotGroup>();
                    }
                    ReusableGroupDict[hash].Add(g);
                }
                return g.IsReusable;
            });
            IsUpdated = true;
        }

        private int GetPropertyHashCode(ShotProperty prop, Entity entity)
        {
            return prop.GetHashCode() * 23 + (entity != null ? entity.EntityId : -1);
        }

        public void AddEntity(EntityShotBase entity, out ShotModelData data)
        {
            if (!IsUpdated)
            {
                Update();
            }

            int hash = GetPropertyHashCode(entity.Property, entity.ParentEntity);

            ShotGroup group = null;
            if (ReusableGroupDict.ContainsKey(hash))
            {
                var groupList = ReusableGroupDict[hash];

                group = groupList.FirstOrDefault();

                if (groupList.Count == 1)
                {
                    ReusableGroupDict.Remove(hash);
                }
                else
                {
                    groupList.RemoveAt(0);
                }
            }
            group = group ?? new ShotGroup(entity);

            group.SetEntity(entity);
            data = group.Data;

            GroupList.Add(group);
        }
    }

    internal class ShotGroup
    {
        public EntityShotBase CurrentEntity { get; set; }
        public ShotModelData Data { get; }
        public Entity ParentEntity { get; }

        public bool IsReusable => CurrentEntity.IsReusable;

        public ShotGroup(EntityShotBase entity)
        {
            Data = new ShotModelData(entity.World, entity.Property);
            ParentEntity = entity.ParentEntity;
        }

        public void SetEntity(EntityShotBase entity)
        {
            CurrentEntity = entity;
        }

        public override int GetHashCode()
        {
            return CurrentEntity.GetHashCode();
        }
    }
}
