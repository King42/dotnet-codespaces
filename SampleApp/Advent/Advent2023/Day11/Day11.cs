namespace Advent.Advent2023;

public static class Day11
{
    public static string Name { get; } = "Cosmic Expansion";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return SolvePart1(GetData(input, false));
        }
        else
        {
            return SolvePart2(GetData(input, true));
        }
    }

    private static string SolvePart1((List<(int col, int row)> galaxies, List<int> emptyColumns, List<int> emptyRows) data)
    {
        var answer = 0L;

        foreach (var galaxy1 in data.galaxies)
        {
            foreach (var galaxy2 in data.galaxies)
            {
                if (galaxy1 == galaxy2)
                {
                    continue;
                }

                var leftCol = Math.Min(galaxy1.col, galaxy2.col);
                var rightCol = Math.Max(galaxy1.col, galaxy2.col);
                var width = rightCol - leftCol + data.emptyColumns.Count(c => leftCol < c && c < rightCol);

                var botRow = Math.Min(galaxy1.row, galaxy2.row);
                var topRow = Math.Max(galaxy1.row, galaxy2.row);
                var height = topRow - botRow + data.emptyRows.Count(r => botRow < r && r < topRow);

                answer += width + height;
            }
        }

        return (answer / 2).ToString();
    }

    private static string SolvePart2(object data)
    {
        throw new NotImplementedException();
    }

    private static (List<(int col, int row)> galaxies, List<int> emptyColumns, List<int> emptyRows) GetData(string[] input, bool part2)
    {
        var galaxies = new List<(int col, int row)>();
        var emptyColumns = Enumerable.Range(0, input.First().Length).ToList();
        var emptyRows = Enumerable.Range(0, input.First().Length).ToList();

        for (var row = 0; row < input.Length; row++)
        {
            var line = input[row];

            for (var col = 0; col < line.Length; col++)
            {
                var c = line[col];
                if (c == '#')
                {
                    emptyColumns.Remove(col);
                    emptyRows.Remove(row);
                    galaxies.Add((col, row));
                }
            }
        }

        return (galaxies, emptyColumns, emptyRows);
    }
}