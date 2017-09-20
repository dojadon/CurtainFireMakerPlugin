using System;
using System.IO;
using VecMath;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdCameraFrame
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

		public float Radius
		{
			get;
			set;
		}

		public Vector3 Position
		{
			get;
			set;
		}

		public Vector3 Angle
		{
			get;
			set;
		}

		public float Fov
		{
			get;
			set;
		}

		public bool Spline
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] PositionInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] AngleInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] RadiusInterpolation
		{
			get;
			set;
		}

		public MvdInterpolationPoint[] FovInterpolation
		{
			get;
			set;
		}

		public MvdCameraFrame()
		{
			this.Radius = 50;
			this.Position = new[] { 0f, 10, 0 };
			this.Angle = new[] { 0, (float)Math.PI, 0 };
			this.Fov = (float)(Math.PI * 1.666667);
			this.PositionInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.AngleInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.RadiusInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
			this.FovInterpolation = new[] { MvdInterpolationPoint.DefaultA, MvdInterpolationPoint.DefaultB };
		}

		public static MvdCameraFrame Parse(MvdCameraData cd, BinaryReader br, out MvdCameraPropertyFrame propertyFrame)
		{
			var rt = new MvdCameraFrame
			{
				StageId = br.ReadInt32(),
				FrameTime = br.ReadInt64(),
				Radius = br.ReadSingle(),
				Position = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Angle = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() },
				Fov = br.ReadSingle(),
			};

			propertyFrame = null;

			switch (cd.MinorType)
			{
				case 0:
					propertyFrame = new MvdCameraPropertyFrame
					{
						FrameTime = rt.FrameTime,
						Perspective = br.ReadBoolean(),
					};

					break;
				case 1:
					propertyFrame = new MvdCameraPropertyFrame
					{
						FrameTime = rt.FrameTime,
						Enabled = br.ReadBoolean(),
						Perspective = br.ReadBoolean(),
						Alpha = br.ReadSingle(),
						EffectEnabled = br.ReadBoolean(),
					};

					break;
				case 3:
					rt.Spline = br.ReadBoolean();
					br.ReadBytes(3);

					break;
			}

			rt.PositionInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) };
			rt.AngleInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) };
			rt.RadiusInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) };
			rt.FovInterpolation = new[] { MvdInterpolationPoint.Parse(br), MvdInterpolationPoint.Parse(br) };

			return rt;
		}

		public void Write(MvdCameraData cd, BinaryWriter bw)
		{
			if (cd.MinorType != 3)
				throw new NotImplementedException("MinorType != 3 not implemented");

			bw.Write(this.StageId);
			bw.Write(this.FrameTime);
			bw.Write(this.Radius);
            bw.Write(this.Position);
            bw.Write(this.Angle);
			bw.Write(this.Fov);

			bw.Write(this.Spline);
			bw.Write(new byte[] { 0, 0, 0 });

			this.PositionInterpolation.ForEach(_ => _.Write(bw));
			this.AngleInterpolation.ForEach(_ => _.Write(bw));
			this.RadiusInterpolation.ForEach(_ => _.Write(bw));
			this.FovInterpolation.ForEach(_ => _.Write(bw));
		}

		public string GetName(MvdNameList names, MvdCameraData cameraData)
		{
			if (this.StageId == 0)
				return names.Names[cameraData.Key];
			else
			{
				var key = cameraData.Key * -1000 - this.StageId;

				return names.Names.ContainsKey(key)
					? names.Names[key]
					: this.StageId.ToString("000");
			}
		}
	}
}
