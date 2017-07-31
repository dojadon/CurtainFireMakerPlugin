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

        public virtual Action<EntityShot> Init { get; set; } = e => { };
        public Action<ShotModelData> InitModelData { get; set; } = data =>
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
         };

        public abstract PmxVertexData[] CreateVertices(World wolrd, ShotProperty prop);

        public abstract int[] CreateVertexIndices(World wolrd, ShotProperty prop);

        public abstract PmxMaterialData[] CreateMaterials(World wolrd, ShotProperty prop);

        public abstract String[] CreateTextures(World wolrd, ShotProperty prop);

        public abstract PmxBoneData[] GetCreateBones(World wolrd, ShotProperty prop);
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

        public override PmxBoneData[] GetCreateBones(World wolrd, ShotProperty prop) => new PmxBoneData[] { new PmxBoneData() };

        public override PmxMaterialData[] CreateMaterials(World wolrd, ShotProperty prop)
        {
            return null;
        }

        public override string[] CreateTextures(World wolrd, ShotProperty prop)
        {
            return null;
        }

        public override int[] CreateVertexIndices(World wolrd, ShotProperty prop)
        {
            return null;
        }

        public override PmxVertexData[] CreateVertices(World wolrd, ShotProperty prop)
        {
            return null;
        }
    }
}
