using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdEffectPropertyData : MvdFixedItemSection
	{
		public IList<MvdEffectPropertyFrame> Frames
		{
			get;
			set;
		}

		public IList<MvdEffectParameter> Parameters
		{
			get;
			set;
		}

		public MvdEffectPropertyData()
			: base(MvdTag.EffectProperty)
		{
			this.Frames = new List<MvdEffectPropertyFrame>();
			this.Parameters = new List<MvdEffectParameter>();
		}

		protected override void ReadExtensionRegion(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			if (br.GetRemainingLength() >= 4)
				this.Parameters = Enumerable.Range(0, br.ReadInt32()).Select(_ => MvdEffectParameter.Parse(br)).ToList();
		}

		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.Frames.Add(MvdEffectPropertyFrame.Parse(this, br));
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			bw.Write(this.Parameters.Count);
			this.Parameters.ForEach(_ => _.Write(bw));
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 0;
			this.RawCount = this.Frames.Count;

			base.Write(document, bw);
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.Frames[index].Write(bw);
		}
	}
}
