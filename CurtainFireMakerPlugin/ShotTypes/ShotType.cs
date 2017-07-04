using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;
using CPmx.Data;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public abstract class ShotType
    {
        private String Name { get; }

        private Action<EntityShot> frameFunc = entity =>
        {

        };

        public ShotType(String name)
        {
            this.Name = name;
        }

        public virtual bool HasMmdData()
        {
            return true;
        }

        public virtual bool RecordMotion()
        {
            return true;
        }

        public void Frame(EntityShot entity)
        {
            this.frameFunc.Invoke(entity);
        }

        public abstract PmxVertexData[] GetVertices(EntityShot entity);

        public abstract int[] GetVertexIndices(EntityShot entity);

        public abstract PmxMaterialData[] GetMaterials(EntityShot entity);

        public abstract String[] GetTextures(EntityShot entity);

        public abstract PmxBoneData[] GetBones(EntityShot entity);
    }

    public class ShotTypeNone : ShotType
    {
        private readonly bool hasMmd;
        private readonly bool recordMotion;

        public ShotTypeNone(string name, bool hasMmd, bool recordMotion) : base(name)
        {
            this.hasMmd = hasMmd;
            this.recordMotion = recordMotion;
        }

        public override bool HasMmdData()
        {
            return this.hasMmd;
        }

        public override bool RecordMotion()
        {
            return this.recordMotion;
        }

        public override PmxBoneData[] GetBones(EntityShot entity)
        {
            return null;
        }

        public override PmxMaterialData[] GetMaterials(EntityShot entity)
        {
            return null;
        }

        public override string[] GetTextures(EntityShot entity)
        {
            return null;
        }

        public override int[] GetVertexIndices(EntityShot entity)
        {
            return null;
        }

        public override PmxVertexData[] GetVertices(EntityShot entity)
        {
            return null;
        }
    }
}
