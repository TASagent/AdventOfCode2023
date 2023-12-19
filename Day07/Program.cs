const string inputFile = @"../../../../input07.txt";

Console.WriteLine("Day 07 - Camel Cards");
Console.WriteLine("Star 1");
Console.WriteLine();

string[] lines = File.ReadAllLines(inputFile);

List<Hand> hands = lines.Select(x => new Hand(x)).ToList();

long value = 0;

hands.Sort((x,y) => Hand.Compare(x,y));

for(int i = 0; i < hands.Count; i++)
{
    value += (hands.Count - i) * hands[i].Bid;
}

Console.WriteLine($"The answer is: {value}");

Console.WriteLine();
Console.WriteLine("Star 2");
Console.WriteLine();

long value2 = 0;

hands.Sort((x, y) => Hand.Compare2(x, y));

for (int i = 0; i < hands.Count; i++)
{
    value2 += (hands.Count - i) * hands[i].Bid;
}

Console.WriteLine($"The answer is: {value2}");

Console.WriteLine();
Console.ReadKey();

public record Hand
{
    public readonly string HandValues;
    public readonly int Bid;

    public readonly HandType HandType;
    public readonly HandType BestHandType;
    public Hand(string input)
    {
        string[] splitString = input.Split(' ');

        HandValues = splitString[0];
        Bid = int.Parse(splitString[1]);

        HandType = GetHandType(HandValues);
        BestHandType = GetBestHandType(HandValues);
    }

    public static int Compare(in Hand a, in Hand b)
    {
        if (a.HandType < b.HandType)
        {
            return -1;
        }

        if (b.HandType < a.HandType)
        {
            return 1;
        }

        for (int i = 0; i < a.HandValues.Length; i++)
        {
            if (a.HandValues[i] == b.HandValues[i])
            {
                continue;
            }

            if (MapCard(a.HandValues[i]) > MapCard(b.HandValues[i]))
            {
                return -1;
            }

            return 1;
        }

        throw new Exception();
    }

    public static int Compare2(in Hand a, in Hand b)
    {
        if (a.BestHandType < b.BestHandType)
        {
            return -1;
        }

        if (b.BestHandType < a.BestHandType)
        {
            return 1;
        }

        for (int i = 0; i < a.HandValues.Length; i++)
        {
            if (a.HandValues[i] == b.HandValues[i])
            {
                continue;
            }

            if (MapCard2(a.HandValues[i]) > MapCard2(b.HandValues[i]))
            {
                return -1;
            }

            return 1;
        }

        throw new Exception();
    }

    private static int MapCard(char c) => c switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'J' => 11,
        'T' => 10,
        '9' => 9,
        '8' => 8,
        '7' => 7,
        '6' => 6,
        '5' => 5,
        '4' => 4,
        '3' => 3,
        '2' => 2,
        '1' => 1,
        _ => throw new Exception(),
    };

    private static int MapCard2(char c) => c switch
    {
        'A' => 14,
        'K' => 13,
        'Q' => 12,
        'T' => 10,
        '9' => 9,
        '8' => 8,
        '7' => 7,
        '6' => 6,
        '5' => 5,
        '4' => 4,
        '3' => 3,
        '2' => 2,
        '1' => 1,
        'J' => 0,
        _ => throw new Exception(),
    };

    private HandType GetHandType(string handValues)
    {
        Dictionary<char, int> cardCounts = new Dictionary<char, int>();

        foreach (char c in handValues)
        {
            if (cardCounts.ContainsKey(c))
            {
                cardCounts[c]++;
            }
            else
            {
                cardCounts[c] = 1;
            }
        }

        int maxCount = cardCounts.Values.Max();

        if (maxCount == 5)
        {
            return HandType.FiveOfKind;
        }
        else if (maxCount == 4)
        {
            return HandType.FourOfKind;
        }
        else if (maxCount == 3)
        {
            if (cardCounts.Count == 2)
            {
                return HandType.FullHouse;
            }

            return HandType.ThreeOfKind;
        }
        else if (maxCount == 2)
        {
            if (cardCounts.Count == 3)
            {
                return HandType.TwoPair;
            }

            return HandType.OnePair;
        }

        return HandType.FuckingNothing;
    }

    private HandType GetBestHandType(string handValues)
    {
        Dictionary<char, int> cardCounts = new Dictionary<char, int>();

        foreach (char c in handValues)
        {
            if (cardCounts.ContainsKey(c))
            {
                cardCounts[c]++;
            }
            else
            {
                cardCounts[c] = 1;
            }
        }

        int jokerCount = 0;
        
        if (cardCounts.ContainsKey('J'))
        {
            jokerCount = cardCounts['J'];
            cardCounts.Remove('J');
        }

        if (jokerCount == 5)
        {
            return HandType.FiveOfKind;
        }

        int maxCount = cardCounts.Values.Max();

        if ((maxCount + jokerCount) == 5)
        {
            return HandType.FiveOfKind;
        }
        else if ((maxCount + jokerCount) == 4)
        {
            return HandType.FourOfKind;
        }
        else if ((maxCount + jokerCount) == 3)
        {
            if (cardCounts.Count == 2)
            {
                return HandType.FullHouse;
            }

            return HandType.ThreeOfKind;
        }
        else if ((maxCount + jokerCount) == 2)
        {
            if (cardCounts.Count == 3)
            {
                return HandType.TwoPair;
            }

            return HandType.OnePair;
        }

        return HandType.FuckingNothing;
    }
}

public enum HandType
{
    FiveOfKind = 0,
    FourOfKind,
    FullHouse,
    ThreeOfKind,
    TwoPair,
    OnePair,
    FuckingNothing
}
