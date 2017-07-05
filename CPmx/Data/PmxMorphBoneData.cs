using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxMorphBoneData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public Vector3 position = new Vector3();
        public Quaternion rotation = new Quaternion();

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_BONE, this.Index);

            exporter.Write(this.position);
            exporter.Write(this.rotation);
        }

        public void Parse(PmxParser parser)
        {
            this.Index = parser.ReadPmxId(parser.SizeBone);

            this.position = parser.ReadVector3();
            this.rotation = parser.ReadQuaternion();
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_BONE;
        }
    }
}
