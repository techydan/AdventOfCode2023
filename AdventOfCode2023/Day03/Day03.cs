using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day03;

public static class Day03
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day03/input.txt");
        Part01(input);
        Part02(input);
    }

    private static void Part01(string[] input)
    {
        var indexDirectionsYx = new List<(int, int)>
        {
            (-1, -1), (-1, 0), (-1, 1),
            (0, -1), (0, 1),
            (1, -1), (1, 0), (1, 1)
        };
        var validNumbers = new List<int>();

        for (var i = 0; i < input.Length; i++)
        {
            var line = input[i];
            const string regexPattern = @"[0-9]+";
            var matches = Regex.Matches(line, regexPattern);

            foreach (Match match in matches)
            {
                var start = match.Index;
                var length = match.Length;

                var adjacentSymbol = false;

                for (var j = start; j < start + length; j++)
                    foreach (var direction in indexDirectionsYx)
                    {
                        var y = i + direction.Item1;
                        var x = j + direction.Item2;
                        if (y < 0 || y >= input.Length || x < 0 || x >= line.Length) continue;

                        var c = input[y][x];
                        if (IsSymbol(c))
                        {
                            adjacentSymbol = true;
                            break;
                        }
                    }

                if (adjacentSymbol) validNumbers.Add(int.Parse(match.Value));
            }
        }

        Console.WriteLine($"Day 03 - Part 1: {validNumbers.Sum()}");
    }

    private static void Part02(string[] input)
    {
        Console.WriteLine("Day 03 - Part 2: Not Implemented");
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