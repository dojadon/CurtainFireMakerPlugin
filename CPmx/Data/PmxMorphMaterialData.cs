using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    [Serializable]
    public class PmxMorphMaterialData : IPmxMorphTypeData
    {
        public int Index { get; set; }

        public byte calcType;
        public Vector4 diffuse = new Vector4();
        public Vector3 specular = new Vector3();
        public float shininess;
        public Vector3 ambient = new Vector3();
        public Vector4 edge = new Vector4();
        public float edgeThick;
        public Vector4 texture = new Vector4();
        public Vector4 sphereTexture = new Vector4();
        public Vector4 toonTexture = new Vector4();

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_MATERIAL, this.Index);

            exporter.Write(this.calcType);
            exporter.Write(this.diffuse);
            exporter.Write(this.specular);
            exporter.Write(this.shininess);
            exporter.Write(this.ambient);
            exporter.Write(this.edge);
            exporter.Write(this.edgeThick);
            exporter.Write(this.texture);
            exporter.Write(this.sphereTexture);
            exporter.Write(this.toonTexture);
        }

        public void Parse(PmxParser parser)
        {
            this.Index = parser.ReadPmxId(parser.SizeMaterial);

            this.calcType = parser.ReadByte();
            this.diffuse = parser.ReadVector4();
            this.specular = parser.ReadVector3();
            this.shininess = parser.ReadSingle();
            this.ambient = parser.ReadVector3();
            this.edge = parser.ReadVector4();
            this.edgeThick = parser.ReadSingle();
            this.texture = parser.ReadVector4();
            this.sphereTexture = parser.ReadVector4();
            this.toonTexture = parser.ReadVector4();
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_MATERIAL;
        }
    }
}
