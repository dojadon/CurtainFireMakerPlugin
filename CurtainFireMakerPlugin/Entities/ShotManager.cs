using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.ShotTypes;
using CurtainFireMakerPlugin.Collections;
using CsPmx.Data;
using CsPmx;

namespace CurtainFireMakerPlugin.Entities
{
    internal class ShotManager
    {
        private Dictionary<ShotType, ShotTypeGroup> TypeGroupMap { get; } = new Dictionary<ShotType, ShotTypeGroup>();
        private World World { get; }

        public ShotManager(World world)
        {
            this.World = world;
        }

        public ShotModelData AddEntity(EntityShot entity)
        {
            if (!this.TypeGroupMap.ContainsKey(entity.Property.Type))
            {
                this.TypeGroupMap[entity.Property.Type] = new ShotTypeGroup(entity.Property.Type, this.World);
            }

            ShotTypeGroup typeGroup = this.TypeGroupMap[entity.Property.Type];

            ShotModelData data = typeGroup.AddEntityToGroup(entity);
            if (data == null)
            {
                data = typeGroup.CreateGroup(entity);
                this.World.PmxModel.InitShotModelData(data);
            }

            return data;
        }

        public void Build()
        {
            foreach (var typeGroup in this.TypeGroupMap.Values)
            {
                typeGroup.Build();
            }
        }
    }

    internal class ShotTypeGroup
    {
        private ShotType Type { get; }
        private List<ShotGroup> GroupList { get; } = new List<ShotGroup>();

        private World World { get; }

        public ShotTypeGroup(ShotType type, World world)
        {
            this.Type = type;
            this.World = world;
        }

        public ShotModelData AddEntityToGroup(EntityShot entity)
        {
            foreach (ShotGroup group in this.GroupList)
            {
                if(group.IsAddable(entity))
                {
                    group.AddEntity(entity);
                    return group.Data;
                }
            }
            return null;
        }

        public ShotModelData CreateGroup(EntityShot entity)
        {
            ShotGroup group = new ShotGroup(entity.Property);
            group.AddEntity(entity);
            this.GroupList.Add(group);

            return group.Data;
        }

        public void Build()
        {
            if (!this.Type.HasMmdData())
            {
                return;
            }

            MultiDictionary<int[], ShotGroup> groupMap = new MultiDictionary<int[], ShotGroup>(new IntegerArrayEqualityComparer());
            var pmxModel = this.World.PmxModel;

            foreach (ShotGroup group in this.GroupList)
            {
                List<int> keyFrameNoList = new List<int>();

                HashSet<int> spawnFrameSet = new HashSet<int>();
                HashSet<int> deathFrameSet = new HashSet<int>();

                group.ShotList.ForEach(e => spawnFrameSet.Add(e.SpawnFrameNo));
                group.ShotList.ForEach(e => deathFrameSet.Add(e.DeathFrameNo));

                keyFrameNoList.AddRange(spawnFrameSet);
                keyFrameNoList.AddRange(deathFrameSet);

                keyFrameNoList.Sort();

                groupMap.Add(keyFrameNoList.ToArray(), group);
            }

            foreach (int[] keyFrameNo in groupMap.Keys)
            {
                ShotGroup[] groupArr = groupMap[keyFrameNo].ToArray();

                if (groupArr.Length > 1)
                {
                    for (int i = 1; i < groupArr.Length; i++)
                    {
                        pmxModel.morphList.Remove(groupArr[i].Data.Morph);
                    }

                    HashSet<int> indicesSet = new HashSet<int>();

                    foreach (ShotGroup group in groupArr)
                    {
                        foreach (var material in group.Data.Materials)
                        {
                            indicesSet.Add(pmxModel.materialList.IndexOf(material));
                        }
                    }

                    int[] indices = indicesSet.ToArray();
                    PmxMorphData morph = groupArr[0].Data.Morph;

                    morph.morphArray = ArrayUtil.Set(new PmxMorphMaterialData[indices.Length], i => new PmxMorphMaterialData());

                    for (int i = 0; i < indices.Length; i++)
                    {
                        morph.morphArray[i].Index = indices[i];
                    }
                }
            }
        }
    }

    internal class ShotGroup
    {
        public List<EntityShot> ShotList { get; } = new List<EntityShot>();
        public ShotModelData Data { get; }

        private ShotProperty Property { get; }

        public ShotGroup(ShotProperty property)
        {
            this.Property = property;

            this.Data = new ShotModelData(this.Property);
        }

        public bool IsAddable(EntityShot entity)
        {
            return this.Property.Equals(entity.Property) && !this.ShotList.Exists(e => !e.IsDeath);
        }

        public void AddEntity(EntityShot entity)
        {
            this.ShotList.Add(entity);
        }
    }

    internal class IntegerArrayEqualityComparer : IEqualityComparer<int[]>
    {
        public bool Equals(int[] x, int[] y)
        {
            if (x.Length != y.Length)
            {
                return false;
            }
            for (int i = 0; i < x.Length; i++)
            {
                if (x[i] != y[i])
                {
                    return false;
                }
            }
            return true;
        }

        public int GetHashCode(int[] obj)
        {
            int result = 17;
            for (int i = 0; i < obj.Length; i++)
            {
                unchecked
                {
                    result = result * 23 + obj[i];
                }
            }
            return result;
        }
    }
}
