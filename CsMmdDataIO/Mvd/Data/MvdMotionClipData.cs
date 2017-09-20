using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdMotionClipData : MvdFixedItemSection
	{
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

		public MvdMotionClip MotionClip
		{
			get;
			set;
		}

		public MvdMotionClipData()
			: base(MvdTag.MotionClip)
		{
		}

		protected override void Read(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			// フォーマットバグ対策
			if (this.MinorType == 0)
				this.RawItemSize -= 4;
			
			base.Read(document, obj, br);
		}

		protected override void ReadExtensionRegion(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.ParentClipId = br.ReadInt32();
		}

		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.MotionClip = MvdMotionClip.Parse(br);
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			bw.Write(this.ParentClipId);
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 0;
			this.RawCount = 1;

			base.Write(document, bw);
		}

		protected override void BeforeWriteHeader(MvdDocument document, BinaryWriter bw)
		{
			// フォーマットバグ対策
			if (this.MinorType == 0)
				this.RawItemSize += 4;
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.MotionClip.Write(bw);
		}
	}
}
