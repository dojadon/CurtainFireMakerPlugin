using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CsPmx.Data;
using VecMath;

namespace CsPmx
{
    public class PmxParser : BinaryReader
    {
        public byte SizeVertex => Size[2];
        public byte SizeTexture => Size[3];
        public byte SizeMaterial => Size[4];
        public byte SizeBone => Size[5];
        public byte SizeMorph => Size[6];
        public byte SizeRigid => Size[7];

        protected Encoding Encording => Size[0] == 0 ? Encoding.Unicode : Encoding.UTF8;

        public byte[] Size { get; set; }

        public PmxParser(Stream outStream) : base(outStream)
        {

        }

        public void Parse(PmxModelData data)
        {
            data.Parse(this);
        }

        public int ReadPmxId(byte size)
        {
            int id = 0;

            switch (size)
            {
                case 1:
                    id = ReadByte();
                    break;

                case 2:
                    id = ReadInt16();
                    break;

                case 4:
                    id = ReadInt32();
                    break;
            }
            return id;
        }

        public string ReadPmxText()
        {
            int len = ReadInt32();
            byte[] bytes = ReadBytes(len);

            string str = Encording.GetString(bytes);

            return str;
        }

        public Vector2 ReadVector2() => new Vector2(ReadSingle(), ReadSingle());

        public Vector3 ReadVector3() => new Vector3(ReadSingle(), ReadSingle(), ReadSingle());

        public Vector4 ReadVector4() => new Vector4(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());

        public Quaternion ReadQuaternion() => new Quaternion(ReadSingle(), ReadSingle(), ReadSingle(), ReadSingle());
    }
}
