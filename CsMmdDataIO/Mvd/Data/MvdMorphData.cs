using System.Collections.Generic;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdMorphData : MvdFixedItemSection
	{
		public IList<MvdMorphFrame> Frames
		{
			get;
			set;
		}

		public int Key
		{
			get
			{
				return this.RawKey;
			}
			set
			{
				this.RawKey = value;
			}
		}

		public int ParentClipId
		{
			get;
			set;
		}

		public MvdMorphData()
			: base(MvdTag.Morph)
		{
			this.Frames = new List<MvdMorphFrame>();
		}

		protected override void ReadExtensionRegion(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			if (this.MinorType >= 1)
				this.ParentClipId = br.ReadInt32();
		}
		
		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.Frames.Add(MvdMorphFrame.Parse(br));
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			if (this.MinorType >= 1)
				bw.Write(this.ParentClipId);
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 1;
			this.RawCount = this.Frames.Count;

			base.Write(document, bw);
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.Frames[index].Write(bw);
		}
	}
}
