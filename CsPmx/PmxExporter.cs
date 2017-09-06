using System;
using System.Text;
using System.IO;
using VecMath;
using CsPmx.Data;

namespace CsPmx
{
    public class PmxExporter : BinaryWriter
    {
        private static readonly Encoding ENCORDER = Encoding.GetEncoding("utf-16");

        public const byte SIZE_VERTEX = 4;
        public const byte SIZE_TEXTURE = 2;
        public const byte SIZE_MATERIAL = 4;
        public const byte SIZE_BONE = 4;
        public const byte SIZE_MORPH = 4;
        public const byte SIZE_RIGID = 2;

        public static readonly byte[] SIZE = { 0, 0, SIZE_VERTEX, SIZE_TEXTURE, SIZE_MATERIAL, SIZE_BONE, SIZE_MORPH, SIZE_RIGID };

        public PmxExporter(Stream OutStream) : base(OutStream)
        {
        }

        public void Export(PmxModelData data)
        {
            data.Export(this);
        }

        public PmxExporter WritePmxId(byte size, int id)
        {
            switch (size)
            {
                case 1:
                    Write((byte)id);
                    break;

                case 2:
                    Write((short)id);
                    break;

                case 4:
                    this.Write(id);
                    break;
            }
            return this;
        }

        public void WritePmxText(String text)
        {
            if (text == null)
            {
                text = "";
            }

            byte[] bytes = ENCORDER.GetBytes(text.ToCharArray());

            Write(bytes.Length);
            Write(bytes);
        }

        public void Write(Vector2 vec)
        {
            Write(vec.x);
            Write(vec.y);
        }

        public void Write(Vector3 vec)
        {
            Write(vec.x);
            Write(vec.y);
            Write(vec.z);
        }

        public void Write(Vector4 vec)
        {
            Write(vec.x);
            Write(vec.y);
            Write(vec.z);
            Write(vec.w);
        }

        public void Write(Quaternion vec)
        {
            Write(vec.x);
            Write(vec.y);
            Write(vec.z);
            Write(vec.w);
        }
    }
}
