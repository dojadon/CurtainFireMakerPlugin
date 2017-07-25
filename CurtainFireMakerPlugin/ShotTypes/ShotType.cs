using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CurtainFireMakerPlugin.Entities;
using VecMath;
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

        public Action<Entity> Init { get; set; } = e => { };

        public abstract PmxVertexData[] CreateVertices(ShotProperty property);

        public abstract int[] CreateVertexIndices(ShotProperty property);

        public abstract PmxMaterialData[] CreateMaterials(ShotProperty property);

        public abstract String[] CreateTextures(ShotProperty property);

        public abstract PmxBoneData[] CreateBones(ShotProperty property);
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

        public override PmxBoneData[] CreateBones(ShotProperty property)
        {
            return new PmxBoneData[] { new PmxBoneData() };
        }

        public override PmxMaterialData[] CreateMaterials(ShotProperty property)
        {
            return null;
        }

        public override string[] CreateTextures(ShotProperty property)
        {
            return null;
        }

        public override int[] CreateVertexIndices(ShotProperty property)
        {
            return null;
        }

        public override PmxVertexData[] CreateVertices(ShotProperty property)
        {
            return null;
        }
    }
}
