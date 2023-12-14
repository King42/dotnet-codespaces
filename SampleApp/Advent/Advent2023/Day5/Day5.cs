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
        return Day5Impl(input);
    }

    private static string Day5Part2(string[] input)
    {
        return Day5ImplWithRanges(input);
    }

    private static string Day5Impl(string[] input)
    {
        var data = GetData(input);

        var nextItems = data.Seeds;

        foreach (var map in data.Maps)
        {
            var items = nextItems;
            nextItems = new List<long>();

            foreach (var item in items)
            {
                long itemValue = item;

                (long sourceStart, long targetStart, long rangeLength, long offsetToTarget) matchingVector = default;
                foreach (var vector in map.Vectors)
                {
                    if (map.SourceIsInRange(itemValue, vector.sourceStart, vector.rangeLength))
                    {
                        matchingVector = vector;
                    }
                }

                nextItems.Add(itemValue + matchingVector.offsetToTarget);
            }
        }

        return nextItems.Min().ToString();
    }

    private static string Day5ImplWithRanges(string[] input)
    {
        throw new NotImplementedException();
        
        var locations = new List<long>();

        var data = GetData(input);

        var itemRanges = data.SeedRanges;

        for (int i = 0; i < itemRanges.Count; i++)
        {
            var itemRange = itemRanges[i];

            foreach (var map in data.Maps)
            {
                var firstItem = itemRange.start;
                long lastItem = itemRange.start + itemRange.length;

                (long sourceStart, long targetStart, long rangeLength, long offsetToTarget) matchingVector = default;
                foreach (var vector in map.Vectors)
                {
                    if (map.SourceIsInRange(lastItem, vector.sourceStart, vector.rangeLength))
                    {
                        matchingVector = vector;

                        long newFirstItem, newLength;
                        if (map.SourceIsInRange(firstItem, vector.sourceStart, vector.rangeLength))
                        {
                            // entire seed range is contained within this vector
                            newFirstItem = firstItem + matchingVector.offsetToTarget;
                            newLength = itemRange.length;
                        }
                        else
                        {
                            // only part of seed range is contained within this vector
                            newFirstItem = matchingVector.sourceStart + matchingVector.offsetToTarget;
                            newLength = lastItem - matchingVector.sourceStart;

                            // add items not contained within the vector back onto the end of the list
                            itemRanges.Add((itemRange.start, itemRange.length - newLength));
                        }
                        
                        itemRange = (newFirstItem, newLength);
                    }
                }
                //itemValue += matchingVector.offsetToTarget;
            }

            //locations.Add(itemValue);
        }

        return locations.Min().ToString();
    }
   
    private static Regex SeedLineParser = new Regex(@"seeds: (?<numbers>.*)");
    private static Regex MapNameParser = new Regex(@"(?<source>\w+)-to-(?<target>\w+) map:");
    private static Regex VectorParser = new Regex(@"(?<targetStart>\d+) +(?<sourceStart>\d+) +(?<rangeLength>\d+)");

    private static Day5Data GetData(string[] input)
    {
        var seedValues = Parsers.ExtractNumbersFromList<long>(SeedLineParser.Match(input.First()).Groups["numbers"].Value);
        var data = new Day5Data()
        {
            Seeds = seedValues
        };

        for (int i = 0; i < seedValues.Count; i += 2)
        {
            var start = seedValues[i];
            var length = seedValues[i+1];
            data.SeedRanges.Add((start, length));
        }

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