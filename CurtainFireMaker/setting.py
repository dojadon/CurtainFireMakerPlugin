# -*- coding: utf-8 -*-
from CurtainFireMakerPlugin.ShotTypes import *
from shottypes import *
import math

ShotType.RegisterType(ShotTypePmx("S", "shot.pmx", 6.0))
ShotType.RegisterType(ShotTypePmx("M", "shot.pmx", 10.0))
ShotType.RegisterType(ShotTypePmx("DIA", "dia.pmx", 8))
ShotType.RegisterType(ShotTypePmx("BULLET", "bullet.pmx", 5))
ShotType.RegisterType(ShotTypePmx("SCALE", "scale.pmx", 6.0))
ShotType.RegisterType(ShotTypePmx("LASER", "laser_curve.pmx", 1.0))
ShotType.RegisterType(ShotTypePmx("LASER_LINE", "laser_line.pmx", 1.0))
ShotType.RegisterType(ShotTypePmx("AMULET", "amulet.pmx", 4.0))
ShotType.RegisterType(ShotTypePmx("MAGIC_CIRCLE", "magic_circle.pmx", 10.0))

ShotType.RegisterType(ShotTypePmxL("L", "shot_l.pmx", 6.0))
ShotType.RegisterType(ShotTypeStar("STAR_S", "star.pmx", 8.0))
ShotType.RegisterType(ShotTypeStar("STAR_M", "star.pmx", 20.0))
ShotType.RegisterType(ShotTypeButterfly("BUTTERFLY", "butterfly.pmx", 2.0))
ShotType.RegisterType(ShotTypeKnife("KNIFE", "knife.pmx", 1))
