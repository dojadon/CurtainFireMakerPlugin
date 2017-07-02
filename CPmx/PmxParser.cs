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
        private byte sizeVertex;
        public byte SizeVertex => sizeVertex;

        private byte sizeTexture;
        public byte SizeTexture => sizeTexture;

        private byte sizeMaterial;
        public byte SizeMaterial => sizeMaterial;

        private byte sizeBone;
        public byte SizeBone => sizeBone;

        private byte sizeMorph;
        public byte SizeMorph => sizeMorph;

        private byte sizeRigid;
        public byte SizeRigid => sizeRigid;

        protected Encoding Encording { get; set; }

        private PmxHeaderData headerData;
        public PmxHeaderData HeaderData
        {
            get { return this.headerData; }
            set
            {
                this.headerData = value;
                this.Encording = this.HeaderData.size[0] == 0 ? Encoding.Unicode : Encoding.UTF8;
                this.sizeVertex = this.HeaderData.size[2];
                this.sizeTexture = this.HeaderData.size[3];
                this.sizeMaterial = this.HeaderData.size[4];
                this.sizeBone = this.HeaderData.size[5];
                this.sizeMorph = this.HeaderData.size[6];
                this.sizeRigid = this.HeaderData.size[7];
            }
        }

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

        public void ReadVector(Vector2 vec)
        {
            vec.X = this.ReadSingle();
            vec.Y = this.ReadSingle();
        }

        public void ReadVector(Vector3 vec)
        {
            vec.X = this.ReadSingle();
            vec.Y = this.ReadSingle();
            vec.Z = this.ReadSingle();
        }

        public void ReadVector(Vector4 vec)
        {
            vec.X = this.ReadSingle();
            vec.Y = this.ReadSingle();
            vec.Z = this.ReadSingle();
            vec.W = this.ReadSingle();
        }
    }
}
