using System;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public abstract class MvdFixedItemSection : MvdSection
	{
		public MvdFixedItemSection(MvdTag tag)
			: base(tag)
		{
		}

		protected abstract void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br);
		protected abstract void WriteItem(MvdDocument document, BinaryWriter bw, int index);

		protected virtual void BeforeWriteHeader(MvdDocument document, BinaryWriter bw)
		{
		}

		protected override void Read(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			if (this.RawItemSize <= 0)
				throw new InvalidOperationException("RawItemSize must be greater than 0.");

			for (int i = 0; i < this.RawCount; i++)
				using (var ir = new BinaryReader(new MemoryStream(br.ReadBytes(this.RawItemSize))))
					ReadItem(document, obj, ir);
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			if (this.RawCount > 0)
			{
				using (var ms = new MemoryStream())
				using (var iw = new BinaryWriter(ms))
				{
					// write the first item to the temp buffer to get the item's size
					WriteItem(document, iw, 0);
					this.RawItemSize = (int)ms.Length;

					// then, write the headers to the real buffer
					BeforeWriteHeader(document, bw);
					base.Write(document, bw);

					// and then, copy the first item's temp buffer to the real one.
					bw.Write(ms.ToArray());
				}

				for (int i = 1; i < this.RawCount; i++)
					WriteItem(document, bw, i);
			}
			else
				base.Write(document, bw);
		}
	}
}
