using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DxMath;
using CsVmd.Data;

namespace CsVmd
{
    public class VmdExporter : BinaryWriter
    {
        private static readonly Encoding ENCORDING = Encoding.GetEncoding("utf-16");
        private static readonly byte[] NULL_STR = ENCORDING.GetBytes("\0");

        public const int BONE_NAME_LENGTH = 15;
        public const int MORPH_NAME_LENGTH = 15;
        public const int MODEL_NAME_LENGTH = 15;
        public const int HEADER_LENGTH = 15;

        public VmdExporter(Stream outStream) : base(outStream)
        {

        }

        public void Export(VmdMotionData data)
        {
            data.Export(this);
        }

        public void Write(Vector2 vec)
        {
            this.Write(vec.X);
            this.Write(vec.Y);
        }

        public void Write(Vector3 vec)
        {
            this.Write(vec.X);
            this.Write(vec.Y);
            this.Write(vec.Z);
        }

        public void Write(Vector4 vec)
        {
            this.Write(vec.X);
            this.Write(vec.Y);
            this.Write(vec.Z);
            this.Write(vec.W);
        }

        public void Write(Quaternion vec)
        {
            this.Write(vec.X);
            this.Write(vec.Y);
            this.Write(vec.Z);
            this.Write(vec.W);
        }

        public void WriteVmdText(string str, int maxLength)
        {
            byte[] bytes = ENCORDING.GetBytes(str);

            this.Write(bytes);

            maxLength -= bytes.Length;

            while(maxLength > 0)
            {
                this.Write(NULL_STR);
                maxLength -= NULL_STR.Length;
            }
        }
    }
}
