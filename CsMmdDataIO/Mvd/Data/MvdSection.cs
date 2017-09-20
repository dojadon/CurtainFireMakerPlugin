using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public abstract class MvdSection
	{
		protected MvdTag Tag
		{
			get;
			set;
		}

		public byte MinorType
		{
			get;
			protected set;
		}

		protected int RawKey
		{
			get;
			set;
		}

		protected int RawItemSize
		{
			get;
			set;
		}

		protected int RawCount
		{
			get;
			set;
		}

		protected MvdSection(MvdTag tag)
		{
			this.Tag = tag;
		}

		protected virtual void ReadExtensionRegion(MvdDocument document, MvdObject obj, BinaryReader br)
		{
		}

		protected virtual void WriteExtensionRegion(MvdDocument document, BinaryWriter bw)
		{
		}

		protected abstract void Read(MvdDocument document, MvdObject obj, BinaryReader br);

		public static MvdSection Parse(MvdDocument document, MvdObject obj, BinaryReader br)
		{
			var tag = (MvdTag)br.ReadByte();
			MvdSection rt = null;

			switch (tag)
			{
				case MvdTag.NameList:
					rt = new MvdNameList();

					break;
				case MvdTag.Bone:
					rt = new MvdBoneData();

					break;
				case MvdTag.Morph:
					rt = new MvdMorphData();

					break;
				case MvdTag.MotionClip:
					rt = new MvdMotionClipData();

					break;
				case MvdTag.MotionBlend:
					rt = new MvdMotionBlendLinkData();

					break;
				case MvdTag.ModelProperty:
					rt = new MvdModelPropertyData();

					break;
				case MvdTag.AccessoryProperty:
					rt = new MvdAccessoryPropertyData();

					break;
				case MvdTag.EffectProperty:
					rt = new MvdEffectPropertyData();

					break;
				case MvdTag.Camera:
					rt = new MvdCameraData();

					break;
				case MvdTag.CameraProperty:
					rt = new MvdCameraPropertyData();

					break;
				case MvdTag.Light:
					rt = new MvdLightData();

					break;
				case MvdTag.Project:
					rt = new MvdProjectData();

					break;
				case MvdTag.Filter:
					rt = new MvdFilterData();

					break;
				case MvdTag.Eof:
					br.ReadByte();

					return null;
			}

			rt.MinorType = br.ReadByte();
			rt.RawKey = br.ReadInt32();
			rt.RawItemSize = br.ReadInt32();
			rt.RawCount = br.ReadInt32();

			using (var exr = br.CreateSizedBufferReader())
				rt.ReadExtensionRegion(document, obj, exr);

			rt.Read(document, obj, br);

			return rt;
		}

		public virtual void Write(MvdDocument document, BinaryWriter bw)
		{
			bw.Write((byte)this.Tag);
			bw.Write(this.MinorType);
			bw.Write(this.RawKey);
			bw.Write(this.RawItemSize);
			bw.Write(this.RawCount);

			using (var ms = new MemoryStream())
			{
				using (var exw = new BinaryWriter(ms))
					WriteExtensionRegion(document, exw);

				bw.WriteSizedBuffer(ms.ToArray());
			}
		}
	}
}
