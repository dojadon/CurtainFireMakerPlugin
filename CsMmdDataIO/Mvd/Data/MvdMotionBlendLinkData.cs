using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdMotionBlendLinkData : MvdFixedItemSection
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

		public MvdMotionBlendLink MotionBlendLink
		{
			get;
			set;
		}

		public MvdMotionBlendLinkData()
			: base(MvdTag.MotionBlend)
		{
		}

		protected override void ReadExtensionRegion(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.ParentClipId = br.ReadInt32();
		}

		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			this.MotionBlendLink = MvdMotionBlendLink.Parse(br);
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			bw.Write(this.ParentClipId);
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 1;
			this.RawCount = 1;

			base.Write(document, bw);
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.MotionBlendLink.Write(bw);
		}
	}
}
