using System.Text.RegularExpressions;

namespace AdventOfCode2023.Day01;

public static class Day01
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day01/input.txt");
        PartOne(input);
        PartTwo(input);
    }

    static void PartOne(string[] input)
    {
        var total = 0;

        foreach (var line in input)
        {
            var firstNumber = line.First(char.IsDigit);
            var lastNumber = line.Last(char.IsDigit);

            var combined = firstNumber.ToString() + lastNumber.ToString();
            var combinedInt = int.Parse(combined); 
            total += combinedInt;
        }

        Console.WriteLine($"Day 01 - Part 1: {total}");
    }
    
    
    static int Parser(string input)
    {
        return input switch
        {
            "one" => 1,
            "two" => 2,
            "three" => 3,
            "four" => 4,
            "five" => 5,
            "six" => 6,
            "seven" => 7,
            "eight" => 8,
            "nine" => 9,
            _ => int.Parse(input)
        };
    }
    
    static void PartTwo(string[] input)
    {
        var total = 0;
        const string pattern = @"(?=(one|two|three|four|five|six|seven|eight|nine|\d))";
        
        foreach (var line in input)
        {
            var matches = Regex.Matches(line, pattern);
            var values = matches
                .Select(x => x.Groups[1].Value)
                .Select(Parser)
                .ToList();
            
            var combined = values.First() * 10 + values.Last();
    
            total += Convert.ToInt32(combined);
        }
        
        Console.WriteLine($"Day 01 - Part 2: {total}");
    }

}
