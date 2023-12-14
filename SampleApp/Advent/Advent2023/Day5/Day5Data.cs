namespace Advent.Advent2023;

public class Day5Data
{
    public List<long> Seeds { get; set; } = new List<long>();

    public List<(long start, long end, long length)> SeedRanges { get; set; } = new List<(long start, long end, long length)>();

    public List<Day5Map> Maps { get; set; } = new List<Day5Map>();
}

public class Day5Map
{
    public string? SourceItem { get; set; }
    public string? TargetItem { get; set; }

    public List<(long sourceStart, long sourceEnd, long targetStart, long targetEnd, long rangeLength, long offsetToTarget)> Vectors = new List<(long, long, long, long, long, long)>();

    public bool SourceIsInRange(long candidateNumber, long sourceStart, long sourceEnd)
    {
        return sourceStart <= candidateNumber && candidateNumber <= sourceEnd;
    }
}