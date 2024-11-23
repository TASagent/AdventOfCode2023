const string inputFile = @"../../../../input09.txt";

Console.WriteLine("Day 09 - Mirage Maintenance");
Console.WriteLine("Star 1");
Console.WriteLine();

IEnumerable<string> lines = File.ReadAllLines(inputFile).Where(x=>x.Length > 0);
//IEnumerable<string> lines = new List<string>() { "10 13 16 21 30 45" };


List<long> newElements = new List<long>();

foreach (string line in lines)
{
    newElements.Add(FindNextElement(line));
}

Console.WriteLine($"The answer is: {newElements.Sum()}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

List<long> newPreviousElements = new List<long>();

foreach (string line in lines)
{
    newPreviousElements.Add(FindPreviousElement(line));
}

Console.WriteLine($"The answer is: {newPreviousElements.Sum()}");

Console.WriteLine();
Console.ReadKey();


//Approach: Iterate, pushing old lists into stack, until elements are all 0
long FindNextElement(string line)
{
    Stack<List<long>> sequenceStack = new Stack<List<long>>();

    List<long> sequence = line.Split(' ').Select(long.Parse).ToList();

    sequenceStack.Push(sequence);

    while (!sequence.All(x => x == 0))
    {
        List<long> newSequence = new List<long>();

        for (int i = 0; i < sequence.Count - 1; i++)
        {
            newSequence.Add(sequence[i + 1] - sequence[i]);
        }

        sequenceStack.Push(newSequence);

        sequence = newSequence;
    }

    long result = 0;

    while (sequenceStack.Count > 0)
    {
        result += sequenceStack.Pop()[^1];
    }

    return result;
}

//Approach: Iterate, pushing old lists into stack, until elements are all 0
long FindPreviousElement(string line)
{
    Stack<List<long>> sequenceStack = new Stack<List<long>>();

    List<long> sequence = line.Split(' ').Select(long.Parse).ToList();

    sequenceStack.Push(sequence);

    while (!sequence.All(x => x == 0))
    {
        List<long> newSequence = new List<long>();

        for (int i = 0; i < sequence.Count - 1; i++)
        {
            newSequence.Add(sequence[i + 1] - sequence[i]);
        }

        sequenceStack.Push(newSequence);

        sequence = newSequence;
    }

    long result = 0;

    while (sequenceStack.Count > 0)
    {
        result = sequenceStack.Pop()[0] - result;
    }

    return result;
}