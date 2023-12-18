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

    private static string Part1(List<(HandType hand, int bet)> data)
    {
        throw new NotImplementedException();
    }

    private static string Part2((long time, long score) data)
    {
        throw new NotImplementedException();
    }

    private enum HandType
    {
        Nothing,
        FiveOfAKind
    }

    private static List<(HandType hand, int bet)> GetDataPart1(string[] input)
    {
        throw new NotImplementedException();
        var data = new List<(HandType hand, int bet)>();

        var timeLine = input[0];
        var scoreLine = input[1];

        var times = Parsers.ExtractNumbersFromList<int>(timeLine.Split(':')[1]);
        var scores = Parsers.ExtractNumbersFromList<int>(scoreLine.Split(':')[1]);


        return data;
    }

    private static (long time, long score) GetDataPart2(string[] input)
    {
        var timeLine = input[0];
        var scoreLine = input[1];

        var time = long.Parse(timeLine.Split(':')[1].Replace(" ", null));
        var score = long.Parse(scoreLine.Split(':')[1].Replace(" ", null));

        return (time, score);
    }}