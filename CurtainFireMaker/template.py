# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import EntityShot
from CurtainFireMakerPlugin.Solids import WavefrontLoader
from VecMath import *
import math

RAD = math.pi / 180.0

vecList = []
WavefrontLoader.GetVertices("ico.obj", lambda v: vecList.append(+v))

for vec in vecList:
	#弾生成
	shot = EntityShot(world, "STAR_M", 0xFF0000)
	
	shot.Velocity = vec
	#弾配置
	shot()
