using System.Collections.Generic;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdProjectData : MvdFixedItemSection
	{
		public IList<MvdProjectFrame> Frames
		{
			get;
			set;
		}

		public MvdProjectData()
			: base(MvdTag.Project)
		{
			this.Frames = new List<MvdProjectFrame>();
		}

		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.Frames.Add(MvdProjectFrame.Parse(this, br));
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 1;
			this.RawCount = this.Frames.Count;

			base.Write(document, bw);
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.Frames[index].Write(this, bw);
		}
	}
}
