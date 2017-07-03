using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    [Serializable]
    public class PmxMorphBoneData : IPmxMorphTypeData
    {
        public int index;
        public Vector3 position = new Vector3();
        public Quaternion rotation = new Quaternion();

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_BONE, this.index);

            exporter.Write(this.position);
            exporter.Write(this.rotation);
        }

        public void Parse(PmxParser parser)
        {
            this.index = parser.ReadPmxId(parser.SizeBone);

            parser.ReadVector(this.position);
            parser.ReadVector(this.rotation);
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_BONE;
        }
    }
}
