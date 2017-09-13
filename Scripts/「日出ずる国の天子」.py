# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from VecMath import *
from vectorutil import *
import math

RAD = math.pi / 180.0

world.FrameCount = 610

ownerbone = EntityBone(world, "Keine", u"センター")
targetbone = EntityBone(world, "Reimu", u"センター")

wayVerti = 10
wayHoriz = 20

matVerti = Matrix3.RotationAxis(Vector3.UnitX, RAD * (180 / (wayVerti + 2)))
matHoriz = Matrix3.RotationAxis(Vector3.UnitY, RAD * (360 / wayHoriz))

veclist1 = []
veclist2 = []
vec1 = Vector3(0, -1, 0)

for i in range(wayVerti):
	vec1 = vec1 * matVerti
	veclist2.append(vec1)

for i in range(wayHoriz):
	veclist2 = map(lambda v: v * matHoriz, veclist2)
	veclist1.append(veclist2)

def world_task1(vecstack, color, rot):
	
	def shot_laser():
		veclist2 = vecstack.pop()
		
		for vec in veclist2:
			
			vec = vec * rot
			
			shot = EntityShot(world, "LASER_LINE", color)
			
			if shot.ModelData.OwnerEntities.Count == 1:
				for vert in shot.ModelData.Vertices:
					vert.Pos = Vector3(vert.Pos.x * 1.6, vert.Pos.y * 1.6, vert.Pos.z * 4000)
			
			morph = shot.CreateVertexMorph("V_" + shot.MaterialMorph.MorphName, lambda v: Vector3(-v.x * 0.9, -v.y * 0.9, 0))
			shot.AddVmdMorph(0, 1, morph)
			shot.AddVmdMorph(59, 1, morph)
			shot.AddVmdMorph(60, 0, morph)
			shot.AddVmdMorph(120, 0, morph)
			
			shot.RecordWhenVelocityChanges = False
			shot.Pos = ownerbone.WorldPos
			shot.Rot = Matrix3.LookAt(vec, Vector3.UnitY)
			
			shot.LivingLimit = 120
			shot()
			
	world.AddTask(shot_laser, 2, wayHoriz, 0)
world.AddTask(lambda: world_task1(veclist1[:], 0xA00000, Matrix3.Identity), 200, 2, 0)
world.AddTask(lambda: world_task1(veclist1[::-1], 0x0000A0, Matrix3.RotationAxis(Vector3.UnitY, RAD * 0.5 * (360 / wayHoriz))), 200, 2, 40)

vecList3 = [+Vector3(-1, 1, 1), +Vector3(1, 1, -1), +Vector3(1, -1, 1), +Vector3(-1, -1, -1)]

def world_task2():
	for vec in vecList3:
		posList = [Vector3(40, 20, 0), Vector3(-40, 20, 0)]
		
		rot = Matrix3.RotationAxis(vec ^ (vec ^ Vector3.UnitY), RAD * 30)
		rotList = [rot, ~rot]
		
		for i in range(len(posList)):
			pos = posList[i]
			quat = rotList[i]
			
			parentShot1 = EntityShot(world, "BONE", 0xFFFFFF)
			parentShot1.Pos = pos
			parentShot1.RecordWhenVelocityChanges = False
			parentShot1.ParentEntity = ownerbone
			parentShot1()
			
			parentShot2 = EntityShot(world, "MAG_CIR", 0xFFFFFF)
			parentShot2.RecordWhenVelocityChanges = False
			parentShot2.Pos = vec * 12
			parentShot2.Rot = Matrix3.RotationAxis(Vector3.UnitZ ^ vec, math.acos(vec.z))
			parentShot2.ParentEntity = parentShot1
			parentShot2()
			
			def shot_s(parentShot1 = parentShot1, parentShot2 = parentShot2, quat = quat):
				parentShot1.Rot = parentShot1.Rot * quat
				
				shot = EntityShot(world, "S", 0x0000A0)
				shot.Velocity = +(parentShot2.WorldPos - parentShot1.WorldPos) * -1.0
				shot.Pos = parentShot2.WorldPos
				shot.LivingLimit = 300
				shot()
				
				shot = EntityShot(world, "S", 0xA00000)
				shot.Velocity = +(targetbone.WorldPos - parentShot2.WorldPos) * 1.0
				shot.Pos = parentShot2.WorldPos
				shot.LivingLimit = 300
				shot()
				
			parentShot1.AddTask(shot_s, 20, 12, 0)
world.AddTask(world_task2, 0, 1, 0)
