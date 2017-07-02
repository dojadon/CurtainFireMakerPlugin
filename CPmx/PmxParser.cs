using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CPmx
{
    class PmxParser : BinaryReader
    {
        private static Encoding ENCORDER_UTF16 = Encoding.GetEncoding("utf-16");
        private static Encoding ENCORDER_UTF8 = Encoding.GetEncoding("utf-8");

        protected Encoding Encording { get; set; }

        public PmxParser(Stream outStream) : base(outStream)
        {

        }

        public int ReadPmxId(byte size)
        {
            int id = 0;

            switch (size)
            {
                case 1:
                    id = this.ReadByte();
                    break;

                case 2:
                    id = this.ReadInt16();
                    break;

                case 4:
                    id = this.ReadInt32();
                    break;
            }
            return id;
        }

        public String ReadPmxText()
        {
            int len = this.ReadInt32();
            byte[] bytes = this.ReadBytes(len);
            char[] chars = this.Encording.GetChars(bytes);

            return new string(chars);
        }
    }
}
