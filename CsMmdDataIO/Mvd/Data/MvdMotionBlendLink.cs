using System.IO;
using System.Linq;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdMotionBlendLink
	{
		public int ClipAId
		{
			get;
			set;
		}

		public int ClipBId
		{
			get;
			set;
		}

		public int BlendSplineType
		{
			get;
			set;
		}

		public MvdTimeWarpPoint[] BlendPoints
		{
			get;
			set;
		}

		public MvdMotionBlendLink()
		{
			this.BlendPoints = new MvdTimeWarpPoint[0];
		}

		public static MvdMotionBlendLink Parse(BinaryReader br)
		{
			return new MvdMotionBlendLink
			{
				ClipAId = br.ReadInt32(),
				ClipBId = br.ReadInt32(),
				BlendSplineType = br.ReadInt32(),
				BlendPoints = Enumerable.Range(0, br.ReadInt32()).Select(_ => MvdTimeWarpPoint.Parse(br)).ToArray(),
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.ClipAId);
			bw.Write(this.ClipBId);
			bw.Write(this.BlendSplineType);
			bw.Write(this.BlendPoints.Length);
			this.BlendPoints.ForEach(_ => _.Write(bw));
		}
	}
}
