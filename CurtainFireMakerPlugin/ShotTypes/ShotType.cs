using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;
using VecMath;
using CsMmdDataIO.Pmx.Data;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public abstract class ShotType
    {
        public static List<ShotType> ShotTypeList { get; } = new List<ShotType>();

        public int Id { get; }
        private static int NextId { get; set; }

        public ShotType()
        {
            Id = NextId++;
            ShotTypeList.Add(this);
        }

        public virtual bool HasMesh { get; } = false;

        public virtual void InitEntity(EntityShot entity) { }

        public virtual void InitModelData(ShotModelData data)
        {
            if (data.Materials != null)
            {
                var prop = data.Property;
                foreach (var material in data.Materials)
                {
                    material.Diffuse = new Vector4(prop.Red, prop.Green, prop.Blue, 1);
                    material.Ambient = new Vector3(prop.Red, prop.Green, prop.Blue);
                }
            }
        }

        public virtual void InitWorld(World world) { }

        public abstract PmxBoneData[] CreateBones(World wolrd, ShotProperty prop);

        public abstract PmxMaterialData[] CreateMaterials(World wolrd, ShotProperty prop);

        public abstract string[] CreateTextures(World wolrd, ShotProperty prop);

        public abstract int[] CreateVertexIndices(World wolrd, ShotProperty prop);

        public abstract PmxVertexData[] CreateVertices(World wolrd, ShotProperty prop);
    }
}
