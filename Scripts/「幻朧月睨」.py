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

posList = [Vector3(40, 0, 0), Vector3(0, 40, 20)]

def world_task():

	posStack = posList[:]

	def shot_m_task():

		pos = posStack.pop()
		mat = Matrix3.RotationAxis(randomvec(), RAD * 180 * random())

		def shot_m(task):
			for vec in vecList:
				vec = vec * mat

				shot = EntityShot(world, "M", 0x0000A0)
				shot.Pos = vec * task.RunCount * 20 + pos
				shot.LivingLimit = 90
				shot()

				def move(shot = shot, vec = vec):
					newShot = EntityShot(world, "M", 0xA00000)
					newShot.Pos = shot.Pos
					newShot.Velocity = vec * 2.4
					newShot.LivingLimit = 200
					newShot()
				shot.AddTask(move, 0, 1, shot.LivingLimit)

		world.AddTask(shot_m, 2, 12, 30, True)

		def shot_bullet(prop, speed, limit):

			mat = Matrix3.RotationAxis(randomvec(), RAD * 180 * random())

			for vec in vecList:

				vec = vec * mat

				shot = EntityShot(world, prop)
				shot.Velocity = vec * speed
				shot.LivingLimit = limit
				shot()
		world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0xA00000), 4.8, 30), 0, 1, 30)
		world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0x0000A0), 4.5, 30), 0, 1, 30)
		world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0xA00000), 4.5, 28), 0, 1, 32)
		world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0x0000A0), 4.2, 28), 0, 1, 32)
		world.AddTask(lambda: shot_bullet(ShotProperty("BULLET", 0xA00000), 4.2, 24), 0, 1, 36)

	world.AddTask(shot_m_task, 90, 2, 0)
world.AddTask(world_task, 0, 1, 0)
