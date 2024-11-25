using AoCTools;

const string inputFile = @"../../../../input10.txt";

Console.WriteLine("Day 10 - Pipe Maze");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);

Point2D startingPoint = FindStartingPoint();

Dictionary<Point2D, int> distanceMap = new Dictionary<Point2D, int>();
distanceMap[startingPoint] = 0;

Queue<Point2D> pipesToCheck = new Queue<Point2D>();
pipesToCheck.Enqueue(startingPoint);

while (pipesToCheck.Count > 0)
{
    Point2D checkingPipe = pipesToCheck.Dequeue();
    int newDistance = distanceMap[checkingPipe] + 1;

    for (Direction direction = 0; direction < Direction.MAX; direction++)
    {
        Point2D newPosition = ShiftPosition(checkingPipe, direction);
        if (CanTravel(checkingPipe, direction) && (!distanceMap.ContainsKey(newPosition) || distanceMap[newPosition] > newDistance))
        {
            distanceMap[newPosition] = newDistance;
            pipesToCheck.Enqueue(newPosition);
        }
    }
}


Console.WriteLine($"The answer is: {distanceMap.Values.Max()}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();


Point2D UpperLeft = (distanceMap.Keys.Min(x => x.x), distanceMap.Keys.Min(x => x.y));
Point2D BottomRight = (distanceMap.Keys.Max(x => x.x), distanceMap.Keys.Max(x => x.y));


Dictionary<Point2D, char> printOutput = new Dictionary<Point2D, char>();

foreach (Point2D pipe in distanceMap.Keys)
{
    printOutput[pipe - UpperLeft] = lines[pipe.y][pipe.x];
}


int enclosedCount = 0;

for (int x = UpperLeft.x + 1; x < BottomRight.x; x++)
{
    for (int y = UpperLeft.y + 1; y < BottomRight.y; y++)
    {
        if (distanceMap.ContainsKey((x, y))) continue;

        //Found an empty spot

        int pipeCount = 0;
        char lastChar = '\0';
        for (int checkX = UpperLeft.x; checkX < x; checkX++)
        {
            if (distanceMap.ContainsKey((checkX, y)))
            {
                switch (lines[y][checkX])
                {
                    case '-': break;

                    case '|':
                        pipeCount++;
                        break;

                    case 'L':
                    case 'F':
                        lastChar = lines[y][checkX];
                        break;

                    case 'J':
                        if (lastChar == 'F') pipeCount++;
                        break;

                    case '7':
                        if (lastChar == 'L') pipeCount++;
                        break;
                }
            }
        }

        if (pipeCount % 2 == 0) continue;

        pipeCount = 0;
        for (int checkY = UpperLeft.y; checkY < y; checkY++)
        {
            if (distanceMap.ContainsKey((x, checkY)))
            {

                switch (lines[checkY][x])
                {
                    case '|': break;

                    case '-':
                        pipeCount++;
                        break;

                    case '7':
                    case 'F':
                        lastChar = lines[checkY][x];
                        break;

                    case 'J':
                        if (lastChar == 'F') pipeCount++;
                        break;

                    case 'L':
                        if (lastChar == '7') pipeCount++;
                        break;
                }
            }
        }

        if (pipeCount % 2 == 0) continue;

        enclosedCount++;
        printOutput[(x, y) - UpperLeft] = 'I';
    }
}

Console.WriteLine($"The answer is: {enclosedCount}");
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Grid:");
Console.WriteLine();
Console.ReadKey();
Console.WriteLine();


for (int y = 0; y < BottomRight.y - UpperLeft.y + 1; y++)
{
    for (int x = 0; x < BottomRight.x - UpperLeft.x + 1; x++)
    {
        if (printOutput.ContainsKey((x, y)))
        {
            if (printOutput[(x, y)] == 'I')
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.Write(' ');
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else
            {
                Console.Write(printOutput[(x, y)]);
            }
        }
        else
        {
            Console.Write(' ');
        }
    }
    Console.WriteLine();
}



Console.WriteLine();


Console.WriteLine();
Console.ReadKey();


bool CanTravel(Point2D position, Direction direction)
{
    Point2D newPosition = ShiftPosition(position, direction);

    if (newPosition.y < 0 || newPosition.y >= lines.Length) return false;
    if (newPosition.x < 0 || newPosition.x >= lines[newPosition.y].Length) return false;

    char current = lines[position.y][position.x];
    char newPipe = lines[newPosition.y][newPosition.x];

    return PipeSupportsDirection(current, direction) && PipeSupportsDirection(newPipe, direction.Invert());
}

Point2D ShiftPosition(Point2D position, Direction direction) => direction switch
{
    Direction.Up => position + (0, -1),
    Direction.Down => position + (0, 1),
    Direction.Left => position + (-1, 0),
    Direction.Right => position + (1, 0),
    _ => throw new Exception(),
};

bool PipeSupportsDirection(char pipe, Direction direction)
{
    switch (direction)
    {
        case Direction.Up:
            return pipe switch
            {
                'L' => true,
                'J' => true,
                '|' => true,
                'S' => true,
                _ => false
            };
        case Direction.Down:
            return pipe switch
            {
                '7' => true,
                'F' => true,
                '|' => true,
                'S' => true,
                _ => false
            };
        case Direction.Left:
            return pipe switch
            {
                '7' => true,
                'J' => true,
                '-' => true,
                'S' => true,
                _ => false
            };
        case Direction.Right:
            return pipe switch
            {
                'L' => true,
                'F' => true,
                '-' => true,
                'S' => true,
                _ => false
            };
        default:
            throw new Exception();
    }
}

Point2D FindStartingPoint()
{
    for (int y = 0; y < lines.Length; y++)
    {
        for (int x = 0; x < lines[y].Length; x++)
        {
            if (lines[y][x] == 'S')
            {
                return new Point2D(x, y);
            }
        }
    }

    throw new Exception();
}

public enum Direction
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3,
    MAX
}


public static class ExtensionMethods
{
    public static Direction Invert(this Direction direction) => direction switch
    {
        Direction.Up => Direction.Down,
        Direction.Left => Direction.Right,
        Direction.Right => Direction.Left,
        Direction.Down => Direction.Up,
        _ => throw new Exception()
    };
}