using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxMaterialData : IPmxData
    {
        public String MaterialName { get; set; } = "";
        public String MaterialNameE { get; set; } = "";

        public int MaterialId { get; set; }
        public String Script { get; set; } = "";

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
        public byte Flag { get; set; }

        public Vector4 Edge { get; set; } = new Vector4();
        public float EdgeThick { get; set; }

        public Vector4 Diffuse { get; set; } = new Vector4();
        public Vector3 Specular { get; set; } = new Vector3();
        public Vector3 Ambient { get; set; } = new Vector3();
        public float Shininess { get; set; }

        public int TextureId { get; set; }
        public int SphereId { get; set; }
        /**
         * スフィアモード
         * <ul>
         * <li>0:無効
         * <li>1:乗算(sph)
         * <li>2:加算(spa)
         * <li>3:サブテクスチャ(追加UV1のx,yをUV参照して通常テクスチャ描画を行う)
         * </ul>
         */
        public byte Mode { get; set; }
        /** 0: トゥーンはテクスチャーファイル. 1: 共有ファイル. */
        public byte SharedToon { get; set; }
        /** トゥーンファイル番号。 shared:1 では, 0ならtoon01.bmp, 9ならtoon10.bmp. 0xffならtoon0.bmp. shared:0 では, texture ID. */
        public int ToonId { get; set; }
        public int FaceCount { get; set; }

        public void Export(PmxExporter exporter)
        {
            exporter.WritePmxText(this.MaterialName);
            exporter.WritePmxText(this.MaterialNameE);

            exporter.Write(this.Diffuse);
            exporter.Write(this.Specular);
            exporter.Write(this.Shininess);
            exporter.Write(this.Ambient);

            exporter.Write(this.Flag);

            exporter.Write(this.Edge);
            exporter.Write(this.EdgeThick);

            exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, this.TextureId);
            exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, this.SphereId);
            exporter.Write(this.Mode);
            exporter.Write(this.SharedToon);

            if (this.SharedToon == 0)
            {
                exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, this.ToonId);
            }
            else
            {
                exporter.Write((byte)this.ToonId);
            }

            exporter.WritePmxText(this.Script);

            exporter.Write(this.FaceCount);
        }

        public void Parse(PmxParser parser)
        {
            this.MaterialName = parser.ReadPmxText();
            this.MaterialNameE = parser.ReadPmxText();

            this.Diffuse = parser.ReadVector4();
            this.Specular = parser.ReadVector3();
            this.Shininess = parser.ReadSingle();
            this.Ambient = parser.ReadVector3();

            this.Flag = parser.ReadByte();

            this.Edge = parser.ReadVector4();
            this.EdgeThick = parser.ReadSingle();

            this.TextureId = parser.ReadPmxId(parser.SizeTexture);
            this.SphereId = parser.ReadPmxId(parser.SizeTexture);
            this.Mode = parser.ReadByte();
            this.SharedToon = parser.ReadByte();

            if (this.SharedToon == 0)
            {
                this.ToonId = parser.ReadPmxId(parser.SizeTexture);
            }
            else
            {
                this.ToonId = parser.ReadByte();
            }

            this.Script = parser.ReadPmxText();

            this.FaceCount = parser.ReadInt32();
        }
    }
}
