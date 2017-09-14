# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Solids import WavefrontLoader
from VecMath import *
from math import pi, cos, sin, tan, acos, asin

RAD = pi / 180.0

veclist1 = []

way = 14
rot = Matrix3.RotationAxis(Vector3.UnitX, RAD * (180 / (way + 1)))
vec = -Vector3.UnitY

world.StartFrame = 330

ownerbone = EntityBone(world, "Mystia", u"センター")

for i in range(way):
	vec = vec * rot
	veclist1.append(vec)

veclist2 = []
WavefrontLoader.GetVertices("ico.obj", lambda v: veclist2.append(v))
veclist3 = []
WavefrontLoader.GetVertices("snub_cube.obj", lambda v: veclist3.append(v))

def world_task1(task, angle):
	
	for vec in veclist1:
		color = 0x0000A0 if angle < 0 else 0x00A0A0
		
		axis = vec ^ (vec ^ Vector3.UnitY)
		
		parent = Entity(world)
		parent.Pos = vec * Matrix3.RotationAxis(axis, angle * 40)
		
		def shot_scale(task, parent = parent, axis = axis, rot = Matrix3.RotationAxis(axis, angle * 7), color = color):
			
			shot = EntityShot(world, "SCALE", color)
			shot.Pos = ownerbone.WorldPos
			shot.Velocity = parent.Pos * 4.0 * ownerbone.WorldMat
			shot.Upward = axis * ownerbone.WorldMat
			shot.LivingLimit = 120
			shot()
			
			parent.Pos = parent.Pos * rot
		parent.AddTask(shot_scale, 0, 28, 0, True)
		parent()
world.AddTask(lambda t: world_task1(t, RAD), 28, 8, 0, True)
world.AddTask(lambda t: world_task1(t, -RAD), 28, 8, 14, True)

def world_task2():
	
	for vec in veclist2 + veclist3:
		
		flag = vec in veclist2
		
		parent = Entity(world)
		parent.Pos = vec
		
		def shot_s(parent = parent, rot = Matrix3.RotationAxis(vec ^ (vec ^ Vector3.UnitY), RAD * 12), flag = flag):
			
			shot = EntityShot(world, "S", 0x00A040)
			shot.Velocity = parent.Pos * (4 if flag else 6)
			shot.Pos = ownerbone.WorldPos
			shot.LivingLimit = 120
			shot()
			
			parent.Pos = parent.Pos * rot
		parent.AddTask(shot_s, 10, 30, 0 if flag else 5)
		parent()
world.AddTask(world_task2, 0, 1, 0)

