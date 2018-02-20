# -*- coding: utf-8 -*-

import clr
clr.AddReference("CurtainFireMakerPlugin")
clr.AddReference("VecMath")
clr.AddReference("MMDataIO")
clr.AddReference("MikuMikuPlugin")
clr.AddReference("DxMath")

def init_shottype(provider):
	from CurtainFireScript.ShotTypes import *

	provider.RegisterShotType(
		ShotTypeBone("BONE"),
		ShotTypeBillboard("XS", "shot.pmx", 1.0),
		ShotTypeBillboard("S", "shot.pmx", 2.0),
		ShotTypeBillboard("M", "shot.pmx", 6.0),
		ShotTypePmx("DIA", "dia.pmx", 4.0),
		ShotTypePmx("DIA_BRIGHT", "dia_bright.pmx", 4.0),
		ShotTypePmx("RICE_M", "rice.pmx", 8.0),
		ShotTypePmx("BULLET", "bullet.pmx", 2.0),
		ShotTypePmx("SCALE", "scale.pmx", 4.0),
		ShotTypePmx("LASER", "laser_curve.pmx", 1.0),
		ShotTypePmx("LASER_LINE", "laser_line.pmx", 1.0),
		ShotTypePmx("AMULET", "amulet.pmx", 4.0),
		ShotTypePmx("MAGIC_CIRCLE", "magic_circle.pmx", 10.0),

		ShotTypeL("L", "shot_l.pmx", 16.0),
		ShotTypeStar("STAR_S", "star.pmx", 4.0),
		ShotTypeStar("STAR_M", "star.pmx", 8.0),
		ShotTypeButterfly("BUTTERFLY", "butterfly.pmx", 8.0),
		ShotTypeKnife("KNIFE", "knife.pmx", 4.0))

def output_pmx_log(data):
	print u"出力完了：" + data.Header.ModelName
	print u"頂点数：" + "{:,}".format(data.VertexArray.Length)
	print u"面数数：" + "{:,}".format(int(data.VertexIndices.Length / 3))
	print u"材質数：" + "{:,}".format(data.MaterialArray.Length)
	print u"テクスチャ数：" + "{:,}".format(data.TextureFiles.Length)
	print u"ボーン数：" + "{:,}".format(data.BoneArray.Length)
	print u"モーフ数：" + "{:,}".format(data.MorphArray.Length)

def output_vmd_log(data):
	pass
