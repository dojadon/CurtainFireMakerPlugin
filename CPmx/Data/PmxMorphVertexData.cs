using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    [Serializable]
    public class PmxMorphVertexData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public Vector3 positions = new Vector3();

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_VERTEX, this.Index);
            exporter.Write(this.positions);
        }

        public void Parse(PmxParser parser)
        {
            this.Index = parser.ReadPmxId(parser.SizeVertex);
            parser.ReadVector(this.positions);
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_VERTEX;
        }
    }
}
