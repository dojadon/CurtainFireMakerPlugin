using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxMorphGroupData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public float Rate { get; set; }

        public void Export(PmxExporter exporter)
        {
                exporter.WritePmxId(PmxExporter.SIZE_MORPH, this.Index);
                exporter.Write(this.Rate);
        }

        public void Parse(PmxParser parser)
        {
            this.Index = parser.ReadPmxId(parser.SizeMorph);
            this.Rate = parser.ReadSingle();
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_GROUP;
        }
    }
}
