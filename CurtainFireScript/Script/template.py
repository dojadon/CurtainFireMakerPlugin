# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import EntityShot
from VecMath import *
from SolidMath import *
import solidutil
import math

veclist = [+v for v in Solid.GetIcosahedron().Divide().Vertices]
beveled_veclist = []

for vec in veclist:
	beveled_veclist += solidutil.bevel(vec, veclist, 0.2)

for vec in veclist:
	#弾生成
	shot = EntityShot(world, "S", 0x0000FF)
	#弾の移動量設定
	shot.Velocity = vec * 3.0
	#弾配置
	shot()

for vec in beveled_veclist:
	#弾生成
	shot = EntityShot(world, "S", 0xFF0000)
	#弾の移動量設定
	shot.Velocity = vec * 3.0
	#弾配置
	shot()
