namespace Advent.Advent2023;

public static class Day2
{
    public const string Name = "Cube Conundrum";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Day2Part1(input);
        }
        else
        {
            return Day2Part2(input);
        }
    }

    private static string Day2Part1(string[] input)
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

    private static string Day2Part2(string[] input)
    {
        var answer = 0;

        var allPulls = new List<List<(int red, int green, int blue)>>();
        foreach (var line in input)
        {
            allPulls.Add(Day2GetPullsForGame(line.Substring(line.IndexOf(':') + 1)));
        }

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

            answer += red * green * blue;

            game++;
        }
        return answer.ToString();
    }

    private static List<(int red, int green, int blue)> Day2GetPullsForGame(string line)
    {
        var pulls = new List<(int red, int green, int blue)>();

        foreach (var strDice in line.Trim().Split(';', StringSplitOptions.RemoveEmptyEntries))
        {
            int red = 0, green = 0, blue = 0;
            foreach (var strDie in strDice.Trim().Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                var trimmedStrDie = strDie.Trim();
                var indexOfSpace = trimmedStrDie.IndexOf(' ');
                var val = int.Parse(trimmedStrDie.Substring(0, indexOfSpace));
                var strColor = trimmedStrDie.Substring(indexOfSpace + 1).Trim();
                switch (strColor)
                {
                    case "red":
                        red = val;
                        break;
                    case "green":
                        green = val;
                        break;
                    case "blue":
                        blue = val;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
            //Console.WriteLine($"{red} red, {green} green, {blue} blue");
            pulls.Add((red, green, blue));
        }

        return pulls;
    }
}