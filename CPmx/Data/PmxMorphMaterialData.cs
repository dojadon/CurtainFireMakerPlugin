using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxMorphMaterialData : IPmxMorphTypeData
    {
        public int Index { get; set; }

        public byte CalcType { get; set; }
        public Vector4 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public float Shininess { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector4 Edge { get; set; }
        public float EdgeThick { get; set; }
        public Vector4 Texture { get; set; }
        public Vector4 SphereTexture { get; set; }
        public Vector4 ToonTexture { get; set; }

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_MATERIAL, this.Index);

            exporter.Write(this.CalcType);
            exporter.Write(this.Diffuse);
            exporter.Write(this.Specular);
            exporter.Write(this.Shininess);
            exporter.Write(this.Ambient);
            exporter.Write(this.Edge);
            exporter.Write(this.EdgeThick);
            exporter.Write(this.Texture);
            exporter.Write(this.SphereTexture);
            exporter.Write(this.ToonTexture);
        }

        public void Parse(PmxParser parser)
        {
            this.Index = parser.ReadPmxId(parser.SizeMaterial);

            this.CalcType = parser.ReadByte();
            this.Diffuse = parser.ReadVector4();
            this.Specular = parser.ReadVector3();
            this.Shininess = parser.ReadSingle();
            this.Ambient = parser.ReadVector3();
            this.Edge = parser.ReadVector4();
            this.EdgeThick = parser.ReadSingle();
            this.Texture = parser.ReadVector4();
            this.SphereTexture = parser.ReadVector4();
            this.ToonTexture = parser.ReadVector4();
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_MATERIAL;
        }
    }
}
