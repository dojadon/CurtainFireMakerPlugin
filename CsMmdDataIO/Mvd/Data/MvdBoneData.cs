using System.Collections.Generic;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdBoneData : MvdFixedItemSection
	{
		public IList<MvdBoneFrame> Frames
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

		public int StageCount
		{
			get;
			set;
		}

		public int ParentClipId
		{
			get;
			set;
		}

		public MvdBoneData()
			: base(MvdTag.Bone)
		{
			this.Frames = new List<MvdBoneFrame>();
		}

		protected override void ReadExtensionRegion(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			if (br.GetRemainingLength() >= 4)
				this.StageCount = br.ReadInt32();

			if (this.MinorType >= 2)
				this.ParentClipId = br.ReadInt32();
		}

		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.Frames.Add(MvdBoneFrame.Parse(this, br));
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			bw.Write(this.StageCount);

			if (this.MinorType >= 2)
				bw.Write(this.ParentClipId);
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
