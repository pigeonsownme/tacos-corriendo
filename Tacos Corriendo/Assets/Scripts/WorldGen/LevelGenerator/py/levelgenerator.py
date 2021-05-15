from random import randint, choice
import json


# Represents one in-game chunk
class Chunk:
    def __init__(self, x, y, contents=None):
        self.x = x
        self.y = y
        self.contents = contents
        # ^ In case we decide to generate more than just roads with this

    # Custom '==' operator to allow use of the 'in' keyword
    def __eq__(self, other):
        return (
            self.x == other.x and self.y == other.y and self.contents == other.contents
        )

    def __hash__(self):
        n1 = self.x
        n2 = 1000 * self.y
        n3 = 1000000 * ord(self.contents[0]) if self.contents is not None else 0
        return n1 + n2 + n3

    def ToDict(self):
        return {"x": self.x, "y": self.y, "contents": self.contents}


# Chunk but with 'connections' property (useful for generating roads)
class Fork(Chunk):
    def __init__(self, x, y):
        super().__init__(x, y, "fork")
        self.connections = []


class Road(Chunk):
    def __init__(self, x, y, model=None):
        super().__init__(x, y, "road")
        self.model = model

    def ToDict(self):
        output = super().ToDict()
        # TODO
        output.update({"model": self.model})
        return output


# A Vector object is just one straight orthogonal line
class Vector:
    def __init__(self, x1, y1, x2, y2):
        self.x1 = x1
        self.y1 = y1
        self.x2 = x2
        self.y2 = y2

    # Converts a Vector object into Chunk objects with contents='road'
    def ToChunks(self):
        x1, x2 = sorted([self.x1, self.x2])
        y1, y2 = sorted([self.y1, self.y2])

        chunks = []
        for x in range(x1, x2 + 1):
            for y in range(y1, y2 + 1):
                chunks.append(Road(x, y))

        return chunks

    # Custom __eq__ (called when testing with '==') to be able to use the built-in 'in' keyword
    def __eq__(self, other):
        return (
            self.x1 == other.x1
            and self.y1 == other.y1
            and self.x2 == other.x2
            and self.y2 == other.y2
        )

    # Generates 2 Vector objects to connect 2 Fork objects
    @staticmethod
    def Connect(forkA, forkB, first_axis=None):
        # The first road's axis is random by default, but can be manually set
        if first_axis is None:
            first_axis = choice(["h", "v"])  # 'h' --> horizontal    'v' --> vertical

        corner = [forkB.x, forkA.y] if first_axis == "h" else [forkA.x, forkB.y]

        road1 = Vector(forkA.x, forkA.y, *corner)
        road2 = Vector(*corner, forkB.x, forkB.y)

        forkA.connections.append(forkB)
        forkB.connections.append(forkA)

        return [road1, road2]


class Level:
    def __init__(self, width, height):
        self.width = width
        self.height = height
        self.chunks = {"roads": [], "buildings": []}
        self.forks = []
        self.roads = []

    # Generates road fork/intersection locations with Fork objects
    def GenerateForks(
        self, min_forks, max_forks, min_distance, max_per_row=None, max_per_column=None
    ):
        # Max number of forks per line
        if max_per_row is None and max_per_column is None:
            max_per_row = self.width // (min_distance * 2)
            max_per_column = self.height // (min_distance * 2)

        # The number of forks varies randomly in [min_forks;max_forks]
        n_forks = randint(min_forks, max_forks)
        # Keeps trying to place Fork objects in valid locations until the target number of forks is
        # met or until the number of attempts exceeds a constant threshold
        max_attempts = 50
        attempts = 0
        while len(self.forks) < n_forks:
            x, y = [randint(0, self.width - 1), randint(0, self.height - 1)]
            new_fork = Fork(x, y)
            attempts += 1

            if attempts > max_attempts:
                break

            if new_fork in self.forks:
                continue
            if len([fork for fork in self.forks if fork.x == new_fork.x]) > max_per_row:
                continue
            if (
                len([fork for fork in self.forks if fork.y == new_fork.y])
                > max_per_column
            ):
                continue
            if [
                fork
                for fork in self.forks
                if (fork.x - new_fork.x) ** 2 + (fork.y - new_fork.y) ** 2
                < min_distance ** 2
            ]:
                continue

            self.forks.append(new_fork)
            attempts = 0

    # Connects each Fork object to one other random Fork object with Vector objects, then converts the
    # Vector objects into Chunk objects
    def GenerateRoads(self):
        for forkA in self.forks:
            # So the Fork object is not connected to itself or a Fork object it is already connected to
            possible_connections = self.forks.copy()
            for fork in forkA.connections + [forkA]:
                possible_connections.remove(fork)

            forkB = choice(possible_connections)
            self.roads += Vector.Connect(forkA, forkB)

        for road in self.roads:
            self.chunks["roads"] += road.ToChunks()

    # Some Fork objects will end up being dead ends
    def FixDeadEnds(self):
        dead_ends = []
        # Checks for dead ends
        for fork in self.forks:
            axis_to_connect = ""
            neighbors = 0

            if Road(fork.x - 1, fork.y) in self.chunks["roads"]:
                neighbors += 1
                axis_to_connect = "v"
            if Road(fork.x + 1, fork.y) in self.chunks["roads"]:
                neighbors += 1
                axis_to_connect = "v"
            if Road(fork.x, fork.y - 1) in self.chunks["roads"]:
                neighbors += 1
                axis_to_connect = "h"
            if Road(fork.x, fork.y + 1) in self.chunks["roads"]:
                neighbors += 1
                axis_to_connect = "h"

            if neighbors == 1:
                dead_ends.append([fork, axis_to_connect])

        # Fix dead ends with new roads, the same as Level.GenerateRoads(), but taking into account
        # which axis to generate the roads in
        new_roads = []
        for dead_end, axis_to_connect in dead_ends:
            possible_connections = self.forks.copy()
            for fork in dead_end.connections + [dead_end]:
                possible_connections.remove(fork)

            forkB = choice(possible_connections)
            new_roads += Vector.Connect(dead_end, forkB, axis_to_connect)

        self.roads += new_roads

        for road in new_roads:
            self.chunks["roads"] += road.ToChunks()

    # Converts Vector objects into Chunk objects
    def ConvertRoadTypes(self):
        unique_roads = set(self.chunks["roads"])
        self.chunks["roads"].clear()

        for road in unique_roads:
            neighbors = ""

            if Road(road.x, road.y + 1) in unique_roads:
                neighbors += "d"
            if Road(road.x - 1, road.y) in unique_roads:
                neighbors += "l"
            if Road(road.x + 1, road.y) in unique_roads:
                neighbors += "r"
            if Road(road.x, road.y - 1) in unique_roads:
                neighbors += "u"

            # Order : dlru
            converter = {
                "u": "│",
                "d": "│",
                "l": "─",
                "r": "─",
                "du": "│",
                "lr": "─",
                "dlr": "┬",
                "dlu": "┤",
                "dru": "├",
                "lru": "┴",
                "ru": "└",
                "lu": "┘",
                "dr": "┌",
                "dl": "┐",
                "dlru": "┼",
            }

            road.model = converter[neighbors]
            self.chunks["roads"].append(road)

    # Logs the level into the console
    def Print(self):
        output = [[" " for j in range(self.width)] for i in range(self.height)]

        for chunk in self.chunks["roads"] + self.chunks["buildings"]:
            output[chunk.y][chunk.x] = chunk.model

        for fork in self.forks:
            output[fork.y][fork.x] = "0"

        for line in output:
            print("\t", *line, sep="")

    def ToJSON(self, destination):
        dict_chunks = list(map(Chunk.ToDict, self.chunks["roads"]))
        json.dump(dict_chunks, destination, indent=4)

    def ToTXT(self):
        lines = [list("0") * self.width for i in range(self.height)]

        converter = {
            "│": "1",
            "─": "2",
            "┬": "3",
            "┤": "4",
            "├": "5",
            "┴": "6",
            "└": "7",
            "┘": "8",
            "┌": "9",
            "┐": "A",
            "┼": "B",
        }

        for chunk in self.chunks["roads"]:
            lines[chunk.y][chunk.x] = converter[chunk.model]

        output = ""
        for line in lines:
            output += "".join(line) + "\n"

        return output
