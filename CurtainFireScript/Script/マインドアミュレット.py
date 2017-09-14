# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
import math

RAD = math.pi / 180.0

world.StartFrame = 650

ownerparentbone = EntityBone(world, "Reimu", u"左人指１")

ownerbone = EntityBone(world, u"大幣", u"棒先")
ownerbone.ParentEntity = ownerparentbone

targetbone = EntityBone(world, "Keine", u"センター")

target = EntityShot(world, "BONE", 0xFFFFFF)
target.RecordWhenVelocityChanges = False
def follow(): target.Pos = targetbone.WorldPos
target.AddTask(follow, 0, 0, 0)
target()

def shot_amulet(frame = 30):
	
	parent = EntityShot(world, "BONE", 0xFFFFFF)
	parent.ParentEntity = target
	
	parent.Pos = ownerbone.WorldPos - target.WorldPos
	parent.Velocity = parent.Pos * (-1.0 / 30)
	
	parent()
	
	for i in range(2):
		
		shot = EntityShot(world, "AMULET", 0xA00080)
		shot.ParentEntity = parent
		shot.Velocity = Vector3.UnitX * (100 / frame * 0.5) * (i * 2 - 1)
		shot.SetMotionInterpolationCurve(Vector2(0.2, 0.8), Vector2(0.2, 0.8), frame / 2)
		
		def reverse(shot = shot):
			shot.Velocity *= -1
			shot.SetMotionInterpolationCurve(Vector2(0.8, 0.2), Vector2(0.8, 0.2), frame / 2)
		shot.AddTask(reverse, 0, 1, frame / 2)
		
		shot()

world.AddTask(shot_amulet, 2, 50, 1)