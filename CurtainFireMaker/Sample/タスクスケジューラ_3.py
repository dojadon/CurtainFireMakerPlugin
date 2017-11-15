# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import EntityShot
from CurtainFireMakerPlugin.Solids import WavefrontLoader

#発射する弾のベクトルのリストをobjファイルから取得
veclist = []
WavefrontLoader.GetVertices("ico.obj", lambda v: veclist .append(v))

def shot_task():
    for vec in veclist :
        #弾のインスタンス生成
        shot = EntityShot(world , "S", 0xFF0000)
    	
        #弾の移動量設定
        shot.Velocity = vec * 2.0
        
        #移動量を回転させる関数
        def rotate(shot = shot, matrix = Matrix3.RotationAxis(vec ^ (vec ^ Vector3.UnitY), RAD * 80)):
            shot.Velocity *= matrix
        #rotateを60フレーム後に一回実行する
        shot.AddTask(rotate, 0, 1, 60)
    	
        #弾を配置
        shot()
#タスクを設定、引数は以下の通り
#実行する関数、実行する間隔、実行する回数、実行するまでの待機時間
#ここではshot_taskを15フレーム毎に10回実行する
world.AddTask(shot_task, 15, 10, 0)