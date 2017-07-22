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
            this.World = world;
        }

        public ShotModelData AddEntity(EntityShot entity)
        {
            if (!this.TypeGroupDict.ContainsKey(entity.Property.Type))
            {
                this.TypeGroupDict[entity.Property.Type] = new ShotTypeGroup(entity.Property.Type, this.World);
            }

            ShotTypeGroup typeGroup = this.TypeGroupDict[entity.Property.Type];

            ShotModelData data = typeGroup.AddEntityToGroup(entity);
            if (data == null)
            {
                data = typeGroup.CreateGroup(entity);
                this.World.PmxModel.InitShotModelData(data);
            }

            return data;
        }

        public void CompressMorph()
        {
            foreach (var typeGroup in this.TypeGroupDict.Values)
            {
                typeGroup.Compress();
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
            ShotGroup group = new ShotGroup(entity.Property);
            group.AddEntity(entity);
            this.GroupList.Add(group);

            return group.Data;
        }

        public void Compress()
        {
            if (!this.Type.HasMmdData())
            {
                return;
            }

            var pmxModel = this.World.PmxModel;
            var vmdMotion = this.World.VmdMotion;

            var typeMorphDict = new MultiDictionary<byte, PmxMorphData>();
            foreach (var morph in vmdMotion.MorphDict.Keys)
            {
                typeMorphDict.Add(morph.Type, morph);
            }

            foreach (var morphList in typeMorphDict.Values)
            {
                this.Compress(morphList, vmdMotion.MorphDict);
            }
        }

        private void Compress(List<PmxMorphData> morphList, MultiDictionary<PmxMorphData, VmdMorphFrameData> frameDataDict)
        {
            var dict = new MultiDictionary<int[], PmxMorphData>(new IntegerArrayComparer());

            foreach (var morph in morphList)
            {
                dict.Add(Array.ConvertAll(frameDataDict[morph].ToArray(), m => m.KeyFrameNo), morph);
            }

            foreach (var key in dict.Keys)
            {
                var removeList = dict[key];

                if (removeList.Count > 1)
                {
                    PmxMorphData addMoroh = this.Compress(removeList);
                    removeList.Remove(addMoroh);

                    foreach (var morph in removeList)
                    {
                        World.PmxModel.MorphList.Remove(morph);
                    }
                }
            }
        }

        private PmxMorphData Compress(List<PmxMorphData> morphList)
        {
            var morphTypeDataList = new List<IPmxMorphTypeData>();
            foreach (var morph in morphList)
            {
                morphTypeDataList.AddRange(morph.MorphArray);
            }

            morphList[0].MorphArray = morphTypeDataList.ToArray();

            return morphList[0];
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

    internal class IntegerArrayComparer : IEqualityComparer<int[]>
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
