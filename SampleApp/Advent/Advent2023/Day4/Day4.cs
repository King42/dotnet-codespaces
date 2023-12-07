namespace Advent.Advent2023;

using System.Text.RegularExpressions;

public class Day4
{
   public string Name { get; } = "Scratchcards";

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
            var winningNumbers = row.set1Numbers;
            var potentialNumbers = row.set2Numbers;
            int numOfWinners = 0;

            foreach (var potential in potentialNumbers)
            {
                if (winningNumbers.Contains(potential))
                {
                    numOfWinners++;
                }
            }

            if (numOfWinners > 0)
            {
                answer += Math.Pow(2, numOfWinners - 1);
            }
        }
    
        return answer.ToString();
    }

    private static string Day4Part2(string[] input)
    {
        var answer = 0;
    
        return answer.ToString();
    }

    private static readonly Regex Parser = new Regex(@"\w+ +(?<id>\d+):(?: +(?<set1>\d+))+ \|(?: +(?<set2>\d+))+");

    private static List<(int cardNumber, List<int> set1Numbers, List<int> set2Numbers)> GetData(string[] input)
    {
        var data = new List<(int cardNumber, List<int> set1Numbers, List<int> set2Numbers)>();

        foreach (var line in input)
        {
            var match = Parser.Match(line);
            var cardNumber = int.Parse(match.Groups["id"].Value);
            var set1Numbers = match.Groups["set1"].Captures.Select(c => int.Parse(c.Value)).ToList();
            var set2Numbers = match.Groups["set2"].Captures.Select(c => int.Parse(c.Value)).ToList();
            data.Add((cardNumber, set1Numbers, set2Numbers));
        }

        return data;
    }
}