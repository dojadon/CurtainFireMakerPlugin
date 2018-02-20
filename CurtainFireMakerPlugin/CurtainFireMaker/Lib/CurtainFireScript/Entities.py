# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from CurtainFireScript.VectorUtil import *
from MMDataIO.Pmx import PmxModelData
from VecMath import *
from System import Array
import DxMath

def CreateRigidObject(world, meshes, division_num, parent = None, p_min = None, p_max = None):
	if p_min == None or p_max == None:
		p_min = meshes[0].Pos1
		p_max = meshes[0].Pos1

		for mesh in meshes:
			for pos in mesh.Pos1, mesh.Pos2, mesh.Pos3:
				p_min = Vector3(min(p_min.x, pos.x), min(p_min.y, pos.y), min(p_min.z, pos.z))
				p_max = Vector3(max(p_max.x, pos.x), max(p_max.y, pos.y), max(p_max.z, pos.z))

	if division_num <= 0 or len(meshes) < 100:
		divided_meshes_list = []
	else:
		center = (p_min + p_max) * 0.5

		undividable_meshes = []
		divided_meshes_list = [[] for i in range(8)]

		def get_group_index(pos):
			return int(pos.x < center.x) + int(pos.y < center.y) * 2 + int(pos.z < center.z) * 4

		for mesh in meshes:
			indices = [get_group_index(pos) for pos in mesh.Pos1, mesh.Pos2, mesh.Pos3]
			if all(i == indices[0] for i in indices):
				divided_meshes_list[indices[0]].append(mesh)
			else:
				undividable_meshes.append(mesh)
		meshes = undividable_meshes

	rigid = StaticRigidObject(Array[MeshTriangle](meshes), AABoundingBox(p_min, p_max))

	if parent != None:
		parent.ChildRigidObjectList.Add(rigid)
	else:
		world.AddRigidObject(rigid)

	for idx, divided_meshes in enumerate(divided_meshes_list):
		if len(divided_meshes) > 0:
			dp_min = Vector3(p_min.x if idx & 0b001 else center.x, p_min.y if idx & 0b010 else center.y, p_min.z if idx & 0b100 else center.z)
			dp_max = Vector3(center.x if idx & 0b001 else p_max.x, center.y if idx & 0b010 else p_max.y, center.z if idx & 0b100 else p_max.z)
			CreateRigidObject(world, divided_meshes, division_num - 1, rigid, dp_min, dp_max)

def CreateRigidObjectByPmx(world, scene, scale, modelname, filepath, division_num):
	filtered = filter(lambda m: m.DisplayName == modelname, scene.Models)

	if len(filtered) == 0:
		print "Not found model : " + modelname
		return

	import DxMath
	pos = DxMath.Vector3.Zero
	rot = DxMath.Quaternion.Identity

	for layer in filtered[0].Bones.RootBone.Layers:
		data = layer.Frames.GetFrame(0)
		pos += data.Position
		rot *= data.Quaternion
	mat = Matrix4(rot, pos)

	data = PmxModelData()
	from System.IO import BinaryReader, FileStream, FileMode, FileAccess, FileShare
	data.Read(BinaryReader(FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)))

	poslist = [vec4(vert.Pos * scale) * mat for vert in data.VertexArray]

	meshes = [MeshTriangle(pos[0], pos[1], pos[2]) for pos in zip(*[iter([poslist[i] for i in data.VertexIndices])] * 3)]
	CreateRigidObject(world, meshes, division_num)

class EntityBone(Entity):
	def __new__(cls, world, scene, modelname, bonename, extra_parent = None):
		return Entity.__new__(cls, world)

	def __init__(self, world, scene, modelname, bonename, extra_parent = None):
		filtered = filter(lambda m: m.DisplayName == modelname, scene.Models)

		if len(filtered) == 0:
			print "Not found model : " + modelname
			return

		self.model = filtered[0]
		self.bone = self.model.Bones[bonename]
		if self.bone == None:
			print "Not found bone : " + modelname + ", " + bonename
			return

		self.init_pos = self.bone.InitialPosition

		if self.bone.ParentBoneID >= 0 and self.model.Bones.Count > self.bone.ParentBoneID:
			parentbone = self.model.Bones[self.bone.ParentBoneID]
			self.ParentEntity = EntityBone(world, scene, modelname, parentbone.Name, extra_parent)
			self.init_pos -= parentbone.InitialPosition
		elif extra_parent != None:
			self.ParentEntity = extra_parent
			#self.init_pos -= extra_parent.bone.InitialPosition

		self.OnSpawn()
		self.Frame()

	def DiedDecision(self, entity):
		return False

	def Frame(self):
		import DxMath
		pos = self.init_pos
		rot = DxMath.Quaternion.Identity

		for layer in self.bone.Layers:
			data = layer.Frames.GetFrame(max(self.World.FrameCount, 0))
			pos += data.Position
			rot *= data.Quaternion
		self.Pos = pos
		self.Rot = rot

		Entity.Frame(self)
