using System;
using System.Collections.Generic;
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

        public void Build()
        {
            foreach (var timeline in this.timelineMap.Values)
            {
                timeline.Build();
            }
        }
    }

    internal class TimeLine
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

        public void Build()
        {
            if (!this.type.HasMmdData())
            {
                return;
            }

            MultiDictionary<int[], TimeLineRow> rowMap = new MultiDictionary<int[], TimeLineRow>();

            var pmxModel = this.world.model;

            foreach (TimeLineRow row in this.rowList)
            {
                List<int> keyFrameNoSet = new List<int>();

                HashSet<int> spawnFrameSet = new HashSet<int>();
                HashSet<int> deathFrameSet = new HashSet<int>();

                row.shotList.ForEach(e => spawnFrameSet.Add(e.SpawnFrameNo));
                row.shotList.ForEach(e => deathFrameSet.Add(e.DeathFrameNo));

                keyFrameNoSet.AddRange(spawnFrameSet);
                keyFrameNoSet.AddRange(deathFrameSet);

                keyFrameNoSet.Sort();

                rowMap.Add(keyFrameNoSet.ToArray(), row);
            }

            foreach (int[] keyFrameNo in rowMap.Keys)
            {
                TimeLineRow[] rowArr = rowMap[keyFrameNo].ToArray();

                if (rowArr.Length > 1)
                {
                    for (int i = 1; i < rowArr.Length; i++)
                    {
                        pmxModel.morphList.Remove(rowArr[i].data.morph);
                    }

                    HashSet<int> indicesSet = new HashSet<int>();

                    foreach (TimeLineRow row in rowArr)
                    {
                        foreach (var material in row.data.materials)
                        {
                            indicesSet.Add(pmxModel.materialList.IndexOf(material));
                        }
                    }

                    int[] indices = indicesSet.ToArray();
                    PmxMorphData morph = rowArr[0].data.morph;

                    morph.morphArray = ArrayUtil.Set(new PmxMorphMaterialData[indices.Length], i => new PmxMorphMaterialData());

                    for (int i = 0; i < indices.Length; i++)
                    {
                        morph.morphArray[i].Index = indices[i];
                    }
                }
            }
        }
    }

    internal class TimeLineRow
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
