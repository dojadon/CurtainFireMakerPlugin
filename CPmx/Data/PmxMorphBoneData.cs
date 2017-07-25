using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxMorphBoneData : IPmxMorphTypeData
    {
        public int Index { get; set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxId(PmxExporter.SIZE_BONE, this.Index);

            exporter.Write(this.Position);
            exporter.Write(this.Rotation);
        }

        public void Parse(PmxParser parser)
        {
            this.Index = parser.ReadPmxId(parser.SizeBone);

            this.Position = parser.ReadVector3();
            this.Rotation = parser.ReadQuaternion();
        }

        public byte GetMorphType()
        {
            return PmxMorphData.MORPHTYPE_BONE;
        }
    }
}
