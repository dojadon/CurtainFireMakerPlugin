from VecMath import *
from System import Random
from random import random, gauss

rand = Random()

def randomvec(): return +Vector3(rand.NextDouble() - 0.5, rand.NextDouble() - 0.5, rand.NextDouble() - 0.5)

def vec3(vec4): return Vector3(vec4.x, vec4.y, vec4.z)

def vec4(vec3): return Vector4(vec3.x, vec3.y, vec3.z, 1)

def quat(mat): return Quaternion(mat)

def mat3(quat): return Matrix3(quat)

def mat4(quat): return Matrix4(quat)
