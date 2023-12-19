const string inputFile = @"../../../../input06.txt";

Console.WriteLine("Day 06 - Wait For It");
Console.WriteLine("Star 1");
Console.WriteLine();

//Time:         35      93     73     66
//Distance:    212    2060   1201   1044

List<(long time, long distance)> races = new List<(long time, long distance)>
{
    (35, 212),
    (93, 2060),
    (73, 1201),
    (66, 1044)
};


long value = races.Select(CountWinningStrategies).Aggregate(1L, (x, y) => x * y);

Console.WriteLine($"The answer is: {value}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

long value2 = CountWinningStrategies((35937366, 212206012011044));

Console.WriteLine($"The answer is: {value2}");

Console.WriteLine();
Console.ReadKey();

long CountWinningStrategies((long time, long distance) input)
{
    long winningCount = 0;
    for (long holdTime = 1; holdTime < input.time; holdTime++)
    {
        if (holdTime * (input.time - holdTime) > input.distance)
        {
            winningCount++;
        }
    }

    return winningCount;
}
