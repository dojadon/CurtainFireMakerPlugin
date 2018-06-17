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

        public void RegisterShotType(IEnumerable<ShotType> types)
        {
            types.ForEach(t => ShotTypeDict.Add(t.Name, t));
        }

        public ShotType GetShotType(string name)
        {
            if (!ShotTypeDict.ContainsKey(name))
            {
                throw new ArgumentException($"Not found key : {name}");
            }
            return ShotTypeDict[name];
        }
    }

    public abstract class ShotType
    {
        public static Vector4 KeyNormal { get; } = new Vector4(0.3F, 0.7F, 0.0F, 0.0F);
        public static Vector4 KeyAlpha { get; } = new Vector4(0.3F, 0.7F, 0.0F, 0.1F);
        public static Vector4 KeyMask { get; } = new Vector4(0.3F, 0.7F, 0.1F, 0.0F);
        public static Vector4 KeyBillboard { get; } = new Vector4(0.3F, 0.7F, 0.2F, 0.0F);
        public static Vector4 KeyBillboardAlpha { get; } = new Vector4(0.3F, 0.7F, 0.2F, 0.1F);
        public static Vector4 KeyBillboardShotL { get; } = new Vector4(0.3F, 0.7F, 0.2F, 0.2F);

        public string Name { get; }
        public Vector4 Key { get; }

        public abstract PmxModelData OriginalData { get; }

        public ShotType(string name, Vector4 key)
        {
            Name = name;
            Key = key;
        }

        public ShotType(string name) : this(name, KeyNormal) { }

        public virtual bool HasMesh { get; } = false;

        public virtual void InitEntity(EntityShotBase entity) { }

        public virtual void InitModelData(ShotProperty prop, PmxMaterialData[] materials)
        {
            if (materials != null)
            {
                foreach (var material in materials)
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
