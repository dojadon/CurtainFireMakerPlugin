using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VecMath;

namespace CsMmdDataIO.Pmx.Data
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
            exporter.WriteText(MaterialName);
            exporter.WriteText(MaterialNameE);

            exporter.Write(Diffuse);
            exporter.Write(Specular);
            exporter.Write(Shininess);
            exporter.Write(Ambient);

            exporter.Write(Flag);

            exporter.Write(Edge);
            exporter.Write(EdgeThick);

            exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, TextureId);
            exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, SphereId);
            exporter.Write(Mode);
            exporter.Write(SharedToon);

            if (SharedToon == 0)
            {
                exporter.WritePmxId(PmxExporter.SIZE_TEXTURE, ToonId);
            }
            else
            {
                exporter.Write((byte)ToonId);
            }

            exporter.WriteText(Script);

            exporter.Write(FaceCount);
        }

        public void Parse(PmxParser parser)
        {
            MaterialName = parser.ReadPmxText();
            MaterialNameE = parser.ReadPmxText();

            Diffuse = parser.ReadVector4();
            Specular = parser.ReadVector3();
            Shininess = parser.ReadSingle();
            Ambient = parser.ReadVector3();

            Flag = parser.ReadByte();

            Edge = parser.ReadVector4();
            EdgeThick = parser.ReadSingle();

            TextureId = parser.ReadPmxId(parser.SizeTexture);
            SphereId = parser.ReadPmxId(parser.SizeTexture);
            Mode = parser.ReadByte();
            SharedToon = parser.ReadByte();

            if (SharedToon == 0)
            {
                ToonId = parser.ReadPmxId(parser.SizeTexture);
            }
            else
            {
                ToonId = parser.ReadByte();
            }

            Script = parser.ReadPmxText();

            FaceCount = parser.ReadInt32();
        }
    }
}
