# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.ShotTypes import *
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Mathematics import *
from CsMmdDataIO.Pmx.Data import BoneFlags
from VecMath import *
import math

RAD = math.pi / 180.0

class ShotTypeStar(ShotTypePmx):
	def __new__(cls, name, path, scale):
		return ShotTypePmx.__new__(cls, name, path, scale)
	
	def __init__(self, name, path, scale):
		pass
	
	def InitWorld(self, world):
		parent = EntityShot(world, "BONE", 0)
		parent.ShouldRecord = EntityShot.RecordType.LocalMat
		
		def rotate(rot = Quaternion.RotationAxis(Vector3.UnitZ, RAD * 179)): parent.Rot *= rot
		parent.AddTask(rotate, 179, 0, 0)
		parent()
		
		self.parent = parent
		
	def InitEntity(self, entity):
		entity.RootBone.LinkParentId = self.parent.RootBone.BoneId
		entity.RootBone.LinkWeight = 1.0
		entity.RootBone.Flag |= BoneFlags.LOCAL_LINK | BoneFlags.ROTATE_LINK

class ShotTypeButterfly(ShotTypePmx):
	def __new__(cls, name, path, scale):
		return ShotTypePmx.__new__(cls, name, path, scale)
	
	def __init__(self, name, path, scale):
		pass
	
	def InitWorld(self, world):
		parent = EntityShot(world, "BONE", 0)
		parent.ShouldRecord = EntityShot.RecordType.None
		
		curve = CubicBezierCurve(Vector2(0, 0), Vector2(0.8, 0.2), Vector2(0.2, 0.8), Vector2(1, 1))
		rot = Quaternion.RotationAxis(Vector3.UnitZ, math.pi / 4)
		
		def shot_task_func(task):
			flag = task.RunCount % 2 == 0
			parent.AddBoneKeyFrame(parent.RootBone, Vector3.Zero, rot if flag else ~rot, curve)
		parent.AddTask(shot_task_func, 60, 0, 0, True)
		parent()
		
		self.parent = parent
		
	def InitEntity(self, entity):
		def setup(bone, weight):
			bone.LinkParentId = self.parent.RootBone.BoneId
			bone.LinkWeight = weight
			bone.Flag |= BoneFlags.LOCAL_LINK | BoneFlags.ROTATE_LINK
		setup(entity.ModelData.Bones[1], 1.0)
		setup(entity.ModelData.Bones[2], -1.0)

class ShotTypeKnife(ShotTypePmx):
	def __new__(cls, name, path, scale):
		return ShotTypePmx.__new__(cls, name, path, scale)
	
	def __init__(self, name, path, scale):
		pass
	
	def InitModelData(data):
		prop = data.Property
		data.Materials[1].Diffuse = Vector4(prop.Red, prop.Green, prop.Blue, 1.0)
		data.Materials[1].Ambient = Vector3(prop.Red, prop.Green, prop.Blue)