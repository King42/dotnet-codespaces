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
        var locations = new List<long>();

        var data = GetData(input);

        foreach (var seed in data.Seeds)
        {
            long itemValue = seed;

            foreach (var map in data.Maps)
            {
                (long sourceStart, long targetStart, long rangeLength, long offsetToTarget) matchingVector = default;
                foreach (var vector in map.Vectors)
                {
                    if (map.SourceIsInRange(itemValue, vector.sourceStart, vector.rangeLength))
                    {
                        matchingVector = vector;
                    }
                }
                itemValue += matchingVector.offsetToTarget;
            }

            locations.Add(itemValue);
        }

        return locations.Min().ToString();
    }

    private static string Day5Part2(string[] input)
    {
        var answer = 0L;

        throw new NotImplementedException();
    }
   
    private static Regex SeedLineParser = new Regex(@"seeds: (?<numbers>.*)");
    private static Regex MapNameParser = new Regex(@"(?<source>\w+)-to-(?<target>\w+) map:");
    private static Regex VectorParser = new Regex(@"(?<targetStart>\d+) +(?<sourceStart>\d+) +(?<rangeLength>\d+)");

    private static Day5Data GetData(string[] input)
    {
        var data = new Day5Data()
        {
            Seeds = Parsers.ExtractNumbersFromList<long>(SeedLineParser.Match(input.First()).Groups["numbers"].Value)
        };

        bool nameParsed = false;
        Day5Map? map = null;

        foreach (var line in input.Skip(1))
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                // prep for next map including the 1st one
                map = new Day5Map();
                data.Maps.Add(map);
                nameParsed = false;
            }
            else if (!nameParsed)
            {
                var match = MapNameParser.Match(line);
                if (map == null || !match.Success)
                {
                    throw new InvalidOperationException();
                }
                map.SourceItem = match.Groups["source"].Value;
                map.TargetItem = match.Groups["target"].Value;
                nameParsed = true;
            }
            else
            {
                var match = VectorParser.Match(line);
                if (map == null || !match.Success)
                {
                    throw new InvalidOperationException();
                }
                
                long sourceStart = long.Parse(match.Groups["sourceStart"].Value);
                long targetStart = long.Parse(match.Groups["targetStart"].Value);
                long rangeLength = long.Parse(match.Groups["rangeLength"].Value);
                map.Vectors.Add((sourceStart, targetStart, rangeLength, targetStart - sourceStart));
            }
        }

        // default sorting of a tuple will sort primarily by the first item (i.e. sourceStart)
        data.Maps.ForEach(m => m.Vectors.Sort());
        data.Maps.ForEach(m => m.Vectors.Reverse());
        return data;
    }
}