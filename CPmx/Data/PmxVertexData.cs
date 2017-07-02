using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    public class PmxVertexData : IPmxData
    {
        public const byte WEIGHT_TYPE_BDEF1 = 0;
        public const byte WEIGHT_TYPE_BDEF2 = 1;
        public const byte WEIGHT_TYPE_BDEF4 = 2;
        public const byte WEIGHT_TYPE_SDEF = 3;

        public Vector3 pos = new Vector3();
        public Vector3 normal = new Vector3();
        public Vector2 uv = new Vector2();
        public float edge;

        /**
         * <ul>
         * <li>0:BDEF1
         * <li>1:BDEF2
         * <li>2:BDEF4
         * <li>3:SDEF
         * <li>4:QDEF(※2.1拡張)
         * </ul>
         */
        public byte weightType;
        public int[] boneId;
        public float[] weight;
        public Vector3 sdef_c = new Vector3();
        public Vector3 sdef_r0 = new Vector3();
        public Vector3 sdef_r1 = new Vector3();

        public void Export(PmxExporter exporter)
        {
            exporter.Write(this.pos);
            exporter.Write(this.normal);
            exporter.Write(this.uv);

            // for (byte i = 0; i < num_uv; i++)
            // {
            // exporter.dumpLeFloat(uvEX.x).dumpLeFloat(uvEX.y).dumpLeFloat(uvEX.z).dumpLeFloat(uvEX.w);
            // }

            exporter.Write(this.weightType);

            for (byte i = 0; i < this.boneId.Length; i++)
            {
                exporter.WritePmxId(PmxExporter.SIZE_BONE, this.boneId[i]);
            }

            switch (this.weightType)
            {
                case WEIGHT_TYPE_BDEF1:
                    break;

                case WEIGHT_TYPE_BDEF2:
                case WEIGHT_TYPE_SDEF:
                    exporter.Write(this.weight[0]);
                    break;

                case WEIGHT_TYPE_BDEF4:
                    for (byte i = 0; i < 4; i++)
                    {
                        exporter.Write(this.weight[i]);
                    }
                    break;
            }

            if (this.weightType == WEIGHT_TYPE_SDEF)
            {
                exporter.Write(this.sdef_c);
                exporter.Write(this.sdef_r0);
                exporter.Write(this.sdef_r1);
            }
            exporter.Write(this.edge);
        }

        public void Parse(PmxParser parser)
        {
            parser.ReadVector(this.pos);
            parser.ReadVector(this.normal);
            parser.ReadVector(this.uv);

            this.weightType = parser.ReadByte();

            switch (this.weightType)
            {
                case WEIGHT_TYPE_BDEF1:
                    this.boneId = new int[1];
                    break;

                case WEIGHT_TYPE_BDEF2:
                case WEIGHT_TYPE_SDEF:
                    this.boneId = new int[2] ;
                    break;

                case WEIGHT_TYPE_BDEF4:
                    this.boneId = new int[4] ;
                    break;
            }

            for (int i=0; i<this.boneId.Length; i++)
            {
                this.boneId[i] = parser.ReadPmxId(parser.SizeBone);
            }

            if (this.weightType == WEIGHT_TYPE_SDEF)
            {
                parser.ReadVector(this.sdef_c);
                parser.ReadVector(this.sdef_r0);
                parser.ReadVector(this.sdef_r1);
            }
            this.edge = parser.ReadSingle();
        }
    }
}
