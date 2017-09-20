using System.Collections.Generic;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdLightData : MvdFixedItemSection
	{
		public IList<MvdLightFrame> Frames
		{
			get;
			set;
		}

		public MvdLightData()
			: base(MvdTag.Light)
		{
			this.Frames = new List<MvdLightFrame>();
		}

		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.Frames.Add(MvdLightFrame.Parse(br));
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
