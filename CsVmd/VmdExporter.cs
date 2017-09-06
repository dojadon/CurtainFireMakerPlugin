using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using VecMath;
using CsVmd.Data;

namespace CsVmd
{
    public class VmdExporter : BinaryWriter
    {
        private static readonly Encoding ENCORDING = Encoding.GetEncoding("Shift_JIS");
        private static readonly byte[] NULL_STR = ENCORDING.GetBytes("\0");

        public const int BONE_NAME_LENGTH = 15;
        public const int MORPH_NAME_LENGTH = 15;
        public const int MODEL_NAME_LENGTH = 20;
        public const int HEADER_LENGTH = 30;

        public VmdExporter(Stream outStream) : base(outStream)
        {

        }

        public void Export(VmdMotionData data)
        {
            data.Export(this);
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

        private void WriteFiller(byte[] filler, int fillerLength)
        {
            if (filler.Length <= 0 || fillerLength <= 0)
            {
                return;
            }

            byte lastData = filler[filler.Length - 1];

            int fillerIdx = 0;
            for (int remain = fillerLength; remain > 0; remain--)
            {
                byte bVal = fillerIdx < filler.Length ? filler[fillerIdx++] : lastData;
                this.Write(bVal);
            }
        }

        public void WriteVmdText(string str, int fixedLength)
        {
            byte[] bytes = ENCORDING.GetBytes(str);

            Write(bytes);

            int remain = fixedLength - bytes.Length;
            if (remain > 0)
            {
                WriteFiller(NULL_STR, remain);
            }
        }
    }
}
