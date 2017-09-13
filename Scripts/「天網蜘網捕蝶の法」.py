from CurtainFireMakerPlugin.Entities import *
from VecMath import *
import math

RAD = math.pi / 180.0
way = 3
num_divide = 5

shotDict = {}
for i in range(num_divide):
	shotDict[i] = []

def shot_func(pos, vec, level):
	shot = EntityShot(world, "M", 0x0000A0)
	shot.Pos = pos
	shot.Velocity = vec * 8
	shot.LivingLimit = 100
	shot()

	laser = EntityShot(world, "LASER", 0x0000A0)
	if laser.ModelData.OwnerEntity.Count == 1:
		for vert in laser.ModelData.Vertices:
			vertPos = vert.Pos
			vert.Pos = Vector3(vertPos.x * 4, vertPos.y * 4, vertPos.z * 400)

	laser.Pos = pos
	laser.RecordWhenVelocityChanges = False
	laser.Rot = Matrix3.LookAt(vec, Vector3.UnitY)

	morph = laser.CreateVertexMorph(lambda v: Vector3(-v.x * 0.9, -v.y * 0.9, 0))
	laser.AddVmdMorph(0, 1, morph)
	laser.AddVmdMorph(29, 1, morph)
	laser.AddVmdMorph(30, 0, morph)
	laser.AddVmdMorph(80, 0, morph)

	laser.LivingLimit = 80
	laser()

	if level < num_divide:

		def shot_task_func():

			mat1 = Matrix3.RotationAxis(+(vec ^ Vector3.UnitY), RAD * 60)
			mat2 = Matrix3.RotationAxis(vec, RAD * (360 / way))

			shotVec = vec * mat1

			for i in range(way):
				shot_func(shot.Pos, +shotVec, level + 1)
				shotVec = shotVec * mat2
		shot.AddTask(shot_task_func, 0, 1, 6)
		shot.LivingLimit = 7
shot_func(Vector3.Zero, -Vector3.UnitZ, 0)
