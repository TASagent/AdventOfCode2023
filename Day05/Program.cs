using System.Collections.Generic;

const string inputFile = @"../../../../input05.txt";

const int MAPPING_COUNT = 7;

Console.WriteLine("Day 05 - If You Give A Seed A Fertilizer");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);


List<long> seeds = new List<long>(lines[0][7..].Split(' ').Select(long.Parse));

int lineNum = 3;
int layerNum = 0;

List<Mapping>[] mappings = new List<Mapping>[MAPPING_COUNT];

for (int i = 0; i < MAPPING_COUNT; i++)
{
    mappings[i] = new List<Mapping>();
}

while (lineNum < lines.Length)
{
    if (string.IsNullOrEmpty(lines[lineNum]))
    {
        lineNum += 2;
        layerNum++;
        continue;
    }

    List<long> values = lines[lineNum].Split(' ').Select(long.Parse).ToList();
    mappings[layerNum].Add(new Mapping(values[0], values[1], values[2]));
    lineNum++;
}

long lowestLocation = long.MaxValue;

foreach (long seed in seeds)
{
    long output = seed;
    for (int layer = 0; layer < MAPPING_COUNT; layer++)
    {
        output = GetMapping(output, layer, mappings);
    }

    lowestLocation = Math.Min(lowestLocation, output);
}

Console.WriteLine($"The answer is: {lowestLocation}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

for (int i = 0; i < MAPPING_COUNT; i++)
{
    mappings[layerNum].Sort((x, y) => x.SourceStart.CompareTo(y.SourceStart));
}

List<(long start, long range)> startingRanges = new List<(long start, long range)>();

for (int seedIndex = 0; seedIndex < seeds.Count; seedIndex += 2)
{
    startingRanges.Add((seeds[seedIndex], seeds[seedIndex + 1]));
}

IEnumerable<(long start, long range)> ranges = startingRanges;

for (int layer = 0; layer < MAPPING_COUNT; layer++)
{
    int layerValue = layer;
    ranges = ranges.SelectMany(x => GetRangeMapping(x, layerValue, mappings));
}

List<(long start, long range)> finalRanges = ranges.ToList();

Console.WriteLine($"The answer is: {finalRanges.Min(x => x.start)}");

Console.WriteLine();
Console.ReadKey();

long GetMapping(long input, int layer, List<Mapping>[] mappings)
{
    foreach (Mapping mapping in mappings[layer])
    {
        if (input >= mapping.SourceStart && input < mapping.SourceStart + mapping.Range)
        {
            return (input - mapping.SourceStart) + mapping.DestStart;
        }
    }

    return input;
}

IEnumerable<(long start, long range)> GetRangeMapping(
    (long start, long range) input,
    int layer,
    List<Mapping>[] mappings)
{
    List<Mapping> relevantMappings = mappings[layer];

    long newStart = input.start;
    long remainingRange = input.range;

    for (int mappingNum = 0; mappingNum < relevantMappings.Count; mappingNum++)
    {
        if (relevantMappings[mappingNum].IsRangeInside(newStart, remainingRange))
        {
            long segmentRange;
            if (newStart < relevantMappings[mappingNum].SourceStart)
            {
                //Preceeding Segment
                segmentRange = relevantMappings[mappingNum].SourceStart - newStart;
                yield return (newStart, segmentRange);

                remainingRange -= segmentRange;
                newStart += segmentRange;
            }

            long segmentEnd = Math.Min(
                newStart + remainingRange,
                relevantMappings[mappingNum].SourceStart + relevantMappings[mappingNum].Range);

            segmentRange = segmentEnd - newStart;

            //Interior Segment
            yield return (newStart - relevantMappings[mappingNum].SourceStart + relevantMappings[mappingNum].DestStart, segmentRange);

            newStart += segmentRange;
            remainingRange -= segmentRange;
        }
    }

    if (remainingRange > 0)
    {
        yield return (newStart, remainingRange);
    }
}

record Mapping(long DestStart, long SourceStart, long Range)
{
    public bool IsSourceInside(long value) =>
        value >= SourceStart && value < SourceStart + Range;
    public bool IsRangeInside(long value, long range) =>
        !(value >= (SourceStart + Range) || (value + range) <= SourceStart);
}
