using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsMmdDataIO.Pmx.Data
{
    [Serializable]
    public class PmxMorphVertexData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; } 

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_VERTEX, Index);
            exporter.Write(Position);
        }

        public void Parse(PmxParser parser)
        {
            Index = parser.ReadPmxId(parser.SizeVertex);
            Position = parser.ReadVector3();
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_VERTEX;
        }
    }
}
