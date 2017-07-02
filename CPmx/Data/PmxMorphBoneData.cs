using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    public class PmxMorphBoneData : IPmxMorphTypeData
    {
        public int[] indices;
        public Vector3[] positions;
        public Quaternion[] rotations;

        public void Export(PmxExporter exporter)
        {
            exporter.Write(this.indices.Length);
            for (int i = 0; i < this.indices.Length; i++)
            {
                exporter.WritePmxId(PmxExporter.SIZE_BONE, this.indices[i]);

                exporter.Write(this.positions[i]);
                exporter.Write(this.rotations[i]);
            }
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_BONE;
        }

        public void SetIndices(int[] indices)
        {
            this.indices = indices;
            this.positions = ArrayUtil.Set(new Vector3[indices.Length], i => new Vector3());
            this.rotations = ArrayUtil.Set(new Quaternion[indices.Length], i => new Quaternion());
        }
    }
}
