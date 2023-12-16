using Microsoft.VisualBasic;

namespace Advent.Advent2023;

public static class Day6
{
    public static string Name { get; } = "Wait For It";

    public static string GetAnswer(int part, string[] input)
    {
        if (part == 1)
        {
            return Day6Part1(GetDataPart1(input));
        }
        else
        {
            return Day6Part2(GetDataPart2(input));
        }
    }

    private static string Day6Part1(List<(int time, int score)> data)
    {
        var answer = 1;

        foreach (var t in data)
        {
            var scores = ScoresForRace(t.time);

            answer *= scores.Count(s => s > t.score);
        }

        return answer.ToString();
    }

    // calculates a list of all possible scores
    private static List<int> ScoresForRace(int time)
    {
        var scores = new List<int>
        {
            0
        };
        
        // using the algorithmic progression to calculate each score from the previous one
        // e.g if time = 7, n * (time - n) = score[n], score[0] = 0 * 7 = 0, score[1] = 1 * 6
        // using the algorithmic progression though, score[n] = score[n - 1] + (time - n) - (n - 1), so score[1] = score[0] + 6 - 0
        // ends up overcomplicated for no benefit though
        var score = 0;
        for (var t = 1; t <= time; t++)
        {
            score += time - 2 * t + 1;
            scores.Add(score);
        }

        return scores;
    }

    private static string Day6Part2((long time, long score) data)
    {
        var minTime = MinTimeToWin(data.time, data.score);
        var answer = data.time - 2 * minTime + 2;
        if (data.time % 2 == 0)
        {
            answer--;
        }

        return answer.ToString();
    }

    // Finds the smallest value for time that is a winning time
    private static long MinTimeToWin(long time, long minScore)
    {
        /* 
        *   makes better use of the algorithmic progression described in ScoresForRace
        *   the amount added to each preceding score decreases by 2 each time
        *   e.g. if time = 7, score[0] = 0
        *   score[1] = score[0] + 6
        *   score[2] = score[1] + 4
        *   ...
        *   score[6] = score[5] + (-4)
        *   score[7] = score[6] + (-6)
        */

        // abuse of a for loop, calculates each score in sequence until it finds a winning score
        long t, addend, score;
        for (t = -1, addend = time - 1, score = 0; ++t <= time / 2 && score < minScore; score += addend, addend -= 2);
        return t;
    }

    private static List<(int time, int score)> GetDataPart1(string[] input)
    {
        var data = new List<(int time, int score)>();

        var timeLine = input[0];
        var scoreLine = input[1];

        var times = Parsers.ExtractNumbersFromList<int>(timeLine.Split(':')[1]);
        var scores = Parsers.ExtractNumbersFromList<int>(scoreLine.Split(':')[1]);

        for (int i = 0; i < times.Count; i++)
        {
            data.Add((times[i], scores[i]));
        }

        return data;
    }

    private static (long time, long score) GetDataPart2(string[] input)
    {
        var timeLine = input[0];
        var scoreLine = input[1];

        var time = long.Parse(timeLine.Split(':')[1].Replace(" ", null));
        var score = long.Parse(scoreLine.Split(':')[1].Replace(" ", null));

        return (time, score);
    }}