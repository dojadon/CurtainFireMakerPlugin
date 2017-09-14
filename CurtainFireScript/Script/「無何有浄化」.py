# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin import World
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Mathematics import *
from System import Console
from VecMath import *
import math
import vectorutil as vutil

world = World()
RAD = math.pi / 180.0

target = Vector3(80, 80, 80)

vecList = []
Icosahedron.GetVertices(lambda v: vecList.append(v), 0)

axisList = [Vector3.UnitX, Vector3.UnitZ]
angleList = [RAD, -RAD]

for vec in vecList:
	for angle in angleList:
		for axis in axisList:
			
			axis = vec ^ (vec ^ axis)
			
			parent = Entity(world)
			
			rotPosMat = Matrix.RotationAxis(axis, angle * 6)
			parent.Pos = vec
			
			def rotate(parent = parent, rotPosMat = rotPosMat, angle = angle):
				
				parent.Rot = Quaternion.Identity
				
				rotQuat = Quaternion.RotationAxis(axis, -angle * 4)
				
				def shot_dia():
					
					parent.Pos = parent.Pos * rotPosMat
					parent.Rot = parent.Rot * rotQuat
					
					shot = EntityShot(world, "DIA", 0xA00050 if angle < 0 else 0x5000A0)
					shot.Pos = parent.Pos * 160
					shot.Velocity = parent.Pos * parent.Rot * -1.6
					shot.LivingLimit = 120
					shot()
				parent.AddTask(shot_dia, 4, 8, 0)
			parent.AddTask(rotate, 32, 20, 0)
		
			parent()
		

def add_task():
	def shot_l(task):
		
		vec = +target
		axis = vec ^ (vec ^ Vector3.UnitY)
		
		angle = (task.RunCount - 1) * RAD * 5 * 0.5
		mat1 = Matrix.RotationAxis(axis, -RAD * 5)
		mat2 = Matrix.RotationAxis(axis, angle)
		
		for i in range(task.RunCount):
			
			shot = EntityShot(world, "L", 0x4000D0)
			shot.Velocity = vec * mat2 * 2.5
			shot()
			
			mat2 = mat2 * mat1
	world.AddTask(shot_l, 5, 4, 0, True)
world.AddTask(add_task, 90, 8, 90)