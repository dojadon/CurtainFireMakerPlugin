using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdAccessoryPropertyFrame : IKeyFrame
    {
		public long FrameTime
		{
			get;
			set;
		}

		public bool Visible
		{
			get;
			set;
		}

		public bool Shadow
		{
			get;
			set;
		}

		public bool AddBlending
		{
			get;
			set;
		}

		public bool Reserved
		{
			get;
			set;
		}

		public float Scaling
		{
			get;
			set;
		}

		public float Alpha
		{
			get;
			set;
		}

		public int RelatedModelId
		{
			get;
			set;
		}

		public int RelatedBoneId
		{
			get;
			set;
		}

		public MvdAccessoryPropertyFrame()
		{
			this.Visible = true;
			this.Scaling =
				this.Alpha = 1;
		}

		public static MvdAccessoryPropertyFrame Parse(BinaryReader br)
		{
			return new MvdAccessoryPropertyFrame
			{
				FrameTime = br.ReadInt64(),
				Visible = br.ReadBoolean(),
				Shadow = br.ReadBoolean(),
				AddBlending = br.ReadBoolean(),
				Reserved = br.ReadBoolean(),
				Scaling = br.ReadSingle(),
				Alpha = br.ReadSingle(),
				RelatedModelId = br.ReadInt32(),
				RelatedBoneId = br.ReadInt32(),
			};
		}

		public virtual void Write(BinaryWriter bw)
		{
			bw.Write(this.FrameTime);
			bw.Write(this.Visible);
			bw.Write(this.Shadow);
			bw.Write(this.AddBlending);
			bw.Write(this.Reserved);
			bw.Write(this.Scaling);
			bw.Write(this.Alpha);
			bw.Write(this.RelatedModelId);
			bw.Write(this.RelatedBoneId);
		}
	}
}
