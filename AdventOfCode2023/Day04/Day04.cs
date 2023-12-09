namespace AdventOfCode2023.Day04;

public class Day04
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day04/input.txt");
        Part01(input);
        Part02(input);
    }

    private static void Part02(string[] input)
    {
        var scratchcardLines = input
            .Select(x => new ScratchcardLine(x))
            .ToDictionary(x => x.CardNumber, x => x);


        var totalScratchcards = scratchcardLines.Count;

        var scratchcardNumbersToProcess = scratchcardLines.Keys.ToList();
        foreach (var scratchcardNumber in scratchcardNumbersToProcess)
            totalScratchcards += ProcessScratchcard(scratchcardNumber, scratchcardLines);

        Console.WriteLine($"Day 04 - Part 2: {totalScratchcards}");
    }

    private static int ProcessScratchcard(int scratchcardNumber, Dictionary<int, ScratchcardLine> scratchcardLines)
    {
        var scratchcardLine = scratchcardLines[scratchcardNumber];
        var matches = scratchcardLine.WinningNumbers.Intersect(scratchcardLine.OurNumbers).ToList();

        var scratchCardNumbers = new List<int>();
        for (var i = 0; i < matches.Count; i++)
        {
            var a = scratchcardNumber + i + 1;
            scratchCardNumbers.Add(a);
        }

        var total = matches.Count;
        foreach (var scratchCardNumber in scratchCardNumbers)
            total += ProcessScratchcard(scratchCardNumber, scratchcardLines);

        return total;
    }

    private static void Part01(string[] input)
    {
        var scratchcardLines = input
            .Select(x => new ScratchcardLine(x))
            .ToList();

        var total = 0;

        foreach (var line in scratchcardLines)
        {
            var matches = line.WinningNumbers.Intersect(line.OurNumbers).ToList();
            var points = 0;
            for (var i = 0; i < matches.Count; i++)
                if (i == 0)
                    points = 1;
                else
                    points *= 2;

            total += points;
        }

        Console.WriteLine($"Day 04 - Part 1: {total}");
    }

    private class ScratchcardLine
    {
        public ScratchcardLine(string line)
        {
            var cardNumberSplit = line.Split(":");
            var cardNumber = cardNumberSplit[0]
                .Split(" ")
                .Last(x => !string.IsNullOrWhiteSpace(x));
            CardNumber = int.Parse(cardNumber);

            var numbers = cardNumberSplit[1];
            var winningNumbers = numbers
                .Split("|")[0]
                .Split(" ")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(int.Parse)
                .ToArray();

            var ourNumbers = numbers
                .Split("|")[1]
                .Split(" ")
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(int.Parse)
                .ToArray();

            WinningNumbers = winningNumbers;
            OurNumbers = ourNumbers;
        }

        public int CardNumber { get; }
        public int[] WinningNumbers { get; }
        public int[] OurNumbers { get; }
    }
}