using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public class MvdModelRelation
	{
		public int ExternalParentKey
		{
			get;
			set;
		}

		public int RelatedModelId
		{
			get;
			set;
		}

		public int RelatedBoneId
		{
			get;
			set;
		}

		public static MvdModelRelation Parse(BinaryReader br)
		{
			return new MvdModelRelation
			{
				ExternalParentKey = br.ReadInt32(),
				RelatedModelId = br.ReadInt32(),
				RelatedBoneId = br.ReadInt32(),
			};
		}

		public void Write(BinaryWriter bw)
		{
			bw.Write(this.ExternalParentKey);
			bw.Write(this.RelatedModelId);
			bw.Write(this.RelatedBoneId);
		}
	}
}
