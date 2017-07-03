using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    public class PmxMorphMaterialData : IPmxMorphTypeData
    {
        public int index;

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
            exporter.WritePmxId(PmxExporter.SIZE_MATERIAL, this.index);

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
            this.index = parser.ReadPmxId(parser.SizeMaterial);

            this.calcType = parser.ReadByte();
            parser.ReadVector(this.diffuse);
            parser.ReadVector(this.specular);
            this.shininess = parser.ReadSingle();
            parser.ReadVector(this.ambient);
            parser.ReadVector(this.edge);
            this.edgeThick = parser.ReadSingle();
            parser.ReadVector(this.texture);
            parser.ReadVector(this.sphereTexture);
            parser.ReadVector(this.toonTexture);
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_MATERIAL;
        }
    }
}
