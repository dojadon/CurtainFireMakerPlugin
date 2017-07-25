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
        public Action<ShotProperty, PmxMaterialData[]> InitMaterials { get; set; } = (prop, materials) =>
        {
            foreach (var material in materials)
            {
                material.Diffuse = new Vector4(prop.Red, prop.Green, prop.Blue, 1);
                material.Ambient = new Vector3(prop.Red, prop.Green, prop.Blue);
            }
        };

        public abstract PmxVertexData[] CreateVertices();

        public abstract int[] CreateVertexIndices();

        public abstract PmxMaterialData[] CreateMaterials();

        public abstract String[] CreateTextures();

        public abstract PmxBoneData[] CreateBones();
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

        public override PmxBoneData[] CreateBones()
        {
            return new PmxBoneData[] { new PmxBoneData() };
        }

        public override PmxMaterialData[] CreateMaterials()
        {
            return null;
        }

        public override string[] CreateTextures()
        {
            return null;
        }

        public override int[] CreateVertexIndices()
        {
            return null;
        }

        public override PmxVertexData[] CreateVertices()
        {
            return null;
        }
    }
}
