using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    public class PmxMoprhUVData : IPmxMorphTypeData
    {
        public byte uvType;

        public int[] indices;
        public Vector4[] uv;

        public void Export(PmxExporter exporter)
        {
            exporter.Write(this.indices.Length);
            for (int i = 0; i < this.indices.Length; i++)
            {
                exporter.WritePmxId(PmxExporter.SIZE_VERTEX, this.indices[i]);
                exporter.Write(this.uv[i]);
            }
        }

        public byte GetMorphType()
        {
            return this.uvType;
        }

        public void SetIndices(int[] indices)
        {
            this.indices = indices;
            this.uv = ArrayUtil.Set(new Vector4[indices.Length], i => new Vector4());
        }
    }
}
