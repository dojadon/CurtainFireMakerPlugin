using System.IO;
using System.Linq;
using VecMath;
using CsMmdDataIO.Interfaces.Motion;

namespace CsMmdDataIO.Mvd.Data
{
    public class MvdLightFrame : IKeyFrame
    {
        public long FrameTime
        {
            get;
            set;
        }

        public Vector3 Position
        {
            get;
            set;
        }

        public byte[] Color
        {
            get;
            set;
        }

        public bool Enabled
        {
            get;
            set;
        }

        public MvdLightFrame()
        {
            this.Position = new Vector3(-0.5f, -1, 0.5f);
            this.Color = new byte[] { 153, 153, 153 };
            this.Enabled = true;
        }

        public static MvdLightFrame Parse(BinaryReader br)
        {
            return new MvdLightFrame
            {
                FrameTime = br.ReadInt64(),
                Position = br.ReadVector3(),
                Color = new[] { br.ReadByte(), br.ReadByte(), br.ReadByte() },
                Enabled = br.ReadBoolean(),
            };
        }

        public void Write(BinaryWriter bw)
        {
            bw.Write(this.FrameTime);
            bw.Write(this.Position);
            this.Color.ForEach(bw.Write);
            bw.Write(this.Enabled);
        }
    }
}
