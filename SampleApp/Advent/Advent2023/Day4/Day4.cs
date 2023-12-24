using System.Text.RegularExpressions;

namespace Advent.Advent2023;

public static class Day4
{
   public const string Name = "Scratchcards";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Day4Part1(input);
        }
        else
        {
            return Day4Part2(input);
        }
    }

    private static string Day4Part1(string[] input)
    {
        var answer = 0D;

        var data = GetData(input);

        foreach (var row in data)
        {
            if (row.numOfMatches > 0)
            {
                answer += Math.Pow(2, row.numOfMatches - 1);
            }
        }
    
        return answer.ToString();
    }

    private static string Day4Part2(string[] input)
    {
        var data = GetData(input);

        int i = 0;
        // create a dictionary mapping card numbers to the number of cards
        // initially there is 1 copy of each card (the original)
        var winners = data.ToDictionary(k => i++, v => 1);

        for (int rowIndex = 0; rowIndex < data.Count; rowIndex++)
        {
            var row = data[rowIndex];
            var numOfCards = winners[rowIndex];
            var numOfMatches = row.numOfMatches;
            // add more copies of the upcoming cards
            for (int offset = 1; offset <= numOfMatches; offset++)
            {
                winners[rowIndex + offset] += numOfCards;
            }
        }

        var answer = winners.Sum(kv => kv.Value);
        return answer.ToString();
    }

    private static readonly Regex Parser = new Regex(@"\w+ +(?<id>\d+):(?: +(?<set1>\d+))+ \|(?: +(?<set2>\d+))+");

    // for each card (line) find the number of matches
    private static List<(int cardNumber, int numOfMatches)> GetData(string[] input)
    {
        var data = new List<(int, int)>();

        foreach (var line in input)
        {
            var match = Parser.Match(line);
            var cardNumber = int.Parse(match.Groups["id"].Value);
            var set1Numbers = match.Groups["set1"].Captures.Select(c => int.Parse(c.Value)).ToList();
            var set2Numbers = match.Groups["set2"].Captures.Select(c => int.Parse(c.Value)).ToList();

            int numOfMatches = 0;

            foreach (var potential in set2Numbers)
            {
                if (set1Numbers.Contains(potential))
                {
                    numOfMatches++;
                }
            }
            data.Add((cardNumber, numOfMatches));
        }

        return data;
    }
}