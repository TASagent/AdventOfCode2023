const string inputFile = @"../../../../input09.txt";

Console.WriteLine("Day 09 - Mirage Maintenance");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);

List<long> newElements = new List<long>();

foreach (string line in lines)
{
    newElements.Add(FindNextElement(line));
}

Console.WriteLine($"The answer is: {value}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

int value2 = 0;

Console.WriteLine($"The answer is: {value2}");

Console.WriteLine();
Console.ReadKey();


//Approach: Iterate, pushing old lists into stack, until elements are all 0
long FindNextElement(string line)
{
    IEnumerable<>
}