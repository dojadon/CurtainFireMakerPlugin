# -*- coding: utf-8 -*-

veclist = objvertices("ico.obj", 1)

def task():
	for vec in veclist:
		#弾生成
		shot = EntityShot(WORLD, "DIA", 0x0000FF)

		shot.Velocity = vec * 2.0

		shot.LivingLimit = 300

		#弾配置
		shot()
WORLD.AddTask(task, 30, 1, 0)
