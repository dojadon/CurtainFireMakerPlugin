# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin import World
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Mathematics import *
from VecMath import *
import vectorutil as vutil
import System
import math

RAD = math.pi / 180.0
RAND = System.Random()

colorlist = [0xA00000, 0x00A000, 0x0000A0, 0xA0A000, 0xA000A0, 0x00A0A0, 0xFF20A0]

way = 6
mat = Matrix3.RotationAxis(Vector3.UnitZ, RAD * (360 / way))
vec = Vector3.UnitY

veclist = [Vector3.Zero]

for i in range(way):
	veclist.append(vec)
	vec = vec * mat

root = EntityShot(world, "BONE", 0xFFFFFF)
root.RecordWhenVelocityChanges = False
root.Pos = Vector3(0, 0, 160)

rot1 = Quaternion.RotationAxis(Vector3.UnitZ, RAD * 5)
rot2 = Quaternion.RotationAxis(vutil.randomvec(), RAD * 5)
def rotate():
	root.Rot = rot1 * root.Rot
	root.Rot = root.Rot * rot2
root.AddTask(rotate, 3, 120, 0)
root()

colorstack = list(colorlist)

def decision_death1(entity):
	
	if abs(entity.Pos.x) > 400 or abs(entity.Pos.y) > 400 or entity.Pos.z > 800 or entity.Pos.z < -300:
		
		shot = EntityShot(world, "DIA", 0xFFFFFF)
		shot.Pos = entity.Pos
		shot.Velocity = -entity.Velocity
		shot.CheckWorldOut = decision_death2
		shot()
		return True
	return False

decision_death2 = lambda e: (abs(e.Pos.x) > 400 or abs(e.Pos.y) > 400 or e.Pos.z > 800 or e.Pos.z < -300) and e.FrameCount > 10

for vec in veclist:
	
	color = colorstack.pop()
	
	parent = EntityShot(world, "MAG_CIR", 0xA0A0A0)
	parent.ParentEntity = root
	parent.RecordWhenVelocityChanges = False
	parent.Pos = vec * 20
	
	def shot_dia(parent = parent, color = color):
		
		vec = Vector3.UnitX * parent.WorldMat
		
		for i in range(2):
			
			shot = EntityShot(world, "DIA", color)
			shot.Pos = parent.WorldPos
			shot.Velocity = vec * 2.4
			shot.CheckWorldOut = decision_death1
			shot()
			
			vec = -vec
	parent.AddTask(shot_dia, 3, 120, 0)
	parent()
	
veclist = []
Icosahedron.GetVertices(lambda v: veclist.append(v), 0)

axislist = [Vector3.UnitX, Vector3.UnitZ]

for vec in veclist:
	
	for axis in axislist:
		
		parent = Entity(world)
		parent.Pos = vec
		
		def shot_s(task, parent = parent, mat = Matrix3.RotationAxis(axis, RAD * 10)):
			parent.Pos = parent.Pos * mat
			
			shot = EntityShot(world, "S", colorlist[task.RunCount % len(colorlist)])
			shot.Velocity = parent.Pos * 3.4
			shot()
			
		parent.AddTask(shot_s, 10, 40, 0, True)
		parent()
		
