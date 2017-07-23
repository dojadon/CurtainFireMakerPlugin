using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;
using CurtainFireMakerPlugin.Mathematics;
using CsPmx.Data;
using IronPython.Runtime;
using IronPython.Runtime.Operations;

namespace CurtainFireMakerPlugin.ShotTypes
{
    public abstract class ShotType
    {
        public String Name { get; }

        public ShotType(String name)
        {
            this.Name = name;
        }

        public virtual bool HasMmdData => true;
        public virtual bool RecordMotion => true;

        public virtual void Init(EntityShot entity)
        {
        }

        public abstract PmxVertexData[] GetVertices(ShotProperty property);

        public abstract int[] GetVertexIndices(ShotProperty property);

        public abstract PmxMaterialData[] GetMaterials(ShotProperty property);

        public abstract String[] GetTextures(ShotProperty property);

        public abstract PmxBoneData[] GetBones(ShotProperty property);
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

        public override bool HasMmdData => this.hasMmd;

        public override bool RecordMotion => this.recordMotion;

        public override PmxBoneData[] GetBones(ShotProperty property)
        {
            return new PmxBoneData[] { new PmxBoneData() };
        }

        public override PmxMaterialData[] GetMaterials(ShotProperty property)
        {
            return null;
        }

        public override string[] GetTextures(ShotProperty property)
        {
            return null;
        }

        public override int[] GetVertexIndices(ShotProperty property)
        {
            return null;
        }

        public override PmxVertexData[] GetVertices(ShotProperty property)
        {
            return null;
        }
    }
}
