using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CPmx.Data
{
    public class PmxHeaderData : IPmxData
    {
        private static readonly byte[] MAGIC_BYTES = { 0x50, 0x4d, 0x58, 0x20 };// PMX
        protected const String CR = "\r"; // 0x0d
        protected const String LF = "\n"; // 0x0a
        protected const String CRLF = CR + LF; // 0x0d, 0x0a

        public float version;
        public String modelName = "";
        public String modelNameE = "";
        public String description = "";
        public String descriptionE = "";
        public int uv;

        public int encode;

        public void Export(PmxExporter exporter)
        {
            exporter.Write(MAGIC_BYTES);

            exporter.Write(this.version);

            exporter.Write((byte)PmxExporter.SIZE.Length);
            exporter.Write(PmxExporter.SIZE);

            exporter.WritePmxText(this.modelName);
            exporter.WritePmxText(this.modelNameE);

            String description = this.description.Replace(LF, CRLF);
            exporter.WritePmxText(description);

            String descriptionE = this.descriptionE.Replace(LF, CRLF);
            exporter.WritePmxText(descriptionE);
        }
    }
}
