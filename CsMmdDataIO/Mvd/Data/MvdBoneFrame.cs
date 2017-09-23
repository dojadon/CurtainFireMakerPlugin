using System.IO;
using System.Linq;
using VecMath;
using CsMmdDataIO.Interfaces.Motion;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdBoneFrame : IKeyFrame
    {
		public int StageId
		{
			get;
			set;
		}

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

		public Quaternion Quaternion
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] XInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] YInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] ZInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] RotationInterpolation
		{
			get;
			set;
		}

		public bool Spline
		{
			get;
			set;
		}

		public MvdBoneFrame()
		{
            this.Quaternion = Quaternion.Identity;
			this.XInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.YInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.ZInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.RotationInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
		}

		public static MvdBoneFrame Parse(MvdBoneData bd, BinaryReader br)
		{
            var rt = new MvdBoneFrame
            {
                StageId = br.ReadInt32(),
                FrameTime = br.ReadInt64(),
                Position = br.ReadVector3(),
				Quaternion =br.ReadQuaternion(),
				XInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
				YInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
				ZInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
				RotationInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) },
			};

			if (bd.MinorType >= 1)
			{
				rt.Spline = br.ReadBoolean();
				br.ReadBytes(3);
			}

			return rt;
		}

		public void Write(MvdBoneData bd, BinaryWriter bw)
		{
			bw.Write(this.StageId);
			bw.Write(this.FrameTime);
            bw.Write(this.Position);
            bw.Write(this.Quaternion);
			this.XInterpolation.ForEach(_ => _.Write(bw));
			this.YInterpolation.ForEach(_ => _.Write(bw));
			this.ZInterpolation.ForEach(_ => _.Write(bw));
			this.RotationInterpolation.ForEach(_ => _.Write(bw));

			if (bd.MinorType >= 1)
			{
				bw.Write(this.Spline);
				bw.Write(new byte[] { 0, 0, 0 });
			}
		}

		public string GetName(MvdNameList names, MvdBoneData boneData)
		{
			if (this.StageId == 0)
				return names.Names[boneData.Key];
			else
			{
				var key = boneData.Key * -1000 - this.StageId;

				return names.Names.ContainsKey(key)
					? names.Names[key]
					: this.StageId.ToString("000");
			}
		}
	}
}
