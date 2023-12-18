using Microsoft.VisualBasic;

namespace Advent.Advent2023;

public static class Day8
{
    public static string Name { get; } = "Haunted Wasteland";

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

    private static string Part1((string instructions, Dictionary<string, (string left, string right)> nodes) data)
    {
        var answer = 0L;
        var nextNodeKey = "AAA";

        for (var i = 0; nextNodeKey != "ZZZ"; i++, answer++)
        {
            if (i == data.instructions.Length)
            {
                i -= data.instructions.Length;
            }

            var nextNode = data.nodes[nextNodeKey];
            nextNodeKey = data.instructions[i] == 'L' ? nextNode.left : nextNode.right;
        }

        return answer.ToString();
    }

    private static string Part2((string instructions, Dictionary<string, (string left, string right)> nodes) data)
    {
        throw new NotImplementedException();
    }
    private static (string instructions, Dictionary<string, (string left, string right)> nodes) GetData(string[] input, bool part2)
    {
        var nodes = new Dictionary<string, (string left, string right)>();
        foreach (var line in input.Skip(2))
        {
            var k = line.Replace("(", "").Replace(")", "").Replace(" ", "").Split(new char[] { '=', ',' }, StringSplitOptions.RemoveEmptyEntries);
            nodes.Add(k[0], (k[1], k[2]));
        }

        return (input.First(), nodes);
    }
}