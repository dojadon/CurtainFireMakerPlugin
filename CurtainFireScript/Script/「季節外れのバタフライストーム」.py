# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Solids import WavefrontLoader
from VecMath import *
import math

RAD = math.pi / 180.0

vecList = []
WavefrontLoader.GetVertices("ico.obj", lambda v: vecList.append(v))

ownerbone = EntityBone(world, "Wriggle", u"センター")

for vec in vecList:
	
	axisList = [vec ^ (Vector3.UnitX ^ vec), vec ^ (Vector3.UnitZ ^ vec)]
	
	for axis in axisList:
		
		parent1 = Entity(world)
		parent1.Pos = ownerbone.WorldPos + vec * 20.0
		parent1()
		
		mat0 = Matrix3.RotationAxis(axis, RAD * 16)
		mat1 = Matrix3.RotationAxis(axis, RAD * 80)
		mat2 = Matrix3.RotationAxis(axis, RAD * -6)
		matList = [mat0, mat1, mat2, mat2]
		
		def shot_task_func0(task, parent1 = parent1, matList = matList):
			
			def shot_task_func1(task = task, parent1 = parent1, matList = matList):
				
				parent1.Pos = parent1.Pos * matList[0]
				matList[2] = matList[3] *  matList[2]
				
				shot = EntityShot(world, "S", 0xFFFFFF)
				shot.Pos = parent1.Pos
				shot.Velocity = +shot.Pos * 1
				shot.SetMotionInterpolationCurve(Vector2(0.2, 0.8), Vector2(0.2, 0.8), 60)
				shot.LivingLimit = 60
				shot()
				
				def shot_task_func2(shot = shot, matList = matList):
					butterfly = EntityShot(world, "FLY", 0xA0A000)
					butterfly.Pos = shot.Pos
					butterfly.Velocity = +shot.Pos * 2
					butterfly.LivingLimit = 150
					butterfly()
					
					butterfly = EntityShot(world, "FLY", 0x00A000)
					butterfly.Pos = shot.Pos
					butterfly.Velocity = (+shot.Pos * 2) *matList[1]
					butterfly.LivingLimit = 150
					butterfly()
					
					if task.RunCount == 1:
						butterfly = EntityShot(world, "FLY", 0x0000A0)
						butterfly.Pos = shot.Pos
						butterfly.Velocity = (+shot.Pos * 2) * (matList[1] ^ 2) * matList[2]
						butterfly.LivingLimit = 150
						butterfly()
						
				shot.AddTask(shot_task_func2, 0, 1, 59)
			parent1.AddTask(shot_task_func1, 5, 16, 0)
		parent1.AddTask(shot_task_func0, 150, 2, 0, True)
		
		parent2 = Entity(world)
		parent2.Pos = ownerbone.WorldPos + vec * 20.0
		parent2()
		
		mat0 = Matrix3.RotationAxis(axis, RAD * 16)
		
		def shot_task_func3(task, parent2 = parent2, mat0 = mat0):
			def shot_task_func4(task = task, parent2 = parent2, mat0 = mat0):
			
				parent2.Pos = parent2.Pos * (mat0 if task.RunCount % 2 == 0 else ~mat0)
				
				shot = EntityShot(world, "FLY", 0x0000A0 if task.RunCount % 2 == 0 else 0xA00000)
				shot.Pos = parent2.Pos
				shot.Velocity = +parent2.Pos * 6
				shot.LivingLimit = 80
				shot()
			parent2.AddTask(shot_task_func4, 2, 24, 0)
		parent2.AddTask(shot_task_func3, 50, 4, 90, True)
		