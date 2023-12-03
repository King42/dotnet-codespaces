public class Advent2023
{
    public static async Task<string> GetAnswer(int day, int part, bool useTestData)
    {
        var input = await GetInputLines(day, useTestData);
        switch (day)
        {
            case 1:
                return await Day1(part, input);
            default:
                throw new NotImplementedException();
        }
    }

    private static async Task<string> Day1(int part, IEnumerable<string> input)
    {
        var name = "Trebuchet?!";

        if (part == 1)
        {
            return await Day1Part1(input);
        }
        else
        {
            return await Day1Part2(input);
        }
    }

    private static async Task<string> Day1Part1(IEnumerable<string> input)
    {
        var answer = 0;

        foreach (var line in input)
        {
            line.First(c => int.TryParse(c.ToString(), out int firstDigit) && (answer += firstDigit * 10) != -1);
            line.Reverse().First(c => int.TryParse(c.ToString(), out int firstDigit) && (answer += firstDigit) != -1);
        }
        return answer.ToString();
    }

    private static async Task<string> Day1Part2(IEnumerable<string> input)
    {
        var answer = 0;

        foreach (var line in input)
        {
            for (int i = 0; i < line.Length; i++)
            {
                if (Enum.TryParse(typeof(DigitWord), line.Substring(i), out object? result))
                {

                }
            }
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

    enum DigitWord
    {
        zero = 0,
        one,
        two,
        three,
        four,
        five,
        six,
        seven,
        eight,
        nine
    }

    private static async Task<IEnumerable<string>> GetInputLines(int day, bool useTestData)
    {
        return await File.ReadAllLinesAsync($"input/2023/AdventOfCode_Input_2023_Day{day}{(useTestData ? "_test" : "")}.txt");
    }
}