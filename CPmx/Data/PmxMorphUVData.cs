using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxMorphUVData : IPmxMorphTypeData
    {
        public byte UvType { get; set; }

        public int Index { get; set; }
        public Vector4 Uv { get; set; }

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_VERTEX, this.Index);
            exporter.Write(this.Uv);
        }

        public void Parse(PmxParser parser)
        {
            this.Index = parser.ReadPmxId(parser.SizeVertex);
            this.Uv = parser.ReadVector4();
        }

        public byte GetMorphType()
        {
            return this.UvType;
        }
    }
}
