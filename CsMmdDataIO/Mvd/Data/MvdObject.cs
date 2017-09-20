using System.Collections.Generic;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdObject
	{
		public string ObjectName
		{
			get;
			set;
		}

		public string EnglishObjectName
		{
			get;
			set;
		}

		public float KeyFps
		{
			get;
			set;
		}

		public IList<MvdSection> Sections
		{
			get;
			set;
		}

		public MvdObject()
		{
			this.Sections = new List<MvdSection>();
		}

		public static MvdObject Parse(MvdDocument document, BinaryReader br)
		{
            var rt = new MvdObject()
            {
                ObjectName = document.Encoding.GetString(br.ReadSizedBuffer()),
                EnglishObjectName = document.Encoding.GetString(br.ReadSizedBuffer()),
                KeyFps = br.ReadSingle()
            };
            br.ReadSizedBuffer();	// reservedSize / reserved

			while (br.GetRemainingLength() > 1)
			{
				var section = MvdSection.Parse(document, rt, br);

				if (section == null)
					break;

				rt.Sections.Add(section);
			}

			return rt;
		}

		public void Write(MvdDocument document, BinaryWriter bw)
		{
			bw.WriteSizedBuffer(document.Encoding.GetBytes(this.ObjectName));
			bw.WriteSizedBuffer(document.Encoding.GetBytes(this.EnglishObjectName));
			bw.Write(this.KeyFps);
			bw.WriteSizedBuffer(new byte[0]);

			foreach (var i in this.Sections)
				i.Write(document, bw);

			bw.Write((byte)MvdTag.Eof);
			bw.Write((byte)0);
		}
	}
}
