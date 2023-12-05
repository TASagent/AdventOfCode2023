using System.Text.RegularExpressions;

const string inputFile = @"../../../../input04.txt";

Console.WriteLine("Day 04 - Scratchcards");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);
//string line = File.ReadAllText(inputFile);

List<Card> cards = lines.Select(x => new Card(x)).ToList();

int value = cards.Select(x => x.Points).Sum();

Console.WriteLine($"The answer is: {value}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

List<int> pendingCards = new() { 1 };
int cardCount = 0;

for (int i = 1; i < (1<<9); i++)
{
    pendingCards.Add(1);
}

foreach (Card card in cards)
{
    int count = pendingCards[0];

    if (count == 0)
        break;

    pendingCards.RemoveAt(0);
    pendingCards.Add(0);

    int wins = card.WinCount;

    for (int i = 0; i < wins; i++)
    {
        pendingCards[i] += count;
    }

    cardCount += count;
}

Console.WriteLine($"The answer is: {cardCount}");

Console.WriteLine();
Console.ReadKey();

public class Card
{
    public readonly int number;
    public readonly List<int> winners = new();
    public readonly List<int> guesses = new();

    public int WinCount => winners.Intersect(guesses).Count();

    public int Points
    {
        get
        {
            int winCount = WinCount;

            if (winCount == 0)
                return 0;

            return 1 << (winCount - 1);
        }
    }

    public Card(string line)
    {
        number = int.Parse(Regex.Match(line, @"Card\s*(\d+)\:").Groups[1].Value);

        string[] splitLine =
            line.Split(new[] { ':', ' ', '|' }, StringSplitOptions.RemoveEmptyEntries);

        winners.AddRange(splitLine[2..12].Select(int.Parse));
        guesses.AddRange(splitLine[12..].Select(int.Parse));
    }
}
