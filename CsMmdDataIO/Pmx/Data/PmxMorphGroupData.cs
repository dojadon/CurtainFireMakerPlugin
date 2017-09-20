using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Pmx.Data
{
    [Serializable]
    public class PmxMorphGroupData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public float Rate { get; set; }

        public void Export(PmxExporter exporter)
        {
                exporter.WritePmxId(PmxExporter.SIZE_MORPH, Index);
                exporter.Write(Rate);
        }

        public void Parse(PmxParser parser)
        {
            Index = parser.ReadPmxId(parser.SizeMorph);
            Rate = parser.ReadSingle();
        }
    }
}
