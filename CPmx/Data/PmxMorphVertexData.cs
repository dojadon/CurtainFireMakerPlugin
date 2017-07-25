using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxMorphVertexData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; } 

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_VERTEX, this.Index);
            exporter.Write(this.Position);
        }

        public void Parse(PmxParser parser)
        {
            this.Index = parser.ReadPmxId(parser.SizeVertex);
            this.Position = parser.ReadVector3();
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_VERTEX;
        }
    }
}
