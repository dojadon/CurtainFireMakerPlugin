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
            ShotTypeDict.Clear();
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
        public string Name { get; }

        public abstract PmxModelData OriginalData { get; }

        public ShotType(string name)
        {
            Name = name;
        }

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

        public abstract PmxMorphData[] CreateMorphs(World world, ShotProperty prop);

        public virtual bool IsButterfly() => false;
        public virtual bool IsBillboard() => false;
        public virtual bool IsMasked() => false;

        public Vector4 GetExtraUv1() => new Vector4(487, IsButterfly() ? 2 : (IsBillboard() ? 1 : 0), GetRotationAngle(), GetRotationSpeed());

        public virtual bool IsUseTexture() => false;
        public virtual bool IsUseTexture_L() => false;

        public virtual float GetAlphaALObject() => 1;
        public virtual float GetAlphaALScn() => 1;

        public virtual float GetRotationAngle() => 0;
        public virtual float GetRotationSpeed() => 0;

        public Vector4 GetExtraUv2() => new Vector4(GetAlphaALObject(), GetAlphaALScn(), IsUseTexture_L() ? 2 : (IsUseTexture() ? 1 : 0), IsMasked() ? 1 : 0);

        public Vector4 GetExtraUv3() => new Vector4(0, 0, 0, 0);

        public Vector4 GetExtraUv4() => new Vector4(0, 0, 0, 0);
    }
}
