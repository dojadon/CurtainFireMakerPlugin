using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DxMath;

namespace CPmx.Data
{
    [Serializable]
    public class PmxMaterialData : IPmxData
    {
        public String materialName = "";
        public String materialNameE = "";

        public String script = "";

        /**
         * 描画フラグ(8bit)
         * <ul>
         * <li>0x01:両面描画
         * <li>0x02:地面影
         * <li>0x04:セルフシャドウマップへの描画
         * <li>0x08:セルフシャドウの描画
         * <li>0x10:エッジ描画
         * </ul>
         */
        public byte flag;

        public Vector4 edge = new Vector4();
        public float edgeThick;

        public Vector4 diffuse = new Vector4();
        public Vector3 specular = new Vector3();
        public Vector3 ambient = new Vector3();
        public float shininess;

        public int textureId;
        public int sphereId;
        /**
         * スフィアモード
         * <ul>
         * <li>0:無効
         * <li>1:乗算(sph)
         * <li>2:加算(spa)
         * <li>3:サブテクスチャ(追加UV1のx,yをUV参照して通常テクスチャ描画を行う)
         * </ul>
         */
        public byte mode;
        /** 0: トゥーンはテクスチャーファイル. 1: 共有ファイル. */
        public byte sharedToon;
        /** トゥーンファイル番号。 shared:1 では, 0ならtoon01.bmp, 9ならtoon10.bmp. 0xffならtoon0.bmp. shared:0 では, texture ID. */
        public int toonId;
        public int faceCount;

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxText(this.materialName);
            exporter.WritePmxText(this.materialNameE);

            exporter.Write(this.diffuse);
            exporter.Write(this.specular);
            exporter.Write(this.shininess);
            exporter.Write(this.ambient);

            exporter.Write(this.flag);

            exporter.Write(this.edge);
            exporter.Write(this.edgeThick);

            exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, this.textureId);
            exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, this.sphereId);
            exporter.Write(this.mode);
            exporter.Write(this.sharedToon);

            if (this.sharedToon == 0)
            {
                exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, this.toonId);
            }
            else
            {
                exporter.Write((byte)this.toonId);
            }

            exporter.WritePmxText(this.script);

            exporter.Write(this.faceCount);
        }

        public void Parse(PmxParser parser)
        {
            this.materialName = parser.ReadPmxText();
            this.materialNameE = parser.ReadPmxText();

            parser.ReadVector(this.diffuse);
            parser.ReadVector(this.specular);
            this.shininess = parser.ReadSingle();
            parser.ReadVector(this.ambient);

            this.flag = parser.ReadByte();

            parser.ReadVector(this.edge);
            this.edgeThick = parser.ReadSingle();

            this.textureId = parser.ReadPmxId(parser.SizeTexture);
            this.sphereId = parser.ReadPmxId(parser.SizeTexture);
            this.mode = parser.ReadByte();
            this.sharedToon = parser.ReadByte();

            if (this.sharedToon == 0)
            {
                this.toonId = parser.ReadPmxId(parser.SizeTexture);
            }
            else
            {
                this.toonId = parser.ReadByte();
            }

            this.script = parser.ReadPmxText();

            this.faceCount = parser.ReadInt32();
        }
    }
}
