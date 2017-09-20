using System.Collections.Generic;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdFilterData : MvdFixedItemSection
	{
		public IList<MvdFilterFrame> Frames
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

		public int ToneCurveControlPointCount
		{
			get;
			set;
		}

		public MvdFilterData()
			: base(MvdTag.Filter)
		{
			this.Frames = new List<MvdFilterFrame>();
		}

		protected override void ReadExtensionRegion(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.ToneCurveControlPointCount = br.ReadInt32();
		}

		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.Frames.Add(MvdFilterFrame.Parse(br));
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			bw.Write(this.ToneCurveControlPointCount);
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 2;
			this.RawCount = this.Frames.Count;

			base.Write(document, bw);
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.Frames[index].Write(bw);
		}
	}
}
