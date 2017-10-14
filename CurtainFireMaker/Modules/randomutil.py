from System import Random
import math

rand = Random()

def randint(min, max): rand.Next(min, max)

def random(): return rand.NextDouble()

def uniform(min, max): return min + (max - min) * random()

def gaussian(mu = 0, sigma = 1): return mu + sigma * math.sqrt(-2.0 * math.log(random())) * math.sin(2.0 * math.pi * random())
