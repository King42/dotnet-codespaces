using Microsoft.VisualBasic;

namespace Advent.Advent2023;

public static class Day7
{
    public static string Name { get; } = "Camel Cards";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Part1(GetDataPart1(input));
        }
        else
        {
            return Part2(GetDataPart2(input));
        }
    }

    private static string Part1(List<(string hand, HandType handType, int bet)> data)
    {
        var winnings = 0L;

        int handIndex = 1;
        for (var i = HandType.HighCard; i <= HandType.FiveOfAKind; i++)
        {
            foreach (var hand in data.Where(t => t.handType == i).Order())
            {
                winnings += handIndex++ * hand.bet;
            }
        }

        return winnings.ToString();
    }

    private static string Part2((long time, long score) data)
    {
        throw new NotImplementedException();
    }

    private enum HandType
    {
        HighCard = 0,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        FullHouse,
        FourOfAKind,
        FiveOfAKind
    }

    private static List<(string hand, HandType handType, int bet)> GetDataPart1(string[] input)
    {
        var data = new List<(string hand, HandType handType, int bet)>();

        foreach (var line in input)
        {
            var handAndBet = line.Split(' ');
            var hand = handAndBet[0]
                .Replace('T', 'a')
                .Replace('J', 'b')
                .Replace('Q', 'c')
                .Replace('K', 'd')
                .Replace('A', 'e');
            var handType = DetermineHandType(hand);
            var bet = int.Parse(handAndBet[1]);
            data.Add((hand, handType, bet));
        }

        return data;
    }

    private static HandType DetermineHandType(string hand)
    {
        var cards = new Dictionary<char, int>();
        foreach (var c in hand)
        {
            if (!cards.TryAdd(c, 1))
            {
                cards[c]++;
            }
        }

        bool hasThreeOfAKind = false, hasPair = false;
        foreach (var card in cards)
        {
            switch (card.Value)
            {
                case 5:
                    return HandType.FiveOfAKind;
                case 4:
                    return HandType.FourOfAKind;
                case 3:
                    hasThreeOfAKind = true;
                    break;
                case 2:
                    if (hasPair)
                    {
                        return HandType.TwoPair;
                    }
                    hasPair = true;
                    break;
            }
        }

        if (hasThreeOfAKind && hasPair)
        {
            return HandType.FullHouse;
        }
        
        if (hasThreeOfAKind)
        {
            return HandType.ThreeOfAKind;
        }
        
        if (hasPair)
        {
            return HandType.OnePair;
        }

        return HandType.HighCard;
    }

    private static (long time, long score) GetDataPart2(string[] input)
    {
        var timeLine = input[0];
        var scoreLine = input[1];

        var time = long.Parse(timeLine.Split(':')[1].Replace(" ", null));
        var score = long.Parse(scoreLine.Split(':')[1].Replace(" ", null));

        return (time, score);
    }}