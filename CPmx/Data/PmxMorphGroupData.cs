using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPmx.Data
{
    public class PmxMorphGroupData : IPmxMorphTypeData
    {
        public int index;
        public float rate;

        public void Export(PmxExporter exporter)
        {
                exporter.WritePmxId(PmxExporter.SIZE_MORPH, this.index);
                exporter.Write(this.rate);
        }

        public void Parse(PmxParser parser)
        {
            this.index = parser.ReadPmxId(parser.SizeMorph);
            this.rate = parser.ReadSingle();
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_GROUP;
        }
    }
}
