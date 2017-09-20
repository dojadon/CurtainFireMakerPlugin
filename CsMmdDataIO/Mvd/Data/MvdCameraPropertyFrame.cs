using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdCameraPropertyFrame
	{
		public long FrameTime
		{
			get;
			set;
		}

		public bool Perspective
		{
			get;
			set;
		}

		public bool Enabled
		{
			get;
			set;
		}

		public float Alpha
		{
			get;
			set;
		}

		public bool EffectEnabled
		{
			get;
			set;
		}

		public bool DynamicFov
		{
			get;
			set;
		}

		public float DynamicFovRate
		{
			get;
			set;
		}

		public float DynamicFovCoefficent
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

		public MvdCameraPropertyFrame()
		{
			this.Enabled = true;
			this.Perspective = true;
			this.Alpha = 1;
			this.EffectEnabled = true;
			this.DynamicFov = false;
			this.DynamicFovRate = 0.1f;
			this.DynamicFovCoefficent = 1;
			this.RelatedModelId = -1;
			this.RelatedBoneId = -1;
		}

		public static MvdCameraPropertyFrame Parse(MvdCameraPropertyData cpd, BinaryReader br)
		{
			var rt = new MvdCameraPropertyFrame
			{
				FrameTime = br.ReadInt64(),
				Enabled = br.ReadBoolean(),
				Perspective = br.ReadBoolean(),
				Alpha = br.ReadSingle(),
				EffectEnabled = br.ReadBoolean(),
			};

			switch (cpd.MinorType)
			{
				case 0:
					br.ReadByte();

					break;
				case 1:
				case 2:
					rt.DynamicFov = br.ReadBoolean();
					rt.DynamicFovRate = br.ReadSingle();
					rt.DynamicFovCoefficent = br.ReadSingle();

					if (cpd.MinorType == 2)
					{
						rt.RelatedModelId = br.ReadInt32();
						rt.RelatedBoneId = br.ReadInt32();
					}

					break;
			}

			return rt;
		}

		public void Write(MvdCameraPropertyData cpd, BinaryWriter bw)
		{
			bw.Write(this.FrameTime);
			bw.Write(this.Enabled);
			bw.Write(this.Perspective);
			bw.Write(this.Alpha);
			bw.Write(this.EffectEnabled);

			switch (cpd.MinorType)
			{
				case 0:
					bw.Write(false);
				
					break;
				case 1:
				case 2:
					bw.Write(this.DynamicFov);
					bw.Write(this.DynamicFovRate);
					bw.Write(this.DynamicFovCoefficent);

					if (cpd.MinorType == 2)
					{
						bw.Write(this.RelatedModelId);
						bw.Write(this.RelatedBoneId);
					}

					break;
			}
		}
	}
}
