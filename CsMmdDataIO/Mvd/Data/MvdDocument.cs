using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Linearstar.Keystone.IO;

namespace CsMmdDataIO.Mvd.Data
{
	/// <summary>
	/// Motion Vector Data File created by Mogg
	/// </summary>
	public class MvdDocument
	{
		public const string DisplayName = "Motion Vector Data file";
		public const string Filter = "*.mvd";

		public float Version
		{
			get;
			set;
		}

		public Encoding Encoding
		{
			get;
			set;
		}

		public IList<MvdObject> Objects
		{
			get;
			set;
		}

		public MvdDocument()
		{
			this.Objects = new List<MvdObject>();
		}

		public static MvdDocument Parse(Stream stream)
		{
			var rt = new MvdDocument();
			var systemEncoding = Encoding.GetEncoding(932);

			// leave open
			var br = new BinaryReader(stream);
			var header = ReadMvdString(br, 30, systemEncoding);

			if (header != DisplayName)
				throw new InvalidOperationException("invalid format");

			rt.Version = br.ReadSingle();

			if (rt.Version >= 2)
				throw new NotSupportedException("specified format version not supported");

			switch (br.ReadByte())
			{
				case 0:
					rt.Encoding = Encoding.Unicode;

					break;
				case 1:
				default:
					rt.Encoding = Encoding.UTF8;

					break;
			}

			while (br.GetRemainingLength() > 1)
				rt.Objects.Add(MvdObject.Parse(rt, br));

			return rt;
		}

		static string ReadMvdString(BinaryReader br, int count, Encoding encoding)
		{
			return encoding.GetString(br.ReadBytes(count).TakeWhile(_ => _ != '\0').ToArray());
		}

		public void Write(Stream stream)
		{
			// leave open
			var bw = new BinaryWriter(stream);
			var buf = new byte[30];

			Encoding.GetEncoding(932).GetBytes(DisplayName, 0, DisplayName.Length, buf, 0);
			bw.Write(buf);
			bw.Write(this.Version);
			bw.Write((byte)(this.Encoding.CodePage == Encoding.Unicode.CodePage ? 0 : 1));

			foreach (var i in this.Objects)
				i.Write(this, bw);
		}
	}
}
