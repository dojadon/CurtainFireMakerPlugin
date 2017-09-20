using System.Collections.Generic;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdCameraPropertyData : MvdFixedItemSection
	{
		public IList<MvdCameraPropertyFrame> Frames
		{
			get;
			set;
		}

		public MvdCameraPropertyData()
			: base(MvdTag.CameraProperty)
		{
			this.Frames = new List<MvdCameraPropertyFrame>();
		}

		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.Frames.Add(MvdCameraPropertyFrame.Parse(this, br));
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 2;
			this.RawCount = this.Frames.Count;

			base.Write(document, bw);
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.Frames[index].Write(this, bw);
		}
	}
}
