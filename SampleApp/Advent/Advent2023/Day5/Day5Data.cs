namespace Advent.Advent2023;

public class Day5Data
{
    public List<long> Seeds { get; set; } = new List<long>();

    public List<(long start, long length)> SeedRanges { get; set; } = new List<(long start, long length)>();

    public List<Day5Map> Maps { get; set; } = new List<Day5Map>();
}

public class Day5Map
{
    public string? SourceItem { get; set; }
    public string? TargetItem { get; set; }

    public List<(long sourceStart, long targetStart, long rangeLength, long offsetToTarget)> Vectors = new List<(long, long, long, long)>();

    public bool SourceIsInRange(long candidateNumber, long sourceStart, long rangeLength)
    {
        var sourceEnd = sourceStart + rangeLength - 1;
        return sourceStart <= candidateNumber && candidateNumber <= sourceEnd;
    }
}