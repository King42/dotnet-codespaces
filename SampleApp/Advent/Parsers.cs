namespace Advent;

using System.Text.RegularExpressions;

public static class Parsers
{
    private static Regex NumberListParser = new Regex(@"(?: *(?<numbers>\d+) *)+");

    public static List<T> ExtractNumbersFromList<T>(string input) where T : IParsable<T>
    {
        return NumberListParser.Match(input).Groups["numbers"].Captures.Select(c => T.Parse(c.Value, default)).ToList();
    }
}