using System.Text.RegularExpressions;

namespace Advent.Advent2023;

public static class Day5
{
    public const string Name = "If You Give A Seed A Fertilizer";

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

        // walks through each map and maps all the items to the next set of values
        foreach (var map in data.Maps)
        {
            var items = nextItems;
            nextItems = new List<long>();

            foreach (var item in items)
            {
                long itemValue = item;

                (long sourceStart, long sourceEnd, long targetStart, long targetEnd, long rangeLength, long offsetToTarget) matchingVector = default;
                foreach (var vector in map.Vectors)
                {
                    // if the source item is contained with the range of the vector
                    if (map.SourceIsInRange(itemValue, vector.sourceStart, vector.sourceEnd))
                    {
                        matchingVector = vector;
                    }
                }

                // if we didn't find a matching vector then offsetToTarget will still have the default value of 0 here
                nextItems.Add(itemValue + matchingVector.offsetToTarget);
            }
        }

        return nextItems.Min().ToString();
    }

    private static string Day5ImplWithRanges(string[] input)
    {
        var data = GetData(input);

        var nextItemRanges = data.SeedRanges;

        // walks through each map and maps all the item ranges to the next set of item ranges
        foreach (var map in data.Maps)
        {
            var itemRanges = nextItemRanges;
            nextItemRanges = new List<(long start, long end, long length)>();

            // for each item range, it may be processed in it's entirety or effectively broken into two parts
            // if it's broken into two parts then one part will be processed and the other will be appended to the end
            // so using a for loop since itemRanges.Count will grow as we run through the for loop
            for (int i = 0; i < itemRanges.Count; i++)
            {
                var itemRange = itemRanges[i];
                var firstItem = itemRange.start;
                long lastItem = itemRange.end;

                var newItemRange = itemRange;

                foreach (var vector in map.Vectors)
                {
                    bool firstItemInRange = map.SourceIsInRange(firstItem, vector.sourceStart, vector.sourceEnd);
                    bool lastItemInRange = map.SourceIsInRange(lastItem, vector.sourceStart, vector.sourceEnd);

                    // there are 6 scenarios
                    // 1: the entire item range is above the vector
                    // 2: the entire item range is below the vector
                    // 3: the item range begins and ends within the vector (i.e. the item range is entirely contained with the vector)
                    // 4: the item range begins below the vector and ends within it (i.e. overlapping the bottom of the vector)
                    // 5: the item range begins within the vector and ends above it (i.e. overlapping the top of the vector)
                    // 6: the item range begins below the vector and ends above it (i.e. the vector is entirely contained within the item range)

                    if (lastItemInRange)
                    {
                        if (firstItemInRange)
                        {
                            // scenario 3

                            // we can process the entire item range and don't have to break it in two
                            newItemRange.start = firstItem + vector.offsetToTarget;
                            newItemRange.end = lastItem + vector.offsetToTarget;
                            newItemRange.length = newItemRange.end - newItemRange.start + 1;
                        }
                        else
                        {
                            // scenario 4

                            // capture ending section of item range that is contained within this vector
                            newItemRange.start = vector.targetStart;
                            newItemRange.end = lastItem + vector.offsetToTarget;
                            newItemRange.length = newItemRange.end - newItemRange.start + 1;

                            // add beginning section of item range not contained within the vector back onto the end of the list
                            // when it's processed again it will not stop on this vector again
                            itemRanges.Add((firstItem, vector.sourceStart - 1, itemRange.length - newItemRange.length));
                        }
                        break;
                    }
                    else if (map.SourceIsInRange(vector.sourceEnd, firstItem, lastItem))
                    {
                        // scenarios 5 & 6
                        // break the item range in two, process the section that's above the vector and put the rest back onto the list

                        // capture ending section of item range which extends beyond the vector (offset = 0)
                        newItemRange.start = vector.sourceEnd + 1;
                        newItemRange.end = lastItem;
                        newItemRange.length = newItemRange.end - newItemRange.start + 1;

                        // add beginning section of item range back onto the end of the list
                        // when it's processed again it will stop on this same vector again but will now fall into scenario 3 or 4
                        itemRanges.Add((firstItem, vector.sourceEnd, itemRange.length - newItemRange.length));

                        break;
                    }

                    // scenario 1 & 2
                    // do nothing
                }

                nextItemRanges.Add(newItemRange);
            }
        }

        // Because the "start" value is the first item in the tuple then the minimum of the tuples is the same as the minimum of the "start" values
        return nextItemRanges.Min().start.ToString();
    }
   
    private static Regex SeedLineParser = new Regex(@"seeds: (?<numbers>.*)");
    private static Regex MapNameParser = new Regex(@"(?<source>\w+)-to-(?<target>\w+) map:");
    private static Regex VectorParser = new Regex(@"(?<targetStart>\d+) +(?<sourceStart>\d+) +(?<rangeLength>\d+)");

    // Pretty much a straight mapping of the data to an equivalent data structure
    // The Day5Data structure contains the list of seeds (or seed ranges for part 2) and a list of all the mappings
    // Each line in a mapping is referred to as a vector which is a tuple containing:
    //    - start and end of the source
    //    - start and end of the target (destination)
    //    - length of the range
    //    - the offset from the source to the target (i.e. value you can add to a source value to convert it to a target value)
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

            var end = start + length - 1;

            data.SeedRanges.Add((start, end, length));
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

                long sourceEnd = sourceStart + rangeLength - 1;
                long targetEnd = targetStart + rangeLength - 1;

                map.Vectors.Add((sourceStart, sourceEnd, targetStart, targetEnd, rangeLength, targetStart - sourceStart));
            }
        }

        // default sorting of a tuple will sort primarily by the first item (i.e. sourceStart)
        data.Maps.ForEach(m => m.Vectors.Sort());
        data.Maps.ForEach(m => m.Vectors.Reverse());
        return data;
    }
}