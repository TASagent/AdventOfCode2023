using AoCTools;

const string inputFile = @"../../../../input08.txt";

Console.WriteLine("Day 08 - Haunted Wasteland");
Console.WriteLine("Star 1");
Console.WriteLine();

Dictionary<string, Node> nodeMap = new Dictionary<string, Node>();

string[] lines = File.ReadAllLines(inputFile);

string directions = lines[0];

foreach (string line in lines.Skip(2))
{
    Node nextNode = new Node(line);

    nodeMap.Add(nextNode.Name, nextNode);
}

int stepCount = 0;
int directionIndex = 0;

Node currentNode = nodeMap["AAA"];
Node endNode = nodeMap["ZZZ"];

while (currentNode != endNode)
{
    if (directionIndex >= directions.Length)
    {
        directionIndex = 0;
    }

    currentNode = currentNode.Travel(directions[directionIndex], nodeMap);

    directionIndex++;
    stepCount++;
}

Console.WriteLine($"The answer is: {stepCount}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

List<long> loopLengths = nodeMap.Keys.Where(x => x.EndsWith('A')).Select(nodeMap.GetValueOrDefault).Select(Characterize).ToList();

long totalLoopLength = 1;

foreach(long loopLength in loopLengths)
{
    totalLoopLength = AoCMath.LCM(totalLoopLength, loopLength);
}


Console.WriteLine($"The answer is: {totalLoopLength}");

Console.WriteLine();
Console.ReadKey();

long Characterize(Node startingNode)
{
    long testStepCount = 0;
    while (!startingNode.Name.EndsWith('Z'))
    {
        if (directionIndex >= directions.Length)
        {
            directionIndex = 0;
        }

        startingNode = startingNode.Travel(directions[directionIndex], nodeMap);

        directionIndex++;
        testStepCount++;
    }

    return testStepCount;
}


class Node
{
    public string Name { get; }
    public string Left { get; }
    public string Right { get; }

    public Node(string line)
    {
        Name = line[0..3];
        Left = line[7..10];
        Right = line[12..15];
    }

    public Node Travel(char direction, Dictionary<string, Node> nodeMap)
    {
        switch (direction)
        {
            case 'L': return nodeMap[Left];
            case 'R': return nodeMap[Right];
            default: throw new Exception($"Unexpected Direction: {direction}");
        }
    }
}