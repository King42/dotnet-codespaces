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
        var prevDirectedJoint = joint.directedJoint1;
        (int x, int y) nextPos = (curPos.x + prevDirectedJoint.OffsetToNextJoint.xOffset, curPos.y + prevDirectedJoint.OffsetToNextJoint.yOffset);

        while ((nextPos.x, nextPos.y) != (data.startPos.x, data.startPos.y))
        {
            c = data.matrix[nextPos.x, nextPos.y];
            joint = Joints[c];
            var directedJoint = prevDirectedJoint.OutboundDirection == joint.directedJoint1.InboundDirection ? joint.directedJoint1 : joint.directedJoint2;

            curPos = nextPos;
            nextPos = (curPos.x + directedJoint.OffsetToNextJoint.xOffset, curPos.y + directedJoint.OffsetToNextJoint.yOffset);
            prevDirectedJoint = directedJoint;
            pathLen++;
        }

        return (pathLen / 2).ToString();
    }

    private static string Part2((char [,] matrix, (int x, int y) startPos) data)
    {
        var markerMatrix = new char[data.matrix.GetLength(0), data.matrix.GetLength(1)];
        (int x, int y) curPos = data.startPos;
        var c = data.matrix[curPos.x, curPos.y];
        var joint = Joints[c];
        var prevDirectedJoint = joint.directedJoint1;
        (int x, int y) nextPos = (curPos.x + prevDirectedJoint.OffsetToNextJoint.xOffset, curPos.y + prevDirectedJoint.OffsetToNextJoint.yOffset);

        while ((nextPos.x, nextPos.y) != (data.startPos.x, data.startPos.y))
        {
            c = data.matrix[nextPos.x, nextPos.y];
            joint = Joints[c];
            var directedJoint = prevDirectedJoint.OutboundDirection == joint.directedJoint1.InboundDirection ? joint.directedJoint1 : joint.directedJoint2;

            curPos = nextPos;
            nextPos = (curPos.x + directedJoint.OffsetToNextJoint.xOffset, curPos.y + directedJoint.OffsetToNextJoint.yOffset);
            prevDirectedJoint = directedJoint;
        }

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

    private static readonly Dictionary<char, (DirectedJoint directedJoint1, DirectedJoint directedJoint2)> Joints = 
        new Dictionary<char,(DirectedJoint directedJoint1, DirectedJoint directedJoint2)>() {
        ['-'] = (new DirectedJoint(Direction.W, Direction.W, -1, 0), new DirectedJoint(Direction.E, Direction.E, 1, 0)),
        ['|'] = (new DirectedJoint(Direction.N, Direction.N, 0, -1), new DirectedJoint(Direction.S, Direction.S, 0, 1)),
        ['F'] = (new DirectedJoint(Direction.N, Direction.E, 1, 0), new DirectedJoint(Direction.W, Direction.S, 0, 1)),
        ['7'] = (new DirectedJoint(Direction.N, Direction.W, -1, 0), new DirectedJoint(Direction.E, Direction.S, 0, 1)),
        ['L'] = (new DirectedJoint(Direction.W, Direction.N, 0, -1), new DirectedJoint(Direction.S, Direction.E, 1, 0)),
        ['J'] = (new DirectedJoint(Direction.S, Direction.W, -1, 0), new DirectedJoint(Direction.E, Direction.N, 0, -1)),
    };

    private struct DirectedJoint
    {
        public DirectedJoint(Direction inboundDirection, Direction outboundDirection, int xOffset, int yOffset)
        {
            InboundDirection = inboundDirection;
            OutboundDirection = outboundDirection;
            OffsetToNextJoint = (xOffset, yOffset);
        }

        public Direction InboundDirection;
        public Direction OutboundDirection;
        public (int xOffset, int yOffset) OffsetToNextJoint;
    }

    private enum Direction
    {
        N,
        E,
        S,
        W
    }
}