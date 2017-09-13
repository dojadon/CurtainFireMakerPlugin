# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin import World
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Mathematics import *
from VecMath import *
import System
import vectorutil as vutil
import math

RAD = math.pi / 180.0
RAND = System.Random()

num_shot = 80
interval = 5

target = Vector3(0, 0, 80)

veclist = []
Icosahedron.GetVertices(lambda v: veclist.append(v), 1)

bone = EntityBone(world, "Marisa", u"センター")

def world_task(task):
	
	mat = Matrix3.RotationAxis(vutil.randomvec(), RAD * RAND.NextDouble() * 180)
	
	for vec in veclist:
		
		shot = EntityShot(world, "STAR_M", 0xA00000 if task.RunCount % 2 == 0 else 0x0000A0)
		shot.Pos = bone.WorldPos
		shot.Velocity = vec * 1.2 * mat
		shot.SetMotionInterpolationCurve(Vector2(0.1, 0.9), Vector2(0.1, 0.9), 60)
		
		def bezier(shot = shot):
			shot.SetMotionInterpolationCurve(Vector2(0.8, 0.2), Vector2(0.8, 0.2), 60)
		shot.AddTask(bezier, 0, 1, 60)
		
		def move(shot = shot):
			shot.Velocity *= 3.4
		shot.AddTask(move, 0, 1, 120)
		
		shot()
		
world.AddTask(world_task, 3, 8, 325, True)
world.AddTask(world_task, 3, 10, 388, True)
world.AddTask(world_task, 1, 8, 447, True)
world.AddTask(world_task, 3, 10, 480, True)
world.AddTask(world_task, 3, 8, 510, True)
