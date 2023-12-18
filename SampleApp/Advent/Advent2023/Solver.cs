namespace Advent.Advent2023;

public class Solver
{
    public static async Task<string> GetAnswer(int day, int part, bool useTestData)
    {
        var input = await GetInputLines(day, useTestData);
        switch (day)
        {
            case 1:
                return Day1.GetAnswer(part, input);
            case 2:
                return Day2.GetAnswer(part, input);
            case 3:
                return Day3.GetAnswer(part, input);
            case 4:
                return Day4.GetAnswer(part, input);
            case 5:
                return Day5.GetAnswer(part, input);
            case 6:
                return Day6.GetAnswer(part, input);
            case 7:
                return Day7.GetAnswer(part, input);
            default:
                throw new NotImplementedException();
        }
    }
    
    private static async Task<string[]> GetInputLines(int day, bool useTestData)
    {
        var path = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), 
            $"input/2023/AdventOfCode_Input_2023_Day{day}{(useTestData ? "_test" : "")}.txt");
        return await File.ReadAllLinesAsync(path);
    }
}