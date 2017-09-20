using System.Collections.Generic;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdAccessoryPropertyData : MvdFixedItemSection
	{
		public IList<MvdAccessoryPropertyFrame> Frames
		{
			get;
			set;
		}

		public MvdAccessoryPropertyData()
			: base(MvdTag.AccessoryProperty)
		{
			this.Frames = new List<MvdAccessoryPropertyFrame>();
		}

		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.Frames.Add(MvdAccessoryPropertyFrame.Parse(br));
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
