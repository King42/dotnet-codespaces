namespace Advent.Advent2023;

public class Day3
{
    public string Name { get; } = "Gear Ratios";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Day3Part1(input);
        }
        else
        {
            return Day3Part2(input);
        }
    }

    private static string Day3Part1(string[] input)
    {
        var answer = 0;

        var matrix = Day3_BuildMatrix(input);
        var numOfRows = matrix.GetLength(0);
        var numOfCols = matrix.GetLength(1);

        for (var rowNumber = 0; rowNumber < numOfRows; rowNumber++)
        {
            int partNumber = 0;
            bool isActivated = false;
            int multiplier = 1;
            for (var colNumber = numOfCols - 1; colNumber >= 0; colNumber--)
            {
                var cell = matrix[rowNumber, colNumber];
                if (cell.digit != null)
                {
                    partNumber += cell.digit.Value * multiplier;
                    isActivated |= cell.isActivated;

                    multiplier *= 10;
                }
                else
                {
                    if (isActivated)
                    {
                        answer += partNumber;
                    }
                    partNumber = 0;
                    isActivated = false;
                    multiplier = 1;
                }
            }
            
            if (isActivated)
            {
                answer += partNumber;
            }
        }
        return answer.ToString();
    }

    private static string Day3Part2(string[] input)
    {
        var answer = 0;

        var matrix = Day3_BuildMatrix(input);
        var numOfRows = matrix.GetLength(0);
        var numOfCols = matrix.GetLength(1);

        var gears = new Dictionary<(int x, int y), List<int>>();

        for (var rowNumber = 0; rowNumber < numOfRows; rowNumber++)
        {
            int partNumber = 0;
            var adjacentGears = new List<(int, int)>();
            int multiplier = 1;
            for (var colNumber = numOfCols - 1; colNumber >= 0; colNumber--)
            {
                var cell = matrix[rowNumber, colNumber];
                if (cell.digit != null)
                {
                    partNumber += cell.digit.Value * multiplier;
                    adjacentGears.AddRange(cell.adjacentGears);

                    multiplier *= 10;
                }
                else
                {
                    foreach (var gear in adjacentGears.Distinct())
                    {
                        if (!gears.ContainsKey(gear))
                        {
                            gears[gear] = new List<int>();
                        }
                        gears[gear].Add(partNumber);
                    }

                    partNumber = 0;
                    adjacentGears.Clear();
                    multiplier = 1;
                }
            }
            
            foreach (var gear in adjacentGears.Distinct())
            {
                if (!gears.ContainsKey(gear))
                {
                    gears[gear] = new List<int>();
                }
                gears[gear].Add(partNumber);
            }
        }

        foreach (var gear in gears.Keys)
        {
            var partNumbers = gears[gear];
            if (partNumbers.Count == 2)
            {
                answer += partNumbers[0] * partNumbers[1];
            }
        }

        return answer.ToString();
    }

    private static (int? digit, char? symbol, bool isActivated, List<(int x,int y)> adjacentGears)[,] Day3_BuildMatrix(string[] input)
    {
        var matrix = Day3_InitializeMatrix(input);
        var numOfRows = matrix.GetLength(0);
        var numOfCols = matrix.GetLength(1);

        for (var rowNumber = 0; rowNumber < numOfRows; rowNumber++)
        {
            for (var colNumber = 0; colNumber < numOfCols; colNumber++)
            {
                var item = matrix[rowNumber, colNumber];
                if (item.symbol != null)
                {
                    for (var i = -1; i < 2; i++)
                    {
                        var x = rowNumber + i;
                        if (rowNumber < 0 || rowNumber >= numOfRows) continue;
                        for (var j = -1; j < 2; j++)
                        {
                            var y = colNumber + j;
                            if (colNumber < 0 || colNumber >= numOfCols) continue;

                            matrix[x, y].isActivated = true;
                            if (item.symbol == '*')
                            {
                                matrix[x, y].adjacentGears.Add((rowNumber, colNumber));
                            }
                        }
                    }
                }
            }
        }

        return matrix;
    }

    private static (int? digit, char? symbol, bool isActivated, List<(int, int)> adjacentGears)[,] Day3_InitializeMatrix(string[] input)
    {
        var numOfRows = input.Length;
        var numOfCols = input.First().Length;
        var matrix = new (int?, char?, bool, List<(int, int)>)[numOfRows, numOfCols];

        for (int rowNumber = 0; rowNumber < matrix.GetLength(0); rowNumber++)
        {
            var strRow = input[rowNumber];
            for (int colNumber = 0; colNumber < matrix.GetLength(1); colNumber++)
            {
                var c = strRow[colNumber];
                if (int.TryParse(c.ToString(), out int digit))
                {
                    matrix[rowNumber, colNumber] = (digit, default, default, new List<(int, int)>());
                }
                else if (c == '.')
                {
                    matrix[rowNumber, colNumber] = (default, default, default, new List<(int, int)>());
                }
                else
                {
                    matrix[rowNumber, colNumber] = (default, c, default, new List<(int, int)>());
                }
            }
        }
        return matrix;
    }
}