using AoCTools;
using System.Linq;

const string inputFile = @"../../../../input03.txt";

Console.WriteLine("Day 03 - Gear Ratios");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);
//string[] lines =
//{
//    "467..114..",
//    "...*......",
//    "..35..633.",
//    "......#...",
//    "617*......",
//    ".....+.58.",
//    "..592.....",
//    "......755.",
//    "...$.*....",
//    ".664.598.."
//};



char[,] schematic = lines.ToArrayGrid();

Dictionary<Point2D, PartNumber> valueMap = [];

List<Point2D> targetPoints = [];

for (int y = 0; y < schematic.GetLength(1); y++)
{
    long value = 0;

    targetPoints.Clear();

    for (int x = 0; x < schematic.GetLength(0); x++)
    {
        if (char.IsDigit(schematic[x, y]))
        {
            value *= 10;
            value += long.Parse(schematic[x, y].ToString());

            targetPoints.Add((x, y));
        }
        else
        {
            if (targetPoints.Count > 0)
            {
                PartNumber partNumber = new PartNumber(value, targetPoints.First());

                foreach (Point2D targetPoint in targetPoints)
                {
                    valueMap.Add(targetPoint, partNumber);
                }
            }

            targetPoints.Clear();
            value = 0;
        }
    }

    if (targetPoints.Count > 0)
    {
        PartNumber partNumber = new PartNumber(value, targetPoints.First());

        foreach (Point2D targetPoint in targetPoints)
        {
            valueMap.Add(targetPoint, partNumber);
        }
    }

    targetPoints.Clear();
}

HashSet<PartNumber> identifiedPartNumbers = new();


for (int y = 0; y < schematic.GetLength(1); y++)
{
    for (int x = 0; x < schematic.GetLength(1); x++)
    {
        if (!char.IsDigit(schematic[x, y]) && schematic[x, y] != '.')
        {
            foreach (Point2D point in (new Point2D(x, y)).GetFullAdjacent())
            {
                if (valueMap.ContainsKey(point))
                {
                    identifiedPartNumbers.Add(valueMap[point]);
                }
            }
        }
    }
}

//var excludedValues = valueMap.Values.Distinct().Except(identifiedPartNumbers);

//Console.WriteLine($"Excluded Values:");
//foreach(var value in excludedValues)
//{
//    Console.WriteLine($"  {value.FirstPoint}: {value.Value}");
//}

//Console.WriteLine();
//Console.WriteLine();

long cumulativePartNumber = identifiedPartNumbers.Sum(x => x.Value);

Console.WriteLine($"The answer is: {cumulativePartNumber}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

long value2 = 0;

HashSet<PartNumber> gearParts = new();

for (int y = 0; y < schematic.GetLength(1); y++)
{
    for (int x = 0; x < schematic.GetLength(1); x++)
    {
        //If it's a gear
        if (schematic[x, y] == '*')
        {
            gearParts.Clear();

            foreach (Point2D point in (new Point2D(x, y)).GetFullAdjacent())
            {
                if (valueMap.ContainsKey(point))
                {
                    gearParts.Add(valueMap[point]);
                }
            }

            if (gearParts.Count == 2)
            {
                value2 += gearParts.Aggregate(1L, (x, y) => x * y.Value);
            }
        }
    }
}


Console.WriteLine($"The answer is: {value2}");

Console.WriteLine();
Console.ReadKey();


public record PartNumber(long Value, Point2D FirstPoint);
