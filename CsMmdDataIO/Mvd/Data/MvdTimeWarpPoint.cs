using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public struct MvdTimeWarpPoint
	{
		public float X;
		public float Y;

		public MvdTimeWarpPoint(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}

		public static MvdTimeWarpPoint Parse(BinaryReader br)
		{
			return new MvdTimeWarpPoint(br.ReadSingle(), br.ReadSingle());
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.X);
			bw.Write(this.Y);
		}

		public override string ToString()
		{
			return "{X:" + this.X + " Y:" + this.Y + "}";
		}
	}
}
