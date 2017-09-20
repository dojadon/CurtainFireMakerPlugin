using System.Collections.Generic;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdNameList : MvdSection
	{
		public IDictionary<int, string> Names
		{
			get;
			set;
		}

		public MvdNameList()
			: base(MvdTag.NameList)
		{
			this.Names = new Dictionary<int, string>();
		}

		protected override void Read(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			for (int i = 0; i < this.RawCount; i++)
				this.Names.Add(br.ReadInt32(), document.Encoding.GetString(br.ReadSizedBuffer()));
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 0;
			this.RawCount = this.Names.Count;

			base.Write(document, bw);

			foreach (var i in this.Names)
			{
				var buf = document.Encoding.GetBytes(i.Value);

				bw.Write(i.Key);
				bw.Write(buf.Length);
				bw.Write(buf);
			}
		}
	}
}
