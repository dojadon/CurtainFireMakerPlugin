using System.IO;
using System.Linq;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdModelPropertyFrame : IKeyFrame
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

		public bool Physics
		{
			get;
			set;
		}

		public bool PhysicsStillMode
		{
			get;
			set;
		}

		public float EdgeWidth
		{
			get;
			set;
		}

		public byte[] EdgeColor
		{
			get;
			set;
		}

		public float Scale
		{
			get;
			set;
		}

		public bool[] IKEnabled
		{
			get;
			set;
		}

		public MvdModelRelation[] ModelRelation
		{
			get;
			set;
		}

		public MvdModelPropertyFrame()
		{
			this.Visible = true;
			this.Physics = true;
			this.EdgeWidth = 1;
			this.EdgeColor = new byte[] { 0, 0, 0 };
			this.IKEnabled = new bool[0];
			this.ModelRelation = new MvdModelRelation[0];
		}

		public static MvdModelPropertyFrame Parse(MvdModelPropertyData mpd, BinaryReader br)
		{
			var rt = new MvdModelPropertyFrame
			{
				FrameTime = br.ReadInt64(),
				Visible = br.ReadBoolean(),
				Shadow = br.ReadBoolean(),
				AddBlending = br.ReadBoolean(),
				Physics = br.ReadBoolean(),
			};

			if (mpd.MinorType >= 1)
			{
				rt.PhysicsStillMode = br.ReadBoolean();
				br.ReadBytes(3);		// reserved[3]
			}

			rt.EdgeWidth = br.ReadSingle();
			rt.EdgeColor = new[] { br.ReadByte(), br.ReadByte(), br.ReadByte(), br.ReadByte() };

			if (mpd.MinorType >= 2)
				rt.Scale = br.ReadSingle();

			rt.IKEnabled = Enumerable.Range(0, mpd.IKBones.Length).Select(_ => br.ReadBoolean()).ToArray();

			if (mpd.MinorType >= 3)
				rt.ModelRelation = Enumerable.Range(0, mpd.ModelRelationCount).Select(_ => MvdModelRelation.Parse(br)).ToArray();

			return rt;
		}

		public void Write(MvdModelPropertyData mpd, BinaryWriter bw)
		{
			bw.Write(this.FrameTime);
			bw.Write(this.Visible);
			bw.Write(this.Shadow);
			bw.Write(this.AddBlending);
			bw.Write(this.Physics);

			if (mpd.MinorType >= 1)
			{
				bw.Write(this.PhysicsStillMode);
				bw.Write(new byte[3]);		// reserved[3]
			}

			bw.Write(this.EdgeWidth);
			this.EdgeColor.ForEach(bw.Write);

			if (mpd.MinorType >= 2)
				bw.Write(this.Scale);

			this.IKEnabled.ForEach(bw.Write);

			if (mpd.MinorType >= 3)
				this.ModelRelation.ForEach(_ => _.Write(bw));
		}
	}
}
