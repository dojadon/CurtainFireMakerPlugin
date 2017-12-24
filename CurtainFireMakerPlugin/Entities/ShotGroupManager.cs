using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace CurtainFireMakerPlugin.Entities
{
    internal class ShotModelDataProvider
    {
        private List<ShotGroup> GroupList { get; } = new List<ShotGroup>();

        public bool AddEntity(EntityShot entity, out ShotModelData data)
        {
            data = AddEntityToGroup(entity);
            if (data == null)
            {
                data = CreateGroup(entity);
                return true;
            }
            return false;
        }

        private ShotModelData AddEntityToGroup(EntityShot entity)
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
            return !ShotList.Exists(e => !entity.IsGroupable(e));
        }

        public void AddEntity(EntityShot entity)
        {
            ShotList.Add(entity);
        }
    }
}
