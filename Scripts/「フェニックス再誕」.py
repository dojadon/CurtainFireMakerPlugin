# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin import World
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Mathematics import *
from System import Console
from VecMath import *
import math
import vectorutil as vutil

RAD = math.pi / 180.0

target = Vector3(80, 80, 80)

vecList = []
Icosahedron.GetVertices(lambda v: vecList.append(v), 1)

vecList0 = []
Icosahedron.GetVertices(lambda v: vecList0.append(v), 0)

def spawn_entity():
	
	for vec in vecList:
		
		flag = vec in vecList0
		
		axis = vec ^ (vec ^ Vector3.UnitY)
		
		entity = Entity(world)
		entity.Pos = vec
		entity.Rot = Matrix.RotationAxis(axis, RAD * 4)
		
		rotatePosMat = Matrix.RotationAxis(axis, RAD * 8)
		
		def shot_dia1(entity = entity, rotatePosMat = rotatePosMat):
			
			shot = EntityShot(world, "DIA", 0xA02020)
			shot.Velocity = entity.Pos * 2.0
			shot()
			
			entity.Pos = entity.Pos * rotatePosMat
			
		entity.AddTask(shot_dia1, 4, 4, 0)
		
		def shot_dia2(entity = entity, rotatePosMat = rotatePosMat):
			
			shot = EntityShot(world, "DIA", 0xA02020)
			shot.Velocity = entity.Pos * 2.8
			shot()
			
			entity.Pos = entity.Pos * rotatePosMat
			
		entity.AddTask(shot_dia2, 4, 6, 20)
		
		entity()

world.AddTask(spawn_entity, 120, 4, 0)

posList = []
posList.append(("L", Vector3(0, 0, 30))
posList.append(("M", Vector3(0, 0, 24))
posList.append(("S", Vector3(0, 0, 20))
posList.append(("S", Vector3(0, 0, 18))



def 