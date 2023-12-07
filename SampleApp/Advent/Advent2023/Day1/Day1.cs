namespace Advent.Advent2023;

public class Day1
{
    public string Name { get; } = "Trebuchet?!";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Day1Part1(input);
        }
        else
        {
            return Day1Part2(input);
        }
    }

    private static string Day1Part1(string[] input)
    {
        var answer = 0;

        foreach (var line in input)
        {
            line.First(c => int.TryParse(c.ToString(), out int firstDigit) && (answer += firstDigit * 10) != -1);
            line.Reverse().First(c => int.TryParse(c.ToString(), out int firstDigit) && (answer += firstDigit) != -1);
        }
        return answer.ToString();
    }

    private static string Day1Part2(string[] input)
    {
        var answer = 0;

        foreach (var line in input)
        {
            var normalizedLine = line
                .Replace("one", "one1one")
                .Replace("two", "two2two")
                .Replace("three", "three3three")
                .Replace("four", "four4four")
                .Replace("five", "five5five")
                .Replace("six", "six6six")
                .Replace("seven", "seven7seven")
                .Replace("eight", "eight8eight")
                .Replace("nine", "nine9nine");
            normalizedLine.First(c => int.TryParse(c.ToString(), out int firstDigit) && (answer += firstDigit * 10) != -1);
            normalizedLine.Reverse().First(c => int.TryParse(c.ToString(), out int firstDigit) && (answer += firstDigit) != -1);
        }
        return answer.ToString();
    }
}