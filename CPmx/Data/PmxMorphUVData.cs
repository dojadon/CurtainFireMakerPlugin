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

        public int index;
        public Vector4 uv = new Vector4();

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_VERTEX, this.index);
            exporter.Write(this.uv);
        }

        public void Parse(PmxParser parser)
        {
            this.index = parser.ReadPmxId(parser.SizeVertex);
            parser.ReadVector(this.uv);
        }

        public byte GetMorphType()
        {
            return this.uvType;
        }
    }
}
