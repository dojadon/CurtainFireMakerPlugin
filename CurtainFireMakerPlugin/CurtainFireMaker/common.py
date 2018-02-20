# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.Entities import *
from CurtainFireScript.Entities import *
from CurtainFireScript.VectorUtil import *
from VecMath import *

WORLD.MaxFrame = 1200
WORLD.FrameCount = 0

OWNER_BONE = Entity(WORLD)
OWNER_BONE()

TARGET_BONE = Entity(WORLD)
TARGET_BONE.Pos = Vector3(0, 0, -200)
TARGET_BONE()
