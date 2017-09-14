# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from VecMath import *
from vectorutil import randomvec
from random import seed, random, randint, uniform
import math

seed(890106)

RAD = math.pi / 180.0

bone_ownercenter = EntityBone(world, "Reimu", u"センター")
bone_target = EntityBone(world, "Marisa", u"センター")

veclist = []

num_way = 8
mat = Matrix3.RotationAxis(Vector3.UnitY, RAD * (360 / num_way))
vec = Vector3.UnitZ

for i in range(num_way):
	veclist.append(vec)
	vec  = vec * mat

def world_task(axis, r):
	
	rotate = Quaternion.RotationAxis(Vector3.UnitY, RAD * 6)
	
	root = EntityShot(world, "BONE", 0xFFFFFF)
	
	root.RecordWhenVelocityChanges = False
	root.Rot = Quaternion.RotationAxis(Vector3.UnitY ^ axis, math.acos(Vector3.UnitY * axis))
	
	def follow():
		root.Pos = bone_ownercenter.WorldPos
		root.Rot = rotate * root.Rot
	root.AddTask(follow, 0, 470, 0)
	
	follow()
	
	root.DiedDecision = lambda e: False
	root()
	
	shotlist = []
	for vec in veclist:
		
		shot = EntityShot(world, "M", 0xFFFFFF)
		shot.Pos = vec * r
		shot.RecordWhenVelocityChanges = False
		shot.ParentEntity = root
		
		shot()
		shotlist.append(shot)
	
	def shot_task_func1(task):
		
		flag = task.RunCount == 0
		
		upward = Vector3.UnitY * root.WorldMat
		mat = Matrix3.RotationAxis(upward, RAD * uniform(0, 40))
		
		shotStack = list(shotlist)
		def shot_task_func2():
			parentShot = shotStack.pop()
			
			vec = +(parentShot.WorldPos - root.WorldPos) * mat
			shot_amulet(parentShot.WorldPos, vec, upward)
			
		root.AddTask(shot_task_func2, 1 if flag else randint(3, 6), num_way, 0)
		
		task.Interval -= 10
	root.AddTask(shot_task_func1, 90, 4, 10, True)

world_task(+Vector3(1, 1, -0.5), 30.0)
world_task(+Vector3(1, -1, 0.5), 40.0)

def shot_amulet(pos, vec, upward):
	
	def shot_func1(original):
		shot = EntityShot(world, "AMULET", 0xFF00FF)
		shot.Pos = original.Pos
		shot.Velocity = +(bone_target.WorldPos - shot.Pos) * 8.0
		shot.Upward = original.Upward
		shot.LivingLimit = 100
		shot.DiedDecision = lambda e: e.LivingLimit != 0 and  e.FrameCount > e.LivingLimit
		
		shot()
	
	def shot_func2(original):
		shot = EntityShot(world, "S", 0xFF00FF)
		shot.Pos = original.Pos
		shot.Upward = original.Upward
		shot.AddTask(lambda s = shot :shot_func1(s) , 0, 1, 9)
		shot.LivingLimit = 10
		
		shot()
	
	def shot_func3(original):
		shot = EntityShot(world, "AMULET", 0xFF0000)
		shot.Pos = original.Pos
		shot.Velocity = (bone_target.WorldPos - shot.Pos) * 0.0025
		shot.Upward = original.Upward
		shot.SetMotionInterpolationCurve(Vector2(0.2, 0.8), Vector2(0.2, 0.8), 40)
		shot.AddTask(lambda s = shot :shot_func2(s) , 0, 1, 39)
		shot.LivingLimit = 40
		
		shot()
	
	def shot_func4(original):
		shot = EntityShot(world, "S", 0xFF0000)
		shot.Pos = original.Pos
		shot.Upward = original.Upward
		shot.AddTask(lambda s = shot :shot_func3(s) , 0, 1, 9)
		shot.LivingLimit = 10
		
		shot()
	
	for j in range(16):
		shot = EntityShot(world, "AMULET", 0xFFFFFF)
		shot.Pos = pos
		shot.Velocity = vec * (0.5 * j + 2)
		shot.Upward = upward
		shot.AddTask(lambda s = shot: shot_func4(s) , 0, 1, 29)
		shot.SetMotionInterpolationCurve(Vector2(0.3, 0.7), Vector2(0.3, 0.7), 30)
		shot.LivingLimit = 30
		
		shot()