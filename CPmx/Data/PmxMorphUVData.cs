using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    [Serializable]
    public class PmxMorphUVData : IPmxMorphTypeData
    {
        public byte uvType;

        public int Index { get; set; }
        public Vector4 uv = new Vector4();

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_VERTEX, this.Index);
            exporter.Write(this.uv);
        }

        public void Parse(PmxParser parser)
        {
            this.Index = parser.ReadPmxId(parser.SizeVertex);
            this.uv = parser.ReadVector4();
        }

        public byte GetMorphType()
        {
            return this.uvType;
        }
    }
}
