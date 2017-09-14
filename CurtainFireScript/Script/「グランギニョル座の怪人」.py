# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Solids import WavefrontLoader
from VecMath import *
import math

RAD = math.pi / 180.0

num_shot = 80
interval = 4

waittime = 30

vecList = []
WavefrontLoader.GetVertices("ico.obj", lambda v: vecList.append(+v))

ownerBone = EntityBone(world, "Alice", u"右手アクセ")
targetBone = EntityBone(world, "Reimu", u"センター")

def world_task(axis, angle, range, should_shot_scale):
	
	rootBone = EntityShot(world, "BONE", 0xFFFFFF)
	rootBone.RecordWhenVelocityChanges = False
	def record():
		rootBone.Pos = ownerBone.WorldPos
	rootBone.AddTask(record, 0, 300, 0)
	record()
	rootBone()
	
	for vec in vecList:
		
		if (vec ^ axis).Length() < 0.01: continue
		
		rotateAngle = 4
		rotateQuat = Quaternion.RotationAxis(vec ^ (vec ^ axis), angle * rotateAngle)
		
		root = EntityShot(world, "BONE", 0xFFFFFF)
		root.ParentEntity = rootBone
		root.RecordWhenVelocityChanges = False
		root.Rot =  rotateQuat ^ (90 / rotateAngle)
		
		def rotate_root(root = root, rotateQuat = rotateQuat): root.Rot *= rotateQuat
		root.AddTask(rotate_root, interval, 0, 0)
		
		root()
		
		parent = EntityShot(world, "MAG_CIR", 0xFFFFFF)
		parent.RecordWhenVelocityChanges = False
		
		timewait = 60
		
		parent.Velocity = vec * (range / timewait)
		parent.SetMotionInterpolationCurve(Vector2(0.9, 0.1), Vector2(0.9, 0.1), timewait)
		def stop(parent = parent): parent.Velocity *= 0
		parent.AddTask(stop, 0, 1, timewait)
		
		if parent.ModelDataIsOperable:
			for vert in parent.ModelData.Vertices: vert.Pos *= 2
		
		morph = parent.CreateVertexMorph("V_" + parent.MaterialMorph.MorphName, lambda v: -v)
		parent.AddVmdMorph(0, 1, morph)
		parent.AddVmdMorph(timewait, 0, morph)
		
		parent.Rot = Quaternion.RotationAxis(Vector3.UnitZ ^ vec, math.acos(vec.z))
		parent.ParentEntity = root
		
		rotateQuat = rotateQuat ^ 2
		
		def shot_dia(task, parent = parent, rotateQuat = rotateQuat):
			
			parent.Rot *= rotateQuat
			
			shot = EntityShot(world, "DIA", 0xFFA0FF)
			
			shot.Pos = parent.WorldPos
			shot.Velocity = Vector3.UnitZ * parent.WorldMat * -1.2
			shot.LivingLimit = 300
			shot()
			
			if task.RunCount % 2 == 0:
				def reverse(shot = shot, parent = parent):
					shot.Velocity *= -6
				shot.AddTask(reverse, 0, 1, 110)
			
		parent.AddTask(shot_dia, interval, int(300 / interval), timewait, True)
		
		if should_shot_scale:
			def shot_scale(task, root = root, parent = parent, mat = Matrix3.RotationAxis(Vector3.UnitY, RAD * 90)):
				
				target = targetBone.WorldPos
				
				vec = +(target - root.WorldPos) * mat
				
				targetPosList = [target - vec * 20, target, target + vec * 20]
				
				for targetPos in targetPosList:
					shot = EntityShot(world, "SCALE", 0xA00000 if task.RunCount % 2 == 0 else 0xA000A0)
					shot.Velocity = +(targetPos - parent.WorldPos) * 8
					shot.Upward = Vector3.UnitY * parent.WorldMat
					shot.Pos = parent.WorldPos
					shot.LivingLimit = 50
					shot()
			parent.AddTask(shot_scale, interval, int(80 / interval), 250, True)
		parent()
world.AddTask(lambda: world_task(Vector3.UnitX, RAD, 160, True), 0, 1, 0)
world.AddTask(lambda: world_task(Vector3.UnitX, -RAD, 160, True), 0, 1, 0)
world.AddTask(lambda: world_task(Vector3.UnitZ, RAD, 160, False), 0, 1, 0)
world.AddTask(lambda: world_task(Vector3.UnitZ, -RAD, 160, False), 0, 1, 0)