using AoCTools;
using System.Text.RegularExpressions;

const string inputFile = @"../../../../input02.txt";

Point3D limit = (12, 13, 14);

Console.WriteLine("Day 02 - Cube Conundrum");
Console.WriteLine("Star 1");
Console.WriteLine();

List<Game> games = File.ReadAllLines(inputFile).Select(x => new Game(x)).ToList();

int value = 0;

foreach (Game game in games)
{
    if (game.draws.Append(limit).MaxCoordinate() == limit)
    {
        value += game.num;
    }
}

Console.WriteLine($"The answer is: {value}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

long value2 = 0;

foreach (Game game in games)
{
    Point3D max = game.draws.MaxCoordinate();
    value2 += max.x * max.y * max.z;
}

Console.WriteLine($"The answer is: {value2}");

Console.WriteLine();
Console.ReadKey();


class Game
{
    public readonly int num;
    public readonly List<Point3D> draws = new List<Point3D>();

    private static Regex redMatch = new Regex(@" (\d+) red");
    private static Regex greenMatch = new Regex(@" (\d+) green");
    private static Regex blueMatch = new Regex(@" (\d+) blue");

    public Game(string line)
    {
        num = int.Parse(Regex.Match(line, @"^Game (\d+):").Groups[1].Value);

        string[] games = line.Substring(line.IndexOf(':') + 1).Split(";");

        foreach (string game in games)
        {
            draws.Add(new(
                x: redMatch.IsMatch(game) ? int.Parse(redMatch.Match(game).Groups[1].Value) : 0,
                y: greenMatch.IsMatch(game) ? int.Parse(greenMatch.Match(game).Groups[1].Value) : 0,
                z: blueMatch.IsMatch(game) ? int.Parse(blueMatch.Match(game).Groups[1].Value) : 0));
        }
    }
}