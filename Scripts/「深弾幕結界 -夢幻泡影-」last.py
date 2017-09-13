# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from VecMath import *
from random import random, gauss, uniform
import vectorutil as vutil
import math

RAD = math.pi / 180.0

def world_task():
	def shot_amulet(task):
		for i in range(16 + task.RunCount * 4):
			
			shot = EntityShot(world, "AMULET", 0xA00000)
			shot.RecordWhenVelocityChanges = False
			
			up = vutil.randomvec()
			vec = vutil.randomvec()
			
			shot.Pos = vec * task.RunCount * (5 + random() * 2)
			shot.Upward = up
			shot.Rot = Matrix3.LookAt(-vec, up)
			
			def move(shot = shot, vec = vec):
				shot.RecordWhenVelocityChanges = True
				shot.Velocity = vec * -0.05
			shot.AddTask(move, 0, 1, 90 - task.RunCount)
			shot()
	world.AddTask(shot_amulet, 2, 60, 0, True)
world.AddTask(world_task, 0, 1, 0)
