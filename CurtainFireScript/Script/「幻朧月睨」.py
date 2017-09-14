# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Solids import WavefrontLoader
from VecMath import *
from vectorutil import randomvec
from random import random
import math

RAD = math.pi / 180.0

vecList = []
WavefrontLoader.GetVertices("ico_tru.obj", lambda v: vecList.append(v))

ownerbone = EntityBone(world, "Reisen", u"右人指３")

owner = EntityShot(world, "BONE", 0xFFFFFF)
owner.RecordWhenVelocityChanges = False
def follow(): owner.Pos = ownerbone.WorldPos
owner.AddTask(follow, 0, 0, 0)
owner()

def world_task():

	mat = Matrix3.RotationAxis(randomvec(), RAD * 180 * random())

	def shot_m(task):

		for vec in vecList:
			vec = vec * mat

			shot = EntityShot(world, "M", 0x0000A0)
			shot.ParentEntity = owner
			shot.Pos = vec * task.RunCount * 20
			shot.LivingLimit = 80
			shot()

			def move(shot = shot, vec = vec):
				newShot = EntityShot(world, "M", 0xA00000)
				newShot.Pos = shot.WorldPos
				newShot.Velocity = vec * 2.4
				newShot.LivingLimit = 200
				newShot()
			shot.AddTask(move, 0, 1, shot.LivingLimit)
	world.AddTask(shot_m, 1, 20, 0, True)

	def shot_bullet(prop, speed, limit, dis):

		mat = Matrix3.RotationAxis(randomvec(), RAD * 180 * random())

		for vec in vecList:

			vec = vec * mat

			shot = EntityShot(world, prop)
			shot.Velocity = vec * speed
			shot.Pos = vec * dis + ownerbone.WorldPos
			shot.LivingLimit = limit
			shot()
	world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0xA00000), 4.8, 30, 0), 0, 1, 0)
	world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0x0000A0), 4.5, 30, 0), 0, 1, 0)
	world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0xA00000), 4.5, 28, 0), 0, 1, 2)
	world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0x0000A0), 4.2, 28, 0), 0, 1, 2)
	world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0xA00000), 4.2, 24, 0), 0, 1, 6)

	world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0xA00000), 4.5, 30, 160), 0, 1, 0)
	world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0x0000A0), 4.2, 28, 160), 0, 1, 2)
	world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0xA00000), 4.0, 24, 160), 0, 1, 6)
world.AddTask(world_task, 80, 4, 0)
