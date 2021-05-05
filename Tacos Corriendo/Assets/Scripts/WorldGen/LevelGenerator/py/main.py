from levelgenerator import Level
import sys

args = [int(arg) for arg in sys.argv[1:]]

level = Level(width=args[0], height=args[1])
level.GenerateForks(min_forks=args[2], max_forks=args[3], min_distance=args[4])
level.GenerateRoads()
level.FixDeadEnds()

with open("level.json", "w") as f:
    level.ToJSON(f)
