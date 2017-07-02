using System;

namespace CPmx.Data
{
    public class PmxMorphData
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
        /**
         * 操作パネル
         * <ul>
         * <li>1:眉(左下)
         * <li>2:目(左上)
         * <li>3:口(右上)
         * <li>4:その他(右下)
         * <li>0:システム予約
         * </ul>
         */
        public byte type;

        public IPmxMorphTypeData morphType;

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxText(this.morphName);
            exporter.WritePmxText(this.morphNameE);

            exporter.Write(this.type);

            byte morphType = this.morphType.GetMorphType();
            exporter.Write(morphType);

            this.morphType.Export(exporter);
        }
    }
}
