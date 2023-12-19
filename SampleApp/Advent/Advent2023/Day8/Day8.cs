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

    // This should solve it via brute force and is reasonably fast as it walks through entire sets of instructions instead of instruction by instruction
    // but it still takes forever as it still takes billions of iterations
    private static string Part2((string instructions, Dictionary<string, (string left, string right)> nodes) data)
    {
        var answer = 0L;
        // For each node, we walk through the set of instructions once to figure out when it hit a node ending in Z and what the final ending node was
        var collapsedNodes = CollapseNodes(data.instructions, data.nodes);

        var nextNodeKeys = data.nodes.Keys.Where(k => k.EndsWith('A')).ToList();
        
        // you can get the length of individual nodes to Z by taking one node at a time
        //var nextNodeKeys = data.nodes.Keys.Where(k => k.EndsWith('A')).Skip(0).Take(1).ToList();

        do
        {
            // if every path ran into a node ending in Z at the same step then we found the answer
            foreach (var zStep in collapsedNodes[nextNodeKeys[0]].zSteps)
            {
                bool foundZStep = true;
                foreach (var nextNodeKey in nextNodeKeys)
                {
                    if (!collapsedNodes[nextNodeKey].zSteps.Contains(zStep))
                    {
                        foundZStep = false;
                        break;
                    }
                }

                if (foundZStep)
                {
                    return (answer + zStep).ToString();
                }
            }

            // move to the next set of nodes
            // it's important to modify the collection in place to make sure you don't create new objects and cause a stack overflow
            for (int i = 0; i < nextNodeKeys.Count; i++)
            {
                nextNodeKeys[i] = collapsedNodes[nextNodeKeys[i]].endNodeKey;
            }

            answer += data.instructions.Length;
        }while (true);
    }

    private static Dictionary<string, (List<int> zSteps, string endNodeKey)> CollapseNodes(string instructions, Dictionary<string, (string left, string right)> nodes)
    {
        var collapsedNodes = new Dictionary<string, (List<int> zSteps, string endNodeKey)>();

        foreach (var startNodeKey in nodes.Keys)
        {
            var steps = 1;
            var nextNodeKey = startNodeKey;
            var zSteps = new List<int>();
            for (var i = 0; i < instructions.Length; i++, steps++)
            {
                var nextNode = nodes[nextNodeKey];
                nextNodeKey = instructions[i] == 'L' ? nextNode.left : nextNode.right;

                if (nextNodeKey.EndsWith('Z'))
                {
                    zSteps.Add(steps);
                }
            }

            collapsedNodes[startNodeKey] = (zSteps, nextNodeKey);
        }

        return collapsedNodes;
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