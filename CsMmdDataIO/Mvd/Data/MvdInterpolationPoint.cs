using System;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public struct MvdInterpolationPoint
	{
		public static readonly MvdInterpolationPoint DefaultA = new MvdInterpolationPoint(20, 20);
		public static readonly MvdInterpolationPoint DefaultB = new MvdInterpolationPoint(107, 107);

		public byte X;
		public byte Y;

		public MvdInterpolationPoint(byte x, byte y)
		{
			if (x < 0 || x > 128)
				throw new ArgumentOutOfRangeException("x must be between 0 and 127.");

			if (y < 0 || y > 128)
				throw new ArgumentOutOfRangeException("y must be between 0 and 127.");

			this.X = x;
			this.Y = y;
		}

		public static MvdInterpolationPoint Parse(BinaryReader br)
		{
			return new MvdInterpolationPoint(br.ReadByte(), br.ReadByte());
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
