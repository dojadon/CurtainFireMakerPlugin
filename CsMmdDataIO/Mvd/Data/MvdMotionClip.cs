using System.IO;
using System.Linq;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdMotionClip
	{
		public int TrackIndex
		{
			get;
			set;
		}
		public float FrameStart
		{
			get;
			set;
		}

		public float FrameLength
		{
			get;
			set;
		}

		public int RepeatCount
		{
			get;
			set;
		}

		public float Weight
		{
			get;
			set;
		}

		public float Scale
		{
			get;
			set;
		}

		public int TimeWarpSplineType
		{
			get;
			set;
		}

		public MvdTimeWarpPoint[] TimeWarpPoints
		{
			get;
			set;
		}

		public MvdMotionClip()
		{
			this.TimeWarpPoints = new MvdTimeWarpPoint[0];
		}

		public static MvdMotionClip Parse(BinaryReader br)
		{
			return new MvdMotionClip
			{
				TrackIndex = br.ReadInt32(),
				FrameStart = br.ReadSingle(),
				FrameLength = br.ReadSingle(),
				RepeatCount = br.ReadInt32(),
				Weight = br.ReadSingle(),
				Scale = br.ReadSingle(),
				TimeWarpSplineType = br.ReadInt32(),
				TimeWarpPoints = Enumerable.Range(0, br.ReadInt32()).Select(_ => MvdTimeWarpPoint.Parse(br)).ToArray(),
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.TrackIndex);
			bw.Write(this.FrameStart);
			bw.Write(this.FrameLength);
			bw.Write(this.RepeatCount);
			bw.Write(this.Weight);
			bw.Write(this.Scale);
			bw.Write(this.TimeWarpSplineType);
			bw.Write(this.TimeWarpPoints.Length);
			this.TimeWarpPoints.ForEach(_ => _.Write(bw));
		}
	}
}
