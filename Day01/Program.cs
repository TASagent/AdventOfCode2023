using System.Text.RegularExpressions;

const string inputFile = @"../../../../input01.txt";

Console.WriteLine("Day 01 - Trebuchet");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);

int calibrationValue = 0;

Regex firstNumber = new Regex(@"^[a-z]*(\d)");
Regex lastNumber = new Regex(@"(\d)[a-z]*$");

foreach (string line in lines)
{
    calibrationValue += 10 * int.Parse(firstNumber.Match(line).Groups[1].Value) +
        int.Parse(lastNumber.Match(line).Groups[1].Value);
}



Console.WriteLine($"The answer is: {calibrationValue}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

Dictionary<string, int> valueMap = new()
{
    { "0", 0 },
    { "1", 1 },
    { "2", 2 },
    { "3", 3 },
    { "4", 4 },
    { "5", 5 },
    { "6", 6 },
    { "7", 7 },
    { "8", 8 },
    { "9", 9 },
    { "one", 1 },
    { "two", 2 },
    { "three", 3 },
    { "four", 4 },
    { "five", 5 },
    { "six", 6 },
    { "seven", 7 },
    { "eight", 8 },
    { "nine", 9 },
};


Regex newFirstNumber = new Regex(@"^[a-z]*?(one|two|three|four|five|six|seven|eight|nine|\d)");
Regex newLastNumber = new Regex(@"[a-z0-9]*(\d|one|two|three|four|five|six|seven|eight|nine)");

int calibrationValue2 = 0;

foreach (string line in lines)
{
    calibrationValue2 += 10 * valueMap[newFirstNumber.Match(line).Groups[1].Value] +
        valueMap[newLastNumber.Match(line).Groups[1].Value];
}



Console.WriteLine($"The answer is: {calibrationValue2}");

Console.WriteLine();
Console.ReadKey();
