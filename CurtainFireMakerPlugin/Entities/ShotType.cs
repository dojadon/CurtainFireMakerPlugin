using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;
using MMDataIO.Pmx;

namespace CurtainFireMakerPlugin.Entities
{
    public class ShotTypeProvider
    {
        public Dictionary<string, ShotType> ShotTypeDict { get; } = new Dictionary<string, ShotType>();

        public void RegisterShotType(params ShotType[] types)
        {
            Array.ForEach(types, t => ShotTypeDict.Add(t.Name, t));
        }

        public ShotType GetShotType(string name) => ShotTypeDict[name];
    }

    public abstract class ShotType
    {
        public string Name { get; }

        public ShotType(string name)
        {
            Name = name;
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
