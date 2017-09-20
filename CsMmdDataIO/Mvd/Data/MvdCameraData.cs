using System.Collections.Generic;
using System.IO;
using Linearstar.Keystone.IO;
using System.Linq;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdCameraData : MvdFixedItemSection
	{
		public IList<MvdCameraFrame> Frames
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

		public MvdCameraData()
			: base(MvdTag.Camera)
		{
			this.Frames = new List<MvdCameraFrame>();
		}

		protected override void ReadItem(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			MvdCameraPropertyFrame cpf;

			this.Frames.Add(MvdCameraFrame.Parse(this, br, out cpf));

			if (cpf != null)
			{
				var cpd = obj.Sections.OfType<MvdCameraPropertyData>().FirstOrDefault();

				if (cpd == null)
					obj.Sections.Add(cpd = new MvdCameraPropertyData());

				cpd.Frames.Add(cpf);
			}
		}

		protected override void ReadExtensionRegion(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			if (br.GetRemainingLength() >= 4)
				this.StageCount = br.ReadInt32();
		}

		public override void Write(MvdDocument document, BinaryWriter bw)
		{
			this.MinorType = 3;
			this.RawCount = this.Frames.Count;

			base.Write(document, bw);
		}

		protected override void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
			bw.Write(this.StageCount);
		}

		protected override void WriteItem(MvdDocument document, BinaryWriter bw, int index)
		{
			this.Frames[index].Write(this, bw);
		}
	}
}
