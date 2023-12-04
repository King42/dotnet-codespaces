public class Advent2023
{
    public static async Task<string> GetAnswer(int day, int part, bool useTestData)
    {
        var input = await GetInputLines(day, useTestData);
        switch (day)
        {
            case 1:
                return await Day1(part, input);
            case 2:
                return await Day2(part, input);
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

    private static async Task<string> Day2(int part, IEnumerable<string> input)
    {
        var name = "Trebuchet?!";

        if (part == 1)
        {
            return await Day2Part1(input);
        }
        else
        {
            return await Day2Part2(input);
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

    private static async Task<string> Day2Part1(IEnumerable<string> input)
    {
        var answer = 0;

        var allPulls = new List<List<(int red, int green, int blue)>>();
        foreach (var line in input)
        {
            allPulls.Add(Day2GetPullsForGame(line.Substring(line.IndexOf(':') + 1)));
        }

        int maxRed = 12, maxGreen = 13, maxBlue = 14;
        int game = 1;
        foreach (var pulls in allPulls)
        {
            int red = 0, green = 0, blue = 0;
            foreach (var pull in pulls)
            {
                red = Math.Max(red, pull.red);
                green = Math.Max(green, pull.green);
                blue = Math.Max(blue, pull.blue);
            }

            if (red <= maxRed && green <= maxGreen && blue <= maxBlue)
            {
                answer += game;
            }

            game++;
        }
        return answer.ToString();
    }

    private static List<(int red, int green, int blue)> Day2GetPullsForGame(string line)
    {
        var pulls = new List<(int red, int green, int blue)>();

        int red = 0, green = 0, blue = 0;
        foreach (var strDice in line.Trim().Split(';', StringSplitOptions.RemoveEmptyEntries))
        {
            foreach (var strDie in strDice.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var trimmedStrDie = strDie.Trim();
                var indexOfSpace = trimmedStrDie.IndexOf(' ');
                var val = int.Parse(trimmedStrDie.Substring(0, indexOfSpace));
                var strColor = trimmedStrDie.Substring(indexOfSpace + 1).Trim();
                switch (strColor)
                {
                    case "red":
                        red += val;
                        break;
                    case "green":
                        green += val;
                        break;
                    case "blue":
                        blue += val;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
        pulls.Add((red, green, blue));

        return pulls;
    }

    private static async Task<string> Day2Part2(IEnumerable<string> input)
    {
        var answer = 0;

        foreach (var line in input)
        {
        }
        return answer.ToString();
    }

    private static async Task<IEnumerable<string>> GetInputLines(int day, bool useTestData)
    {
        return await File.ReadAllLinesAsync($"input/2023/AdventOfCode_Input_2023_Day{day}{(useTestData ? "_test" : "")}.txt");
    }
}