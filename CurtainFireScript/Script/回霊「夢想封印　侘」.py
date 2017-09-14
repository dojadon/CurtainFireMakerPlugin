# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Solids import WavefrontLoader
from VecMath import *
import math
from random import random, randint
from vectorutil import *

RAD = math.pi / 180.0

TARGET = Vector3(0, 0, 100)

#BONE_CENTER_R = EntityBone(u"博麗霊夢_n＋式", u"センター")

vecList = []
WavefrontLoader.GetVertices("ico.obj", lambda v: vecList.append(v))

def world_task_func1():
	for v in vecList:
		shot = EntityShot(world, "L", 0xFFFFFF)
		shot.Pos = v * 12.0
		shot.Velocity = v * 2.0
		shot.LivingLimit = 100
		shot()
#world.AddTask(world_task_func1, 30, 10, 10)

def world_task_func2():
		
	vec = randomvec()
	upward = vec ^ (vec ^ randomvec())
	mat = Matrix3.RotationAxis(upward, RAD * (20.0 + random() * 20.0))
	
	vecmat = [vec, mat]
	
	def world_task_func3(vm = vecmat):
		shot_fan_shape(vm[0], upward)
		vm[0] = vm[0] * vm[1]
	world.AddTask(world_task_func3, randint(2, 8), 8, 0)
world.AddTask(world_task_func2, 90, 5, 10)

def shot_fan_shape(vec, upward):
	
	mat = Matrix3.RotationAxis(vec ^ (vec ^ Vector3.UnitY), RAD * (4.0 + random() * 6.0))
	vecList = [vec * mat, vec, vec * ~mat]
	upwardList = [upward * mat, upward, upward * mat]
	
	def shot_func1(original):
		shot = EntityShot(world, "AMULET", 0xFF00FF)
		shot.Pos = original.Pos
		shot.Velocity = +(TARGET - shot.Pos) * 2.0
		shot.Upward = original.Upward
		shot.LivingLimit = 100
		
		shot()
	
	def shot_func2(original):
		shot = EntityShot(world, "S", 0xFF00FF)
		shot.Pos = original.Pos
		shot.Upward = original.Upward
		shot.AddTask(lambda s = shot :shot_func1(s) , 0, 1, 19)
		shot.LivingLimit = 20
		
		shot()
	
	def shot_func3(original):
		shot = EntityShot(world, "AMULET", 0xFF0000)
		shot.Pos = original.Pos
		shot.Velocity = (TARGET - shot.Pos) * 0.02
		shot.Upward = original.Upward
		shot.SetMotionInterpolationCurve(Vector2(0.2, 1.8), Vector2(0.2, 1.8), 40)
		shot.AddTask(lambda s = shot :shot_func2(s) , 0, 1, 39)
		shot.LivingLimit = 40
		
		shot()
	
	def shot_func4(original):
		shot = EntityShot(world, "S", 0xFF0000)
		shot.Pos = original.Pos
		shot.Upward = original.Upward
		shot.AddTask(lambda s = shot :shot_func3(s) , 0, 1, 19)
		shot.LivingLimit = 20
		
		shot()
	
	for i in range(len(vecList)):
		for j in range(6):
			shot = EntityShot(world, "AMULET", 0xFFFFFF)
			shot.Velocity = vecList[i] * (0.25 * j + 1)
			shot.Upward = upwardList[i]
			shot.AddTask(lambda s = shot :shot_func4(s) , 0, 1, 39)
			shot.SetMotionInterpolationCurve(Vector2(0.3, 1.7), Vector2(0.3, 1.7), 40)
			shot.LivingLimit = 40
			
			shot()
