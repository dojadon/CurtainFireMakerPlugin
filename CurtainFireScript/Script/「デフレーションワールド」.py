# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Mathematics import *
from VecMath import *
import math

RAD = math.pi / 180.0

vecList = []
Icosahedron.GetVertices(lambda v: vecList.append(v), 1)

num_clone = 25

owner_centerbone = EntityBone(world, "Sakuya", u"センター")
qwner_r_handbone = EntityBone(world, "Sakuya", u"右手首")
qwner_l_handbone = EntityBone(world, "Sakuya", u"左手首")

targetbone = EntityBone(world, "Reimu", u"センター")

pauseList = []

def world_task_func():
	
	target = Entity(world)
	target.Pos = Vector3(100, 0, 50)
	target.Velocity = Vector3(-1, 1, 0)
	target.LivingLimit = 120
	target()
	
	cloneList = []
	
	def shot_knife1():
		mat = Matrix3.RotationAxis(Vector3.UnitY, RAD * 20)
		vec = Vector3.UnitZ * owner_centerbone.WorldMat * (mat ^ 2)
		mat = ~mat
		
		for i in range(5):
			for j in range(2):
				shot = EntityShot(world, "KNIFE", 0xFFD700)
				shot.Velocity = vec * (0.5 + j * 0.2)
				shot.Pos = owner_centerbone.WorldPos + shot.Velocity
				shot()
				
				pauseList.append(shot)
			vec = vec * mat
	world.AddTask(shot_knife1, 0, 1, 10)
	
	def shot_knife2(angle, axis):
		
		for vec in vecList:
			
			vec = vec * owner_centerbone.WorldMat
			
			mat = Matrix3.RotationAxis(vec ^ (vec ^ axis), angle)
			
			shot = EntityShot(world, "KNIFE", 0x0000A0)
			shot.Velocity = vec * 4.0
			shot.Pos = qwner_l_handbone.WorldPos + shot.Velocity
			shot.SetMotionInterpolationCurve(Vector2(0.3, 0.7), Vector2(0.3, 0.7), 30)
			
			def shot_task_func(shot = shot, mat = mat):
				shot.Velocity = shot.Velocity * mat * 0.5
			shot.AddTask(shot_task_func, 0, 1, 30)
			
			shot()
			
			cloneList.append(shot)
			pauseList.append(shot)
			
	world.AddTask(lambda: shot_knife2(RAD * 60, Vector3.UnitZ), 0, 1, 0)
	world.AddTask(lambda: shot_knife2(-RAD * 60, Vector3.UnitZ), 0, 1, 5)
	
	def shot_knife3():
		shot = EntityShot(world, "KNIFE", 0x0000A0)
		shot.Velocity = +(targetbone.WorldPos - qwner_l_handbone.WorldPos) * 2.0
		shot.Pos = qwner_l_handbone.WorldPos + shot.Velocity * 4
		shot()
		
		cloneList.append(shot)
		pauseList.append(shot)
	world.AddTask(shot_knife3, 4, 12, 10)
	
	funcList = []
	
	velocityDict = {}
	
	def pause():
		for shot in pauseList:
			velocityDict[shot] = shot.Velocity
			
			shot.Velocity = Vector3.Zero
			shot.RecordWhenVelocityChanges = False
			
			def shot_task_func(shot = shot):
				shot.AddVmdMotion()
				shot.RecordWhenVelocityChanges = True
				shot.Velocity = velocityDict[shot]
			funcList.append(shot_task_func)
	world.AddTask(pause, 0, 1, 56)
	
	def clone(task):
		for src in cloneList:
			funcList.append(clone_shot(src.Pos, velocityDict[src], task.RunCount - 1))
	world.AddTask(clone, 1, num_clone, 60, True)
	
	def move():
		for func in funcList:
			func()
	world.AddTask(move, 0, 1, 110)
	
world.AddTask(world_task_func, 120, 2, 600)

def clone_shot(pos, vec, index):
	interval = vec * 16
	flag = index < num_clone / 2
	
	shot = EntityShot(world, "KNIFE", 0xE0E0E0 if flag else 0xA0A0A0)
	shot.Pos = pos + interval * (-num_clone / 3 + index + (-1 if flag else 1) * 0.5)
	shot.RecordWhenVelocityChanges = False
	shot.Rot = Matrix3.LookAt(+vec, shot.Upward)
	shot.LivingLimit = 120 + (num_clone - index) + 25
	shot.DiedDecision = lambda e: e.FrameCount > shot.LivingLimit
	
	shot()
	pauseList.append(shot)
	
	def shot_task_func(shot = shot, vec = vec):
		
		shot.RecordWhenVelocityChanges = True
		shot.Velocity = +vec * 6
	
	return shot_task_func
