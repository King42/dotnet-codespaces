namespace Advent.Advent2023;

using System.Text.RegularExpressions;

public class Day5
{
    public string Name { get; } = "If You Give A Seed A Fertilizer";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Day5Part1(input);
        }
        else
        {
            return Day5Part2(input);
        }
    }

    private static string Day5Part1(string[] input)
    {
        var answer = 0L;

        var data = GetData(input);

        throw new NotImplementedException();
    }

    private static string Day5Part2(string[] input)
    {
        var answer = 0L;

        throw new NotImplementedException();
    }
   
    private static Regex SeedLine = new Regex(@"seeds: (?<numbers>.*)");

    private static Day5Data GetData(string[] input)
    {
        var data = new Day5Data()
        {
            Seeds = Parsers.ExtractNumbersFromList<long>(SeedLine.Match(input.First()).Groups["numbers"].Value)
        };

        return data;
    }
}