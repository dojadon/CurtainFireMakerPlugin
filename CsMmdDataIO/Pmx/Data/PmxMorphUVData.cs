using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsMmdDataIO.Pmx.Data
{
    [Serializable]
    public class PmxMorphUVData : IPmxMorphTypeData
    {
        public byte UvType { get; set; }

        public int Index { get; set; }
        public Vector4 Uv { get; set; }

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_VERTEX, Index);
            exporter.Write(Uv);
        }

        public void Parse(PmxParser parser)
        {
            Index = parser.ReadPmxId(parser.SizeVertex);
            Uv = parser.ReadVector4();
        }

        public byte GetMorphType()
        {
            return UvType;
        }
    }
}
