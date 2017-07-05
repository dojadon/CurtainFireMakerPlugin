using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using CPmx.Data;
using DxMath;

namespace CPmx
{
    public class PmxParser : BinaryReader
    {
        public byte SizeVertex => this.Size[2];
        public byte SizeTexture => this.Size[3];
        public byte SizeMaterial => this.Size[4];
        public byte SizeBone => this.Size[5];
        public byte SizeMorph => this.Size[6];
        public byte SizeRigid => this.Size[7];

        protected Encoding Encording => this.Size[0] == 0 ? Encoding.Unicode : Encoding.UTF8;

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

        public string ReadPmxText()
        {
            int len = this.ReadInt32();
            byte[] bytes = this.ReadBytes(len);

            string str = this.Encording.GetString(bytes);

            return str;
        }

        public Vector2 ReadVector2()
        {
            return new Vector2(this.ReadSingle(), this.ReadSingle());
        }

        public Vector3 ReadVector3()
        {
            return new Vector3(this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
        }

        public Vector4 ReadVector4()
        {
            return new Vector4(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
        }

        public Quaternion ReadQuaternion()
        {
            return new Quaternion(this.ReadSingle(), this.ReadSingle(), this.ReadSingle(), this.ReadSingle());
        }

        public void ReadVector(Vector2 vec)
        {
            vec.X = this.ReadSingle();
            vec.Y = this.ReadSingle();
        }
    }
}
