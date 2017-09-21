using System.IO;
using System.Linq;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdEffectPropertyFrame : MvdAccessoryPropertyFrame,  IKeyFrame
    {
		public MvdEffectParameterData[] Parameters
		{
			get;
			set;
		}

		public MvdEffectPropertyFrame()
		{
			this.Parameters = new MvdEffectParameterData[0];
		}

		public static MvdEffectPropertyFrame Parse(MvdEffectPropertyData epd, BinaryReader br)
		{
			return new MvdEffectPropertyFrame
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
				Parameters = Enumerable.Range(0, epd.Parameters.Count).Select(_ => MvdEffectParameterData.Parse(epd, _, br)).ToArray(),
			};
		}

		public override void Write(BinaryWriter bw)
		{
			base.Write(bw);
			this.Parameters.ForEach(_ => _.Write(bw));
		}
	}
}
