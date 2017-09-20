using System;
using System.IO;

namespace CsMmdDataIO.Mvd.Data
{
	public struct MvdEffectParameterData
	{
		public bool? Boolean
		{
			get;
			set;
		}

		public int? Integer
		{
			get;
			set;
		}

		public float? Single
		{
			get;
			set;
		}

		public float? Single2
		{
			get;
			set;
		}

		public float? Single3
		{
			get;
			set;
		}

		public float? Single4
		{
			get;
			set;
		}

		public MvdEffectParameterData(bool value)
			: this()
		{
			this.Boolean = value;
		}

		public MvdEffectParameterData(int value)
			: this()
		{
			this.Integer = value;
		}

		public MvdEffectParameterData(float value)
			: this()
		{
			this.Single = value;
		}

		public MvdEffectParameterData(float value1, float value2)
			: this()
		{
			this.Single = value1;
			this.Single2 = value2;
		}

		public MvdEffectParameterData(float value1, float value2, float value3)
			: this()
		{
			this.Single = value1;
			this.Single2 = value2;
			this.Single3 = value3;
		}

		public MvdEffectParameterData(float value1, float value2, float value3, float value4)
			: this()
		{
			this.Single = value1;
			this.Single2 = value2;
			this.Single3 = value3;
			this.Single4 = value4;
		}

		public MvdEffectParameterData(float[] value)
			: this()
		{
			for (int i = 0; i < value.Length; i++)
				switch (i)
				{
					case 0:
						this.Single = value[i];

						break;
					case 1:
						this.Single2 = value[i];

						break;
					case 2:
						this.Single3 = value[i];

						break;
					case 3:
						this.Single4 = value[i];

						break;
				}
		}

		public static MvdEffectParameterData Parse(MvdEffectPropertyData epd, int index, BinaryReader br)
		{
			switch (epd.Parameters[index].Type)
			{
				case MvdEffectParameterType.Boolean:
					return new MvdEffectParameterData(br.ReadBoolean());
				case MvdEffectParameterType.Integer:
					return new MvdEffectParameterData(br.ReadInt32());
				case MvdEffectParameterType.Single:
					return new MvdEffectParameterData(br.ReadSingle());
				case MvdEffectParameterType.Single2:
					return new MvdEffectParameterData(br.ReadSingle(), br.ReadSingle());
				case MvdEffectParameterType.Single3:
					return new MvdEffectParameterData(br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				case MvdEffectParameterType.Single4:
					return new MvdEffectParameterData(br.ReadSingle(), br.ReadSingle(), br.ReadSingle(), br.ReadSingle());
				default:
					throw new NotSupportedException();
			}
		}

		public void Write(BinaryWriter bw)
		{
			if (this.Boolean.HasValue)
				bw.Write(this.Boolean.Value);
			else if (this.Integer.HasValue)
				bw.Write(this.Integer.Value);
			else if (this.Single.HasValue)
				bw.Write(this.Single.Value);
			else if (this.Single2.HasValue)
				bw.Write(this.Single2.Value);
			else if (this.Single3.HasValue)
				bw.Write(this.Single3.Value);
			else if (this.Single4.HasValue)
				bw.Write(this.Single4.Value);
		}
	}
}
