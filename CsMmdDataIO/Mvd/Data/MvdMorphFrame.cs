using System.IO;
using System.Linq;
using CsMmdDataIO.Interfaces.Motion;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdMorphFrame : IKeyFrame
    {
		public long FrameTime
		{
			get;
			set;
		}

		public float Weight
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] Interpolation
		{
			get;
			set;
		}

		public MvdMorphFrame()
		{
			this.Interpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
		}

		public static MvdMorphFrame Parse(BinaryReader br)
		{
			return new MvdMorphFrame
			{
				FrameTime = br.ReadInt64(),
				Weight = br.ReadSingle(),
				Interpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.FrameTime);
			bw.Write(this.Weight);
			this.Interpolation.ForEach(_ => _.Write(bw));
		}
	}
}
