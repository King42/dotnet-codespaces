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
            var normalizedLine = line
                .Replace("one", "1")
                .Replace("two", "2")
                .Replace("three", "3")
                .Replace("four", "4")
                .Replace("five", "5")
                .Replace("six", "6")
                .Replace("seven", "7")
                .Replace("eight", "8")
                .Replace("nine", "9");
            normalizedLine.First(c => int.TryParse(c.ToString(), out int firstDigit) && (answer += firstDigit * 10) != -1);
            normalizedLine.Reverse().First(c => int.TryParse(c.ToString(), out int firstDigit) && (answer += firstDigit) != -1);
        }
        return answer.ToString();
    }

    private static async Task<IEnumerable<string>> GetInputLines(int day, bool useTestData)
    {
        return await File.ReadAllLinesAsync($"input/2023/AdventOfCode_Input_2023_Day{day}{(useTestData ? "_test" : "")}.txt");
    }
}