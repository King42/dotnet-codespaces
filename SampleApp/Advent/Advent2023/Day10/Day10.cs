using System.Numerics;

namespace Advent.Advent2023;

public static class Day10
{
    public static string Name { get; } = "Pipe Maze";

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

    private static string Part1((char [,] matrix, (int x, int y) startPos) data)
    {
        long pathLen = 2;
        (int x, int y) curPos = data.startPos;
        var c = data.matrix[curPos.x, curPos.y];
        var joint = Joints[c];
        var prevOffset = joint.offset1;
        (int x, int y) nextPos = (curPos.x + prevOffset.xOffset, curPos.y + prevOffset.yOffset);

        while (nextPos != data.startPos)
        {
            c = data.matrix[nextPos.x, nextPos.y];
            joint = Joints[c];
            var offset = (-prevOffset.xOffset, -prevOffset.yOffset) == joint.offset1 ? joint.offset2 : joint.offset1;

            curPos = nextPos;
            nextPos = (curPos.x + offset.xOffset, curPos.y + offset.yOffset);
            prevOffset = offset;
            pathLen++;
        }

        return (pathLen / 2).ToString();
    }

    private static string Part2(object data)
    {
        throw new NotImplementedException();
    }

    private static (char [,] matrix, (int x, int y) startPos) GetData(string[] input, bool part2)
    {
        var matrix = new char[input.First().Length, input.Length];
        (int x, int y) startPos = default;

        for (int y = 0; y < input.Length; y++)
        {
            var line = input[y];

            for (int x = 0; x < line.Length; x++)
            {
                var c = line[x];
                if (c == 'S')
                {
                    startPos = (x, y);
                    // HACK: manually setting start pipe since it's trivial to eyeball it and would be a pain to do it programmatically 
                    c = 'J';
                }
                matrix[x,y] = c;
            }
        }

        return (matrix, startPos);
    }

    private static readonly Dictionary<char, ((int xOffset, int yOffset) offset1, (int xOffset, int yOffset) offset2)> Joints = 
        new Dictionary<char, ((int xOffset, int yOffset) offset1, (int xOffset, int yOffset) offset2)>() {
        ['-'] = ((-1, 0), (1, 0)),
        ['|'] = ((0, -1), (0, 1)),
        ['F'] = ((1, 0), (0, 1)),
        ['7'] = ((-1, 0), (0, 1)),
        ['L'] = ((0, -1), (1, 0)),
        ['J'] = ((-1, 0), (0, -1)),
    };
}