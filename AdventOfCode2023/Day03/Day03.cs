using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day03;

public static class Day03
{
    private static List<(int, int)> _indexDirectionsYx = null!;

    public static void Run()
    {
        var input = File.ReadAllLines("Day03/input.txt");

        _indexDirectionsYx = new List<(int, int)>
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1), (0, 1),
            (1, -1), (1, 0), (1, 1)
        };

        Part01(input);
        Part02(input);
    }

    private static void Part01(string[] input)
    {
        var validNumbers = new List<int>();

        for (var i = 0; i < input.Length; i++)
        {
            var line = input[i];
            const string regexPattern = @"[0-9]+";
            var matches = Regex.Matches(line, regexPattern);

            foreach (Match match in matches)
            {
                var adjacentSymbol = IsMatchAdjacentSymbol(match, input, line, i);

                if (adjacentSymbol) validNumbers.Add(int.Parse(match.Value));
            }
        }

        Console.WriteLine($"Day 03 - Part 1: {validNumbers.Sum()}");
    }

    private static bool IsMatchAdjacentSymbol(Match match, string[] input, string line, int lineIndex)
    {
        var start = match.Index;
        var length = match.Length;

        var adjacentSymbol = false;

        for (var j = start; j < start + length; j++)
            foreach (var direction in _indexDirectionsYx)
            {
                var y = lineIndex + direction.Item1;
                var x = j + direction.Item2;
                if (y < 0 || y >= input.Length || x < 0 || x >= line.Length) continue;

                var c = input[y][x];
                if (IsSymbol(c))
                {
                    adjacentSymbol = true;
                    break;
                }
            }

        return adjacentSymbol;
    }

    private static void Part02(string[] input)
    {
        var validNumbers = new List<int>();

        const string numberRegexPattern = @"[0-9]+";
        const string gearRegexPattern = @"[*]";

        var allGearMatches = new List<(int, Match)>();
        var allNumberMatches = new List<(int, Match)>();

        for (var i = 0; i < input.Length; i++)
        {
            var line = input[i];
            var numberMatches = Regex.Matches(line, numberRegexPattern);
            var gearMatches = Regex.Matches(line, gearRegexPattern);

            foreach (Match match in numberMatches) allNumberMatches.Add((i, match));

            foreach (Match match in gearMatches) allGearMatches.Add((i, match));
        }

        foreach (var gearMatch in allGearMatches)
        {
            var lineIndex = gearMatch.Item1;
            var x = gearMatch.Item2.Index;

            var adjacentNumbers = new List<Match>();
            foreach (var direction in _indexDirectionsYx)
            {
                var yIndex = lineIndex + direction.Item1;
                var xIndex = x + direction.Item2;
                if (yIndex < 0 || yIndex >= input.Length || xIndex < 0 || xIndex >= input[yIndex].Length) continue;

                var c = input[yIndex][xIndex];
                if (!IsNumber(c)) continue;

                var lineNumbers = allNumberMatches.Where(nm => nm.Item1 == yIndex).ToList();
                foreach (var (_, match) in lineNumbers)
                {
                    var start = match.Index;
                    var length = match.Length;

                    if (xIndex >= start && xIndex < start + length)
                    {
                        if (!adjacentNumbers.Contains(match)) adjacentNumbers.Add(match);
                        break;
                    }
                }
            }

            if (adjacentNumbers.Count == 2)
            {
                var firstNumber = int.Parse(adjacentNumbers[0].Value);
                var secondNumber = int.Parse(adjacentNumbers[1].Value);
                var combined = firstNumber * secondNumber;
                validNumbers.Add(combined);
            }
        }

        var sum = validNumbers.Sum();

        Console.WriteLine($"Day 03 - Part 2: {sum}");
    }

    private static bool IsNumber(char c)
    {
        return char.IsNumber(c);
    }

    private static bool IsSymbol(char c)
    {
        return !IsNumber(c) && c != '.';
    }
}