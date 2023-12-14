namespace Advent.Advent2023;

public static class Day1
{
    public const string Name = "Trebuchet?!";

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
            // abusing the fact that you can do assignments within conditionals
            // .First(c => <real conditional> && <assignment conditional>)
            // The real conditional comes first, if it evaluates as false the conditional is short-circuited and the assignment conditional isn't evaluated
            // the assignment conditional has the assignment in it and it MUST evaluate to true to break out of the call to First()
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