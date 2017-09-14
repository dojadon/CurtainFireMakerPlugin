# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from CurtainFireMakerPlugin.Solids import WavefrontLoader
from VecMath import *
import math
import vectorutil as vutil

RAD = math.pi / 180.0

world.StartFrame = -50

distance_circle = 512.0

def task(veclist, axislist, propfunc, speed1, speed2, rot_vec, angle_vec, rot_pos, angle_pos, int_shot, num_shot, int_task, num_task, pause_frame_func, restart_frame_func, restart_frame):
	
	total_num = num_shot * num_task
	total_num_mult = 1.0 / total_num
	
	frame_num = distance_circle / speed1
	
	for vec in veclist:
		
		for axis in axislist:
			
			if abs(vec.y) > 0.99:
				continue
			
			prop = propfunc(vec, axis)
			
			axis = vec ^ (vec ^ axis)
			
			parent = EntityShot(world, "BONE", 0xFFFFFF)
			parent.RecordWhenVelocityChanges = False
			parent.Rot = Quaternion.RotationAxis(axis, rot_pos)
			parent()
			
			circle = EntityShot(world, "MAG_CIR", 0xFFFFFF)
			circle.Pos = vec * distance_circle
			circle.Rot = Matrix3.LookAt(vec, Vector3.UnitY)
			circle.ParentEntity = parent
			circle.RecordWhenVelocityChanges = False
			
			for vert in circle.ModelData.Vertices:
				vert.Pos = Vector3(vert.Pos.x * 3, vert.Pos.y * 3, vert.Pos.z * 3)
				
			circle()
			
			entity = Entity(world)
			entity.Rot = Quaternion.RotationAxis(axis, rot_vec)
			entity()
			
			rotate_pos = Quaternion.RotationAxis(axis, angle_pos)
			rotate_vec = Quaternion.RotationAxis(axis, angle_vec)
			
			def add_task(task1, axis = axis, entity = entity, circle = circle, prop = prop, rotate_pos = rotate_pos, rotate_vec = rotate_vec):
				
				def shot_amulet(task2):
					
					count = (task1.RunCount - 1) * num_shot + task2.RunCount - 1
					
					shot = EntityShot(world, prop)
					shot.Pos = circle.WorldPos
					shot.Velocity = +circle.WorldPos * entity.Rot * -speed1
					shot.Upward = axis
					shot.DiedDecision = lambda e: (e.Pos - parent.Pos).Length > 1024 
					shot()
					
					def pause():
						shot.Velocity *= 0
						shot.RecordWhenVelocityChanges = False
					shot.AddTask(pause, 0, 1, int(frame_num * pause_frame_func(count * total_num_mult)))
					
					def restart(vec = +shot.Velocity):
						shot.RecordWhenVelocityChanges = True
						shot.Velocity = vec * speed2
					shot.AddTask(restart, 0, 1, int(restart_frame * restart_frame_func(count * total_num_mult)))
					
				entity.AddTask(shot_amulet, int_shot, num_shot, 0, True)
			entity.AddTask(add_task, int_task, num_task, 0, True)
			
			def rotate(entity = entity, circle = circle, parent = parent, prop = prop, rotate_pos = rotate_pos, rotate_vec = rotate_vec):
				parent.Rot = +parent.Rot * rotate_pos
				entity.Rot = +entity.Rot * rotate_vec
				
			entity.AddTask(rotate, int_shot, world.MaxFrame / int_shot, 1)
veclist = []
WavefrontLoader.GetVertices("ico.obj", lambda v: veclist.append(v))

axislist = [Vector3.UnitX, Vector3.UnitZ]

propfunc = lambda v, a: ShotProperty("AMULET", 0xA00000 if a.x > 0.99 else 0x0000A0)

speed1 = 4.0
speed2 = 6.0

rot_vec = RAD * -10
angle_vec = RAD * 0.1

rot_pos = RAD * 0
angle_pos= RAD * 3.0

int_shot = 2
num_shot = 50

int_task = 110
num_task = 2

pause_frame_func = lambda t: 0.2 + t * 1.2
restart_frame_func = lambda t: 1 - t * 0.4

restart_frame = 320

world.AddTask(lambda: task(veclist, axislist, propfunc, speed1, speed2, rot_vec, angle_vec, rot_pos, angle_pos, int_shot, num_shot, int_task, num_task, pause_frame_func, restart_frame_func, restart_frame), 0, 1, 0)
