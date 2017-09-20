using System.IO;
using System.Linq;
using VecMath;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdFilterFrame
	{
		public long FrameTime
		{
			get;
			set;
		}

		public int FilterType
		{
			get;
			set;
		}

		public bool Interpolation
		{
			get;
			set;
		}

		/// <summary>
		/// r, g, b
		/// </summary>
		public Vector3 FadeColor
		{
			get;
			set;
		}

		public float FadeValue
		{
			get;
			set;
		}

		public float[] HSVValue
		{
			get;
			set;
		}

		public MvdTimeWarpPoint[] ToneCurveControlPoints
		{
			get;
			set;
		}

		public static MvdFilterFrame Parse(BinaryReader br)
		{
			var rt = new MvdFilterFrame
			{
				FrameTime = br.ReadInt64(),
				FilterType = br.ReadInt32(),
				Interpolation = br.ReadBoolean(),
			};

			br.ReadBytes(3);
			rt.FadeColor = br.ReadVector3();
			rt.FadeValue = br.ReadSingle();
			rt.HSVValue = new[] { br.ReadSingle(), br.ReadSingle(), br.ReadSingle() };
			rt.ToneCurveControlPoints = Enumerable.Range(0, br.ReadInt32()).Select(_ => MvdTimeWarpPoint.Parse(br)).ToArray();

			return rt;
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.FrameTime);
			bw.Write(this.FilterType);
			bw.Write(this.Interpolation);
			bw.Write(new byte[3]);
			bw.Write(this.FadeColor);
            bw.Write(this.FadeValue);
			this.HSVValue.ForEach(bw.Write);
			bw.Write(this.ToneCurveControlPoints.Length);
			this.ToneCurveControlPoints.ForEach(_ => _.Write(bw));
		}
	}
}
