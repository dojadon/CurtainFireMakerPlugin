# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from VecMath import *
from math import pi

RAD = pi / 180.0

veclist = []

way = 18
rot = Matrix3.RotationAxis(Vector3.UnitX, RAD * (180 / (way + 1)))
vec = -Vector3.UnitY

world.StartFrame = 330

ownerbone = EntityBone(world, "Remilia", u"右手首")

for i in range(way):
	vec = vec * rot
	veclist.append(vec)

parentbone = EntityShot(world, "BONE", 0xFFFFFF)
parentbone.RecordWhenVelocityChanges = False
def follow(): parentbone.Pos = ownerbone.WorldPos
follow()
parentbone.AddTask(follow, 1, 400, 0)
parentbone()

def world_task(task):
	for vec in veclist:
		for angle in [RAD, -RAD]:

			axis = vec ^ (vec ^ Vector3.UnitY)

			parent = Entity(world)
			parent.Pos = vec

			def shot_knife(task, parent=parent, axis=axis, rot=Matrix3.RotationAxis(axis, angle * 12)):

				shot = EntityShot(world, "KNIFE", 0xA00000)
				shot.ParentEntity = parentbone
				shot.Velocity = parent.Pos * (4.0 + (parent.Pos * Vector3.UnitZ) * -1 + task.RunCount * 0.03)
				shot.Upward = axis
				shot.LivingLimit = 120
				shot()

				parent.Pos = parent.Pos * rot
			parent.AddTask(shot_knife, 2, 60, 0, True)

			if ((angle > 0) if (task.RunCount % 2 == 0) else (angle < 0)):
				def shot_l(parent=parent):
					shot = EntityShot(world, "L", 0xFF0000)
					shot.ParentEntity = parentbone
					shot.Velocity = parent.Pos * 2.0
					shot.LivingLimit = 160
					shot()
				parent.AddTask(shot_l, 2, 12, 4)
			parent()
world.AddTask(world_task, 160, 2, 0, True)
