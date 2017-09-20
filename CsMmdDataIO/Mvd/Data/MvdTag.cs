namespace CsMmdDataIO.Mvd.Data
{
	public enum MvdTag : byte
	{
		NameList,
		Bone = 16,
		Morph = 32,
		MotionClip = 40,
		MotionBlend = 48,
		ModelProperty = 64,
		AccessoryProperty = 80,
		EffectProperty = 88,
		Camera = 96,
		CameraProperty = 104,
		Light = 112,
		Project = 128,
		Filter = 136,
		Eof = 255,
	}
}
