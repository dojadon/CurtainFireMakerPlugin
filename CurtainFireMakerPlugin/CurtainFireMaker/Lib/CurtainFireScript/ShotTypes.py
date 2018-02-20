# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin import Configuration
from CurtainFireMakerPlugin.Entities import *
from MMDataIO.Pmx import BoneFlags, PmxBoneData
from VecMath import *
from System import Array, String, Int32

import clr
clr.AddReferenceByPartialName("System.Drawing")
from System.Drawing import Color, Bitmap

RAD = 3.1415926535 / 180.0

class ShotTypeBone(ShotType):
	def CreateBones(self, world, prop):
		return Array[PmxBoneData]([PmxBoneData()])

	def CreateMaterials(self, world, prop):
		return Array[PmxMaterialData]([])

	def CreateTextures(self, world, prop):
		return Array[String]([])

	def CreateVertexIndices(self, world, prop):
		return Array[Int32]([])

	def CreateVertices(self, world, prop):
		return Array[PmxVertexData]([])

	def CreateVertexMorph(self, world, prop):
		return None

class ShotTypeStar(ShotTypePmx):
	def __init__(self, name, path, scale):
		self.interval = 180
		self.parent_dict = {}

	def InitWorld(self, world):
		self.parent_dict.clear()

	def InitEntity(self, entity):
		frame = entity.World.FrameCount % self.interval

		if frame not in self.parent_dict:
			parent = EntityShot(entity.World, "BONE", 0)
			parent.GetRecordedRot = lambda e: e.Rot

			def rotate(rot = Quaternion.RotationAxis(Vector3.UnitZ, RAD * 179)): parent.Rot *= rot
			parent.AddTask(rotate, 180, 0, 0)
			parent()
			self.parent_dict[frame] = parent
		parent = self.parent_dict[frame]
		entity.RootBone.LinkParentId = parent.RootBone.BoneId
		entity.RootBone.LinkWeight = 1.0
		entity.RootBone.Flag |= BoneFlags.LOCAL_LINK | BoneFlags.ROTATE_LINK

class ShotTypeButterfly(ShotTypePmx):
	def __init__(self, name, path, scale):
		self.interval = 60
		self.parent_dict = {}

	def InitWorld(self, world):
		self.parent_dict.clear()

	def InitEntity(self, entity):
		frame = entity.World.FrameCount % self.interval

		if frame not in self.parent_dict:
			parent = EntityShot(entity.World, "BONE", 0)
			parent.GetRecordedRot = lambda e: e.Rot

			curve = CubicBezierCurve(Vector2(0, 0), Vector2(0.7, 0.3), Vector2(0.3, 0.7), Vector2(1, 1))
			rot = Quaternion.RotationAxis(Vector3.UnitZ, RAD * 45)

			def shot_task_func(task):
				flag = task.ExecutedCount % 2 == 0
				parent.AddBoneKeyFrame(parent.RootBone, Vector3.Zero, rot if flag else ~rot, curve, 0)
			parent.AddTask(shot_task_func, self.interval, 0, 0, True)
			parent()
			self.parent_dict[frame] = parent
		parent = self.parent_dict[frame]

		def setup(bone, weight):
			bone.LinkParentId = parent.RootBone.BoneId
			bone.LinkWeight = weight
			bone.Flag |= BoneFlags.LOCAL_LINK | BoneFlags.ROTATE_LINK
		setup(entity.ModelData.Bones[1], 1.0)
		setup(entity.ModelData.Bones[2], -1.0)

class ShotTypeKnife(ShotTypePmx):
	def InitModelData(self, prop, materials):
		materials[1].Diffuse = Vector4(prop.Red, prop.Green, prop.Blue, 1.0)
		materials[1].Ambient = Vector3(prop.Red, prop.Green, prop.Blue)

class ShotTypeBillboard(ShotTypePmx):
	def InitEntity(self, entity):
		entity.GetRecordedRot = lambda e: Quaternion.Identity

class ShotTypeL(ShotTypeBillboard):
	def __init__(self, name, path, scale):
		self.image_dict = {}

	def InitModelData(self, prop, materials):
		pass

	def InitWorld(self, world):
		self.image_dict.clear()

	def CreateTextures(self, world, prop):
		texture = ShotTypePmx.CreateTextures(self, world, prop)[0].replace('/', '\\')
		colorTexture = ShotTypeL.AppendFileName(self, texture, hex(prop.Color))

		if prop.Color not in self.image_dict:
			image = Bitmap(Configuration.ResourceDirPath + texture)
			ShotTypeL.SetPxcelColor(self, image, prop.Red, prop.Green, prop.Blue)
			self.image_dict[prop.Color] = image

			def export(world, arg):
				exportPath = world.Config.PmxExportDirPath + "\\" + colorTexture
				image.Save(exportPath)
				image.Dispose()
			world.ExportEvent += export
		return Array[str]([colorTexture])

	def AppendFileName(self, path, str):
		index = path.rfind('.')
		return path[0:index] + str + path[index:]

	def SetPxcelColor(self, image, r, g, b):
		color = Vector3(r, g, b)
		sub = Vector3(1, 1, 1) - color

		boder1 = 0.5
		boder2 = 0.85
		boder_mult = 1 / (boder2 - boder1)

		for x in range(image.Width):
			for y in range(image.Height):
				src = image.GetPixel(x, y)
				sub_scale = 1

				pos = Vector2(float(x) / image.Width - 0.5, float(y) / image.Width - 0.5) * 2
				dis = pos.Length()

				if dis < boder1:
					sub_scale = 0
				elif boder1 <= dis and dis < boder2:
					sub_scale = (dis - boder1) * boder_mult

				image.SetPixel(x, y, ShotTypeL.ColorScale(self, image.GetPixel(x, y), color + sub * sub_scale))
	def ColorScale(self, src, scale):
		return Color.FromArgb(src.A, int(src.R * scale.x), int(src.G * scale.y), int(src.R * scale.z))
