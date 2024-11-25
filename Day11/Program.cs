using AoCTools;

const string inputFile = @"../../../../input11.txt";

Console.WriteLine("Day 11 - Cosmic Expansion");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);

//Anticipating an expansion of the expansion rules, I'll parse and then expand.

HashSet<LongPoint2D> galaxies = new HashSet<LongPoint2D>();

for (int y = 0; y < lines.Length; y++)
{
    for (int x = 0; x < lines[y].Length; x++)
    {
        if (lines[y][x] == '#')
        {
            galaxies.Add((x, y));
        }
    }
}

HashSet<LongPoint2D> expandedGalaxies = Expand(galaxies, 1);

long totalDistance = 0;

foreach (LongPoint2D firstGalaxy in expandedGalaxies)
{
    foreach (LongPoint2D secondGalaxy in expandedGalaxies)
    {
        totalDistance += (firstGalaxy - secondGalaxy).TaxiCabLength;
    }
}

//Remove the double-counting
totalDistance /= 2;

//It _says_ the shortest path, but it's really just the manhattan distance.
Console.WriteLine($"The answer is: {totalDistance}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();


HashSet<LongPoint2D> superExpandedGalaxies = Expand(galaxies, 999_999);

long totalFinalDistance = 0;

foreach (LongPoint2D firstGalaxy in superExpandedGalaxies)
{
    foreach (LongPoint2D secondGalaxy in superExpandedGalaxies)
    {
        totalFinalDistance += (firstGalaxy - secondGalaxy).TaxiCabLength;
    }
}

//Remove the double-counting
totalFinalDistance /= 2;

Console.WriteLine($"The answer is: {totalFinalDistance}");

Console.WriteLine();
Console.ReadKey();


HashSet<LongPoint2D> Expand(HashSet<LongPoint2D> galaxies, long factor)
{
    long maxX = galaxies.Max(x => x.x);
    long maxY = galaxies.Max(x => x.y);

    HashSet<LongPoint2D> expandedX = new HashSet<LongPoint2D>();

    long expansion = 0;
    for (int x = 0; x <= maxX; x++)
    {
        if (!galaxies.Any(g => g.x == x))
        {
            expansion++;
            continue;
        }

        foreach (LongPoint2D galaxy in galaxies.Where(g => g.x == x))
        {
            expandedX.Add((x + (expansion * factor), galaxy.y));
        }
    }


    HashSet<LongPoint2D> expandedY = new HashSet<LongPoint2D>();

    expansion = 0;
    for (int y = 0; y <= maxY; y++)
    {
        if (!expandedX.Any(g => g.y == y))
        {
            expansion++;
            continue;
        }

        foreach (LongPoint2D galaxy in expandedX.Where(g => g.y == y))
        {
            expandedY.Add((galaxy.x, y + (expansion * factor)));
        }
    }

    return expandedY;
}