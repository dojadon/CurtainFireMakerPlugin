using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxVertexData : IPmxData
    {
        public const byte WEIGHT_TYPE_BDEF1 = 0;
        public const byte WEIGHT_TYPE_BDEF2 = 1;
        public const byte WEIGHT_TYPE_BDEF4 = 2;
        public const byte WEIGHT_TYPE_SDEF = 3;

        public Vector3 Pos;
        public Vector3 Normal;
        public Vector2 Uv;
        public float Edge { get; set; }

        /**
         * <ul>
         * <li>0:BDEF1
         * <li>1:BDEF2
         * <li>2:BDEF4
         * <li>3:SDEF
         * <li>4:QDEF(※2.1拡張)
         * </ul>
         */
        public byte WeightType { get; set; }
        public int[] BoneId { get; set; }
        public float[] Weight { get; set; }
        public Vector3 Sdef_c;
        public Vector3 Sdef_r0;
        public Vector3 Sdef_r1;

        public void Export(PmxExporter exporter)
        {
            exporter.Write(this.Pos);
            exporter.Write(this.Normal);
            exporter.Write(this.Uv);

            // for (byte i = 0; i < num_uv; i++)
            // {
            // exporter.dumpLeFloat(uvEX.x).dumpLeFloat(uvEX.y).dumpLeFloat(uvEX.z).dumpLeFloat(uvEX.w);
            // }

            exporter.Write(this.WeightType);

            for (byte i = 0; i < this.BoneId.Length; i++)
            {
                exporter.WritePmxId(PmxExporter.SIZE_BONE, this.BoneId[i]);
            }

            switch (this.WeightType)
            {
                case WEIGHT_TYPE_BDEF1:
                    break;

                case WEIGHT_TYPE_BDEF2:
                case WEIGHT_TYPE_SDEF:
                    exporter.Write(this.Weight[0]);
                    break;

                case WEIGHT_TYPE_BDEF4:
                    for (byte i = 0; i < 4; i++)
                    {
                        exporter.Write(this.Weight[i]);
                    }
                    break;
            }

            if (this.WeightType == WEIGHT_TYPE_SDEF)
            {
                exporter.Write(this.Sdef_c);
                exporter.Write(this.Sdef_r0);
                exporter.Write(this.Sdef_r1);
            }
            exporter.Write(this.Edge);
        }

        public void Parse(PmxParser parser)
        {
            this.Pos = parser.ReadVector3();
            this.Normal = parser.ReadVector3();
            this.Uv = parser.ReadVector2();

            this.WeightType = parser.ReadByte();

            switch (this.WeightType)
            {
                case WEIGHT_TYPE_BDEF1:
                    this.BoneId = new int[1];
                    break;

                case WEIGHT_TYPE_BDEF2:
                case WEIGHT_TYPE_SDEF:
                    this.BoneId = new int[2];
                    break;

                case WEIGHT_TYPE_BDEF4:
                    this.BoneId = new int[4];
                    break;
            }

            for (int i = 0; i < this.BoneId.Length; i++)
            {
                this.BoneId[i] = parser.ReadPmxId(parser.SizeBone);
            }

            if (this.WeightType == WEIGHT_TYPE_SDEF)
            {
                this.Sdef_c = parser.ReadVector3();
                this.Sdef_r0 = parser.ReadVector3();
                this.Sdef_r1 = parser.ReadVector3();
            }
            this.Edge = parser.ReadSingle();
        }
    }
}
