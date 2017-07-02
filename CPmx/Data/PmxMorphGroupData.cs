using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPmx.Data
{
    public class PmxMorphGroupData : IPmxMorphTypeData
    {
        public int[] indices;
        public float[] rateValues;

        public void Export(PmxExporter exporter)
        {
            exporter.Write(this.indices.Length);
            for (int i = 0; i < this.indices.Length; i++)
            {
                exporter.WritePmxId(PmxExporter.SIZE_MORPH, this.indices[i]);

                exporter.Write(this.rateValues[i]);
            }
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_GROUP;
        }

        public void SetIndices(int[] indices)
        {
            this.indices = indices;
            this.rateValues = new float[indices.Length];
        }
    }
}
