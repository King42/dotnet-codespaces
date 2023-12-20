namespace Advent.Advent2023;

using System.Text.RegularExpressions;

public static class Day9
{
    public static string Name { get; } = "Mirage Maintenance";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Part1(GetData(input, false));
        }
        else
        {
            return Part2(GetData(input, true));
        }
    }

    private static string Part1(object data)
    {
        throw new NotImplementedException();
    }

    private static string Part2(object data)
    {
        throw new NotImplementedException();
    }

    private static object GetData(string[] input, bool part2)
    {
        throw new NotImplementedException();
    }
}