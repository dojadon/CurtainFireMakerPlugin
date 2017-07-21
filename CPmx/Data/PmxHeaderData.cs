using System;

namespace CsPmx.Data
{
    [Serializable]
    public class PmxHeaderData : IPmxData
    {
        private static readonly byte[] MAGIC_BYTES = { 0x50, 0x4d, 0x58, 0x20 };// PMX
        private static readonly byte[] SIZE_BYTES = { 0, 0, 4, 2, 4, 4, 2, 2 };
        protected const String CR = "\r"; // 0x0d
        protected const String LF = "\n"; // 0x0a
        protected const String CRLF = CR + LF; // 0x0d, 0x0a

        public byte[] Size { get; set; }

        public float Version { get; set; }
        public String ModelName { get; set; } = "";
        public String ModelNameE { get; set; } = "";
        public String Description { get; set; } = "";
        public String DescriptionE { get; set; } = "";
        public int Uv { get; set; }

        public int Encode { get; set; }

        public void Export(PmxExporter exporter)
        {
            exporter.Write(MAGIC_BYTES);

            exporter.Write(this.Version);

            exporter.Write((byte)PmxExporter.SIZE.Length);
            exporter.Write(PmxExporter.SIZE);

            exporter.WritePmxText(this.ModelName);
            exporter.WritePmxText(this.ModelNameE);

            String description = this.Description.Replace(LF, CRLF);
            exporter.WritePmxText(description);

            String descriptionE = this.DescriptionE.Replace(LF, CRLF);
            exporter.WritePmxText(descriptionE);
        }

        public void Parse(PmxParser parser)
        {
            byte[] magic = parser.ReadBytes(MAGIC_BYTES.Length);

            this.Version = parser.ReadSingle();
            byte sizeLen = parser.ReadByte();
            this.Size = parser.ReadBytes(sizeLen);
            parser.Size = this.Size;

            this.ModelName = parser.ReadPmxText();
            this.ModelNameE = parser.ReadPmxText();

            this.Description = parser.ReadPmxText();
            this.DescriptionE = parser.ReadPmxText();
        }
    }
}
