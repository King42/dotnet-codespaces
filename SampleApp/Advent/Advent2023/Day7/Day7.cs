using Microsoft.VisualBasic;

namespace Advent.Advent2023;

public static class Day7
{
    public static string Name { get; } = "Camel Cards";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Part1(GetData(input, false));
        }
        else
        {
            return Part1(GetData(input, true));
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

    private static List<(string hand, HandType handType, int bet)> GetData(string[] input, bool part2)
    {
        var data = new List<(string hand, HandType handType, int bet)>();

        foreach (var line in input)
        {
            var handAndBet = line.Split(' ');
            var hand = handAndBet[0];
            var handType = DetermineHandType(hand, part2);
            var bet = int.Parse(handAndBet[1]);

            hand = hand
                .Replace('T', 'a')
                .Replace('J', part2 ? '1' : 'b')
                .Replace('Q', 'c')
                .Replace('K', 'd')
                .Replace('A', 'e');

            data.Add((hand, handType, bet));
        }

        return data;
    }

    private static HandType DetermineHandType(string hand, bool part2)
    {
        var cards = new Dictionary<char, int>();
        foreach (var c in hand)
        {
            if (!cards.TryAdd(c, 1))
            {
                cards[c]++;
            }
        }

        bool hasFiveOfAKind = false, hasFourOfAKind = false, hasThreeOfAKind = false, hasTwoPair = false, hasPair = false;
        foreach (var card in cards.Where(c => !part2 || c.Key != 'J'))
        {
            switch (card.Value)
            {
                case 5:
                    hasFiveOfAKind = true;
                    break;
                case 4:
                    hasFourOfAKind = true;
                    break;
                case 3:
                    hasThreeOfAKind = true;
                    break;
                case 2:
                    if (hasPair)
                    {
                        hasTwoPair = true;
                    }
                    else
                    {
                        hasPair = true;
                    }
                    break;
            }
        }

        int jokerCount = part2 && cards.ContainsKey('J') ? cards['J'] : 0;

        if (hasFiveOfAKind)
        {
            return HandType.FiveOfAKind;
        }

        if (hasFourOfAKind)
        {
            if (jokerCount == 1)
            {
                return HandType.FiveOfAKind;
            }
            return HandType.FourOfAKind;
        }
        
        if (hasThreeOfAKind && hasPair)
        {
            return HandType.FullHouse;
        }
        
        if (hasThreeOfAKind)
        {
            if (jokerCount == 2)
            {
                return HandType.FiveOfAKind;
            }
            else if (jokerCount == 1)
            {
                return HandType.FourOfAKind;
            }
            return HandType.ThreeOfAKind;
        }

        if (hasTwoPair)
        {
            if (jokerCount == 1)
            {
                return HandType.FullHouse;
            }
            return HandType.TwoPair;
        }
        
        if (hasPair)
        {
            if (jokerCount == 3)
            {
                return HandType.FiveOfAKind;
            }
            else if (jokerCount == 2)
            {
                return HandType.FourOfAKind;
            }
            else if (jokerCount == 1)
            {
                return HandType.ThreeOfAKind;
            }
            return HandType.OnePair;
        }

        if (jokerCount >= 4)
        {
            return HandType.FiveOfAKind;
        }
        
        if (jokerCount == 3)
        {
            return HandType.FourOfAKind;
        }

        if (jokerCount == 2)
        {
            return HandType.ThreeOfAKind;
        }

        if (jokerCount == 1)
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