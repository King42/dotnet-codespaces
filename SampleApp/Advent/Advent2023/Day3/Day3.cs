namespace Advent.Advent2023;

public static class Day3
{
    public const string Name = "Gear Ratios";

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

            // for each row walk backwards through the columns to build the part number digit by digit
            // and also record if any of the cells were activated (adjacent to a symbol)
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
                    // after we come to a cell that isn't a digit we've reached the end of the part number
                    if (isActivated)
                    {
                        answer += partNumber;
                    }

                    // reset the variables to continue scanning for more part numbers
                    partNumber = 0;
                    isActivated = false;
                    multiplier = 1;
                }
            }
            
            // if the part number starts at the left end of the row then the for loop will exit without processing that part number
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

    // Walk through the matrix and for every cell (tuple) flag it as activated if it's next to a symbol as well as recording which gears are adjacent to it
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
                    // if we find a symbol mark every adjacent cell as activated
                    for (var i = -1; i < 2; i++)
                    {
                        var x = rowNumber + i;
                        if (rowNumber < 0 || rowNumber >= numOfRows) continue;
                        for (var j = -1; j < 2; j++)
                        {
                            var y = colNumber + j;
                            if (colNumber < 0 || colNumber >= numOfCols) continue;

                            matrix[x, y].isActivated = true;
                            // if it's a gear also add it to the list of gears adjacent to that cell
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

    // Initialize a matrix of tuples
    // Effectively we're just differentiating cells as digits, symbols, or empty (periods) here
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