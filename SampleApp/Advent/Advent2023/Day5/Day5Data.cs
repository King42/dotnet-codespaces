namespace Advent.Advent2023;

public class Day5Data
{
    public List<long> Seeds { get; set; } = new List<long>();

    public List<Day5Map> Maps { get; set; } = new List<Day5Map>();
}

public class Day5Map
{
    public string SourceItem { get; set; }
    public string TargetItem { get; set; }

    public List<(int start, int offset)> Vectors = new List<(int start, int offset)>();
}