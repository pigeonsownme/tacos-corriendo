from levelgenerator import Level
import sys
import os

args = [int(arg) for arg in sys.argv[1:]]

level = Level(width=args[0], height=args[1])
level.GenerateForks(
    min_forks=args[2],
    max_forks=args[3],
    min_distance=args[4],
    max_per_row=1,
    max_per_column=1,
)
level.GenerateRoads()
level.FixDeadEnds()
level.ConvertRoadTypes()
level.Print()

os.chdir(os.getcwd() + "\\Assets\\Scripts\\WorldGen\\LevelGenerator\\")

with open("level.txt", "w") as f:
    f.write(level.ToTXT())
