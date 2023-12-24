namespace Advent.Advent2023;

public static class Day11
{
    public static string Name { get; } = "Cosmic Expansion";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Solve(GetData(input, false), false);
        }
        else
        {
            return Solve(GetData(input, true), true);
        }
    }

    private static string Solve(object data, bool part2)
    {
        throw new NotImplementedException();
    }

    private static object GetData(string[] input, bool part2)
    {
        throw new NotImplementedException();
    }
}