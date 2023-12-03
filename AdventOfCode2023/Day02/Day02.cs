namespace AdventOfCode2023.Day02;

public static class Day02
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day02/input.txt");
        PartOne(input);
        PartTwo(input);
    }

    private static void PartOne(string[] input)
    {
        var successfulGameIds = new List<int>();
        
        foreach (var line in input)
        {
            var gameId = int.Parse(line
                .Split(" ")[1]
                .Replace(":", ""));

            var roundsString = line.Split(":")[1];
            var rounds = roundsString.Split(";").Select(x => x.Trim()).ToArray();

            var validRound = true;

            foreach (var round in rounds)
            {
                if (!validRound)
                {
                    break;
                }
                
                var redCubes = 12;
                var greenCubes = 13;
                var blueCubes = 14;
                
                var picks = round.Split(",")
                    .Select(x => x.Trim())
                    .Select(PickParser)
                    .ToArray();
                foreach (var pick in picks)
                {
                    switch (pick.CubeCubeColor)
                    {
                        case CubeColor.Red:
                            redCubes -= pick.NumberOfCubes;
                            break;
                        case CubeColor.Green:
                            greenCubes -= pick.NumberOfCubes;
                            break;
                        case CubeColor.Blue:
                            blueCubes -= pick.NumberOfCubes;
                            break;
                    }
                }

                if (redCubes < 0 || greenCubes < 0 || blueCubes < 0)
                {
                    validRound = false;
                }
            }

            if (validRound)
            {
                successfulGameIds.Add(gameId);
            }
        }
        
        Console.WriteLine($"Day 01 - Part 1: {successfulGameIds.Sum()}");
    }

    private static void PartTwo(string[] input)
    {
        var total = 0;
        
        foreach (var line in input)
        {
            var roundsString = line.Split(":")[1];
            var rounds = roundsString.Split(";").Select(x => x.Trim()).ToArray();
            var allPicks = rounds.SelectMany(x => x.Split(",").Select(y => y.Trim())).Select(PickParser).ToArray();
            var greatestBlue = allPicks.Where(x => x.CubeCubeColor == CubeColor.Blue).Max(x => x.NumberOfCubes);
            var greatestGreen = allPicks.Where(x => x.CubeCubeColor == CubeColor.Green).Max(x => x.NumberOfCubes);
            var greatestRed = allPicks.Where(x => x.CubeCubeColor == CubeColor.Red).Max(x => x.NumberOfCubes);
            
            var totalForGame = greatestBlue * greatestGreen * greatestRed;
            total += totalForGame;
        }
        
        Console.WriteLine($"Day 01 - Part 2: {total}");
    }

    private enum CubeColor
    {
        Red,
        Green,
        Blue
    }

    private static CubeColor GetColor(string color) =>
        color switch
        {
            "red" => CubeColor.Red,
            "green" => CubeColor.Green,
            "blue" => CubeColor.Blue,
            _ => throw new Exception("Invalid color")
        };

    private class Pick
    {
        
        public int NumberOfCubes { get; set; }
        public CubeColor CubeCubeColor { get; set; }

        internal Pick(int numberOfCubes, CubeColor cubeCubeColor)
        {
            NumberOfCubes = numberOfCubes;
            CubeCubeColor = cubeCubeColor;
        }
    }

    private static Pick PickParser(string pick)
    {
        var parts = pick.Split(" ");
        var number = int.Parse(parts[0]);
        var color = GetColor(parts[1]);
        return new Pick(number, color);

    }
}