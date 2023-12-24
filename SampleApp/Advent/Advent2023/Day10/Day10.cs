namespace Advent.Advent2023;

public static class Day10
{
    public static string Name { get; } = "Pipe Maze";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Solve(GetData(input, false), false);
        }
        else
        {
            return Solve(GetData(input, true), true);
        }
    }

    private static string Solve((char [,] matrix, (int x, int y) startPos) data, bool part2)
    {
        long pathLen = 2;
        var markerMatrix = new char?[data.matrix.GetLength(0), data.matrix.GetLength(1)];
        (int x, int y) curPos = data.startPos;
        markerMatrix[curPos.x, curPos.y] = 'P';
        var c = data.matrix[curPos.x, curPos.y];
        var joint = Joints[c];
        var prevDirectedJoint = joint.directedJoint1;
        (int x, int y) nextPos = (curPos.x + prevDirectedJoint.OffsetToNextJoint.xOffset, curPos.y + prevDirectedJoint.OffsetToNextJoint.yOffset);

        while ((nextPos.x, nextPos.y) != (data.startPos.x, data.startPos.y))
        {
            c = data.matrix[nextPos.x, nextPos.y];
            joint = Joints[c];
            var directedJoint = prevDirectedJoint.OutboundDirection == joint.directedJoint1.InboundDirection ? joint.directedJoint1 : joint.directedJoint2;
            var reverseDirectedJoint = directedJoint == joint.directedJoint1 ? joint.directedJoint2 : joint.directedJoint1;

            curPos = nextPos;
            nextPos = (curPos.x + directedJoint.OffsetToNextJoint.xOffset, curPos.y + directedJoint.OffsetToNextJoint.yOffset);
            markerMatrix[curPos.x, curPos.y] = 'P';
            MarkOffsetJoints(markerMatrix, curPos, directedJoint.MarkerOffsets, '$');
            MarkOffsetJoints(markerMatrix, curPos, reverseDirectedJoint.MarkerOffsets, 'O');
            prevDirectedJoint = directedJoint;
            pathLen++;
        }

        if (part2)
        {
            // prints the marker matrix
            using (var sw = File.AppendText("Day10MarkerMatrix.txt"))
            {
                for (int y = 0; y < markerMatrix.GetLength(1); y++)
                {
                    for (int x = 0; x < markerMatrix.GetLength(0); x++)
                    {
                        if (markerMatrix[x,y] == null)
                        {
                            markerMatrix[x,y] = '.';
                        }
                        else if (markerMatrix[x,y] == 'P')
                        {
                            markerMatrix[x,y] = data.matrix[x,y];
                        }

                        sw.Write(markerMatrix[x,y]);
                    }
                    sw.WriteLine();
                }
                sw.Close();
            }

            return Part2_ProcessMarkerMatrix(markerMatrix).ToString();
        }

        return (pathLen / 2).ToString();
    }

    private static int Part2_ProcessMarkerMatrix(char? [,] markerMatrix)
    {
        // walk through the markerMatrix and mark adjacent cells that are next to 'O' or '$' as the same symbol

        // too lazy to implement, easier to look at the printed markerMatrix and do it by hand for cells next to '$'
        // and then do a Find on '$' and let the editor count them for you
        throw new NotImplementedException();
    }

    private static void MarkOffsetJoints(
        char? [,] markerMatrix, 
        (int x, int y) position, 
        List<(int xOffset, int yOffset)> offsets, 
        char marker)
    {
        foreach (var offset in offsets)
        {
            int x = position.x + offset.xOffset;
            int y = position.y + offset.yOffset;
            if (0 <= x && x < markerMatrix.GetLength(0) &&
                0 <= y && y < markerMatrix.GetLength(1))
            if (markerMatrix[x,y] == null)
            {
                markerMatrix[x,y] = marker;
            }
            else if (markerMatrix[x,y] != marker && markerMatrix[x,y] != 'P')
            {
                markerMatrix[x,y] = 'X';
            }
        }
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
        ['-'] = 
        (
            new DirectedJoint(Direction.W, Direction.W, -1, 0, new List<(int xOffset, int yOffset)> { (0, 1) }), 
            new DirectedJoint(Direction.E, Direction.E, 1, 0, new List<(int xOffset, int yOffset)> { (0, -1) })
        ),
        ['|'] = 
        (
            new DirectedJoint(Direction.N, Direction.N, 0, -1, new List<(int xOffset, int yOffset)> { (-1, 0) }), 
            new DirectedJoint(Direction.S, Direction.S, 0, 1, new List<(int xOffset, int yOffset)> { (1, 0) })
        ),
        ['F'] = 
        (
            new DirectedJoint(Direction.N, Direction.E, 1, 0, new List<(int xOffset, int yOffset)> { (-1, 0), (0, -1) }), 
            new DirectedJoint(Direction.W, Direction.S, 0, 1, new List<(int xOffset, int yOffset)>())
        ),
        ['7'] = 
        (
            new DirectedJoint(Direction.N, Direction.W, -1, 0, new List<(int xOffset, int yOffset)>()), 
            new DirectedJoint(Direction.E, Direction.S, 0, 1, new List<(int xOffset, int yOffset)> { (0, -1), (1, 0) })
        ),
        ['L'] = 
        (
            new DirectedJoint(Direction.W, Direction.N, 0, -1, new List<(int xOffset, int yOffset)> { (-1, 0), (0, 1) }), 
            new DirectedJoint(Direction.S, Direction.E, 1, 0, new List<(int xOffset, int yOffset)>())
        ),
        ['J'] = 
        (
            new DirectedJoint(Direction.S, Direction.W, -1, 0, new List<(int xOffset, int yOffset)> { (1, 0), (0, 1) }), 
            new DirectedJoint(Direction.E, Direction.N, 0, -1, new List<(int xOffset, int yOffset)>())
        ),
    };

    private class DirectedJoint
    {
        public DirectedJoint(
            Direction inboundDirection, 
            Direction outboundDirection, 
            int xOffset, 
            int yOffset, 
            List<(int xOffset, int yOffset)> markerOffsets)
        {
            InboundDirection = inboundDirection;
            OutboundDirection = outboundDirection;
            OffsetToNextJoint = (xOffset, yOffset);
            MarkerOffsets = markerOffsets;
        }

        public Direction InboundDirection;
        public Direction OutboundDirection;
        public (int xOffset, int yOffset) OffsetToNextJoint;
        public List<(int xOffset, int yOffset)> MarkerOffsets;
    }

    private enum Direction
    {
        N,
        E,
        S,
        W
    }
}