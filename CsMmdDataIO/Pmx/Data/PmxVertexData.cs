using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsMmdDataIO.Pmx.Data
{
    [Serializable]
    public class PmxVertexData : IPmxData
    {
        public const byte WEIGHT_TYPE_BDEF1 = 0;
        public const byte WEIGHT_TYPE_BDEF2 = 1;
        public const byte WEIGHT_TYPE_BDEF4 = 2;
        public const byte WEIGHT_TYPE_SDEF = 3;

        public int VertexId { get; set; }

        public Vector3 Pos { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 Uv { get; set; }
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
            exporter.Write(Pos);
            exporter.Write(Normal);
            exporter.Write(Uv);

            // for (byte i = 0; i < num_uv; i++)
            // {
            // exporter.dumpLeFloat(uvEX.x).dumpLeFloat(uvEX.y).dumpLeFloat(uvEX.z).dumpLeFloat(uvEX.w);
            // }

            exporter.Write(WeightType);

            for (byte i = 0; i < this.BoneId.Length; i++)
            {
                exporter.WritePmxId(PmxExporter.SIZE_BONE, BoneId[i]);
            }

            switch (WeightType)
            {
                case WEIGHT_TYPE_BDEF1:
                    break;

                case WEIGHT_TYPE_BDEF2:
                case WEIGHT_TYPE_SDEF:
                    exporter.Write(Weight[0]);
                    break;

                case WEIGHT_TYPE_BDEF4:
                    for (byte i = 0; i < 4; i++)
                    {
                        exporter.Write(Weight[i]);
                    }
                    break;
            }

            if (this.WeightType == WEIGHT_TYPE_SDEF)
            {
                exporter.Write(Sdef_c);
                exporter.Write(Sdef_r0);
                exporter.Write(Sdef_r1);
            }
            exporter.Write(Edge);
        }

        public void Parse(PmxParser parser)
        {
            Pos = parser.ReadVector3();
            Normal = parser.ReadVector3();
            Uv = parser.ReadVector2();

            WeightType = parser.ReadByte();

            switch (WeightType)
            {
                case WEIGHT_TYPE_BDEF1:
                    BoneId = new int[1];
                    break;

                case WEIGHT_TYPE_BDEF2:
                case WEIGHT_TYPE_SDEF:
                    BoneId = new int[2];
                    break;

                case WEIGHT_TYPE_BDEF4:
                    BoneId = new int[4];
                    break;
            }

            for (int i = 0; i < BoneId.Length; i++)
            {
                BoneId[i] = parser.ReadPmxId(parser.SizeBone);
            }

            if (WeightType == WEIGHT_TYPE_SDEF)
            {
                Sdef_c = parser.ReadVector3();
                Sdef_r0 = parser.ReadVector3();
                Sdef_r1 = parser.ReadVector3();
            }
            Edge = parser.ReadSingle();
        }
    }
}
