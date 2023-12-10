namespace AdventOfCode2023.Day05;

public class Day05
{
    public static void Run()
    {
        var input = File.ReadAllLines("Day05/input.txt");
        var parsed = ParseInput(input);
        Part1(parsed);
    }

    private static ParsedInput ParseInput(string[] input)
    {
        var seedLine = input[0];
        var seeds = seedLine
            .Split(":")[1]
            .Trim()
            .Split(" ")
            .Select(long.Parse)
            .ToList();

        var mapLines = input[1..];
        SourceDestinationMap? currentMap = null;
        var maps = new List<SourceDestinationMap?>();

        foreach (var mapLine in mapLines)
        {
            if (string.IsNullOrWhiteSpace(mapLine)) continue;

            if (mapLine.Contains("map"))
            {
                if (currentMap != null) maps.Add(currentMap);

                currentMap = new SourceDestinationMap();
                var sourceDestination = mapLine
                    .Replace(" map:", "")
                    .Trim()
                    .Split("-")
                    .ToArray();

                currentMap.Source = sourceDestination[0];
                currentMap.Destination = sourceDestination[2];
                continue;
            }

            var mapLineParts = mapLine
                .Split(" ")
                .Select(long.Parse)
                .ToArray();

            currentMap.Lines.Add(new SourceDestinationMapLine
            {
                DestinationRangeStart = mapLineParts[0],
                SourceRangeStart = mapLineParts[1],
                RangeLength = mapLineParts[2]
            });
        }

        maps.Add(currentMap);


        return new ParsedInput
        {
            Seeds = seeds,
            Maps = maps
        };
    }

    private static void Part1(ParsedInput input)
    {
        var seeds = input.Seeds;
        var locations = new List<long>();
        foreach (var seed in seeds)
        {
            var location = WalkTree(input.Maps, seed, null);
            locations.Add(location);
        }

        var lowestLocation = locations.Min();
        Console.WriteLine($"Day 05 - Part 1: {lowestLocation}");
    }

    private static long WalkTree(List<SourceDestinationMap?> inputMaps, long i, SourceDestinationMap? map)
    {
        map ??= inputMaps.First(x => x.Source == "seed");
        var mapLine = map.Lines.FirstOrDefault(x => x.SourceRangeStart <= i && x.SourceRangeStart + x.RangeLength > i);
        long location = 0;
        if (mapLine == null) location = i;
        else
            location = mapLine.DestinationRangeStart + (i - mapLine.SourceRangeStart);

        var nextMap = inputMaps.FirstOrDefault(x => x.Source == map.Destination);
        if (nextMap == null) return location;
        return WalkTree(inputMaps, location, nextMap);
    }

    private class ParsedInput
    {
        public List<long> Seeds { get; set; }
        public List<SourceDestinationMap?> Maps { get; set; }
    }

    private class SourceDestinationMapLine
    {
        public long DestinationRangeStart { get; set; }
        public long SourceRangeStart { get; set; }
        public long RangeLength { get; set; }
    }

    private class SourceDestinationMap
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public List<SourceDestinationMapLine> Lines { get; } = new();
    }
}

// destination range start, then start range start, range length.