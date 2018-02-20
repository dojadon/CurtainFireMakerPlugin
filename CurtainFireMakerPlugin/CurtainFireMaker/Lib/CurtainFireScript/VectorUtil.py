from VecMath import *
from CurtainFireMakerPlugin import Configuration
from System import Random

RAD = 3.1415926535 / 180.0

rand = Random()

def random(): return rand.NextDouble()

def uniform(min_value, max_value): return min_value + (max_value - min_value) * rand.NextDouble()

def randomvec(): return +Vector3(random() - 0.5, random() - 0.5, random() - 0.5)

def vec3(vec4): return Vector3(vec4.x, vec4.y, vec4.z)

def vec4(vec3): return Vector4(vec3.x, vec3.y, vec3.z, 1)

class Frame:
	def __init__(self, vec, rot):
		self.vec = vec
		self.rot = rot

class WavefrontObject:
	def __init__(self, path, function = lambda v: v):
		file = open(Configuration.ResourceDirPath + "\\Wavefront\\" + path)
		line = file.readline()

		self.veclist = []
		self.face_index_list = []
		self.side_index_list = []

		while line:
			splited = line.split()
			if splited[0] == 'v':
				self.veclist.append(function(Vector3(float(splited[1]), float(splited[2]), float(splited[3]))))
			elif splited[0] == 'f':
				face_index = [int(splited[i + 1].split('/')[0]) - 1 for i in range(len(splited) - 1)]
				self.face_index_list.append(face_index)
				self.side_index_list.extend([(face_index[i], face_index[(i + 1) % len(face_index)]) for i in range(len(face_index))])
    			line = file.readline()

    	def divide(self, level):
    		self.face_index_list = self.divide_faces(self.face_index_list, level)
    		return self

    	def divide_faces(self, facelist, level):
		if level <= 0:
			return facelist

		def get_index(vtx):
			if vtx in self.veclist:
				return self.veclist.index(vtx)
			else:
				self.veclist.append(vtx)
				return len(self.veclist) - 1

		divided_facelist = []
		for face in facelist:
			indices = [get_index((self.veclist[face[i]] + self.veclist[face[(i + 1) % 3]]) * 0.5) for i in range(3)]
			divided_facelist.append(indices)
			divided_facelist.extend([(face[i], indices[(i + 2) % 3], indices[i]) for i in range(3)])
		return self.divide_faces(divided_facelist, level - 1)

	def create_list_connected_vertex(self, vtx):
		idx = self.veclist.index(vtx)

		connected_vtxlist = [side[(1 if idx == side[0] else 0)] for side in self.side_index_list if idx in side]

		return [self.veclist[i] for i in set(connected_vtxlist)]

	def  bevel(self, function):
		for vertices in [[self.veclist[i] for i in face] for face in self.face_index_list]:
			for i in range(3):
				function(vertices[i], vertices[(i + 1) % 3])
				function(vertices[i], vertices[(i + 2) % 3])

def objvertices(path, num_divide = 0):
	return [+v for v in WavefrontObject(path).divide(num_divide).veclist]

def objlines(path):
	file = open(Configuration.ResourceDirPath + "\\Wavefront\\" + path)
	line = file.readline()

	veclist = []
	linelist = []

	while line:
		splited = line.split()
		if splited[0] == 'v':
			veclist.append(Vector3(float(splited[1]), float(splited[2]), float(splited[3])))
		elif splited[0] == 'l':
			linelist.append((veclist[int(splited[1]) - 1], veclist[int(splited[2]) - 1]))
    		line = file.readline()
    	return veclist, linelist
