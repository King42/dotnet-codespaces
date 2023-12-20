namespace Advent.Advent2023;

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

    private static string Part1(List<List<List<int>>> data)
    {
        var answer = 0L;

        foreach (var sequenceDeconstruction in data)
        {
            var nextNum = 0;
            sequenceDeconstruction.Reverse();
            sequenceDeconstruction.ForEach(s => nextNum += s.Last());
            answer += nextNum;
        }

        return answer.ToString();
    }

    private static string Part2(List<List<List<int>>> data)
    {
        var answer = 0L;

        foreach (var sequenceDeconstruction in data)
        {
            var nextNum = 0;
            sequenceDeconstruction.Reverse();
            sequenceDeconstruction.ForEach(s => nextNum = s.First() - nextNum);
            answer += nextNum;
        }

        return answer.ToString();
    }

    private static List<List<List<int>>> GetData(string[] input, bool part2)
    {
        var data = new List<List<List<int>>>();

        foreach (var line in input)
        {
            var sequenceDeconstruction = new List<List<int>>();

            var sequence = Parsers.ExtractNumbersFromList<int>(line);
            while (!sequence.All(n => n == 0))
            {
                sequenceDeconstruction.Add(sequence);
                sequence = DeconstructSequence(sequence);
            }

            data.Add(sequenceDeconstruction);
        }

        return data;
    }

    private static List<int> DeconstructSequence(List<int> sequence)
    {
        var nextSequence = new List<int>();
        for (int i = 0; i < sequence.Count - 1; i++)
        {
            nextSequence.Add(sequence[i + 1] - sequence[i]);
        }
        return nextSequence;
    }
}