using System;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxMorphData : IPmxData
    {
        public const byte MORPHTYPE_GROUP = 0;
        public const byte MORPHTYPE_VERTEX = 1;
        public const byte MORPHTYPE_BONE = 2;
        public const byte MORPHTYPE_UV = 3;
        public const byte MORPHTYPE_EXUV_1 = 4;
        public const byte MORPHTYPE_EXUV_2 = 5;
        public const byte MORPHTYPE_EXUV_3 = 6;
        public const byte MORPHTYPE_EXUV_4 = 7;
        public const byte MORPHTYPE_MATERIAL = 8;

        public String morphName = "";
        public String morphNameE = "";
        public byte type;

        public IPmxMorphTypeData[] morphArray;

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxText(this.morphName);
            exporter.WritePmxText(this.morphNameE);

            exporter.Write(this.type);

            byte morphType = this.morphArray[0].GetMorphType();
            exporter.Write(morphType);

            int elementCount = this.morphArray.Length;
            exporter.Write(elementCount);

            for (int i = 0; i < this.morphArray.Length; i++)
            {
                this.morphArray[i].Export(exporter);
            }
        }

        public void Parse(PmxParser parser)
        {
            this.morphName = parser.ReadPmxText();
            this.morphName = parser.ReadPmxText();

            this.type = parser.ReadByte();

            byte morphType = parser.ReadByte();

            int elementCount = parser.ReadInt32();

            switch (morphType)
            {
                case MORPHTYPE_GROUP:
                    this.morphArray = ArrayUtil.Set(new PmxMorphGroupData[elementCount], i => new PmxMorphGroupData());
                    break;

                case MORPHTYPE_VERTEX:
                    this.morphArray = ArrayUtil.Set(new PmxMorphVertexData[elementCount], i => new PmxMorphVertexData());
                    break;

                case MORPHTYPE_BONE:
                    this.morphArray = ArrayUtil.Set(new PmxMorphBoneData[elementCount], i => new PmxMorphBoneData());
                    break;

                case MORPHTYPE_UV:
                case MORPHTYPE_EXUV_1:
                case MORPHTYPE_EXUV_2:
                case MORPHTYPE_EXUV_3:
                case MORPHTYPE_EXUV_4:
                    this.morphArray = ArrayUtil.Set(new PmxMorphUVData[elementCount], i => new PmxMorphUVData());
                    break;

                case MORPHTYPE_MATERIAL:
                    this.morphArray = ArrayUtil.Set(new PmxMorphMaterialData[elementCount], i => new PmxMorphMaterialData());
                    break;
            }

            for (int i = 0; i < this.morphArray.Length; i++)
            {
                this.morphArray[i].Parse(parser);
            }
        }
    }
}
