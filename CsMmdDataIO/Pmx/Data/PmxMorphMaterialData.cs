using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsMmdDataIO.Pmx.Data
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
            exporter.WritePmxId(PmxExporter.SIZE_MATERIAL, Index);

            exporter.Write(CalcType);
            exporter.Write(Diffuse);
            exporter.Write(Specular);
            exporter.Write(Shininess);
            exporter.Write(Ambient);
            exporter.Write(Edge);
            exporter.Write(EdgeThick);
            exporter.Write(Texture);
            exporter.Write(SphereTexture);
            exporter.Write(ToonTexture);
        }

        public void Parse(PmxParser parser)
        {
            Index = parser.ReadPmxId(parser.SizeMaterial);

            CalcType = parser.ReadByte();
            Diffuse = parser.ReadVector4();
            Specular = parser.ReadVector3();
            Shininess = parser.ReadSingle();
            Ambient = parser.ReadVector3();
            Edge = parser.ReadVector4();
            EdgeThick = parser.ReadSingle();
            Texture = parser.ReadVector4();
            SphereTexture = parser.ReadVector4();
            ToonTexture = parser.ReadVector4();
        }
    }
}
