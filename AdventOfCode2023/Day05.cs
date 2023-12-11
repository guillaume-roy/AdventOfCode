using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023
{
    public class Day05
    {
        public class SeedTransformer
        {
            public long Source { get; set; }
            public long Destination { get; set; }
            public long Range { get; set; }

            public SeedTransformer(long source, long destination, long range)
            {
                Source = source;
                Destination = destination;
                Range = range;
            }

            public long GetDestination(long source)
            {
                if (source < Source || source >= (Source + Range))
                {
                    return -1;
                }

                var sourceIndex = source - Source;
                return Destination + sourceIndex;
            }

            public long GetSource(long destination)
            {
                if (destination < Destination || destination >= (Destination + Range))
                {
                    return -1;
                }

                var destinationIndex = destination - Destination;
                return Source + destinationIndex;
            }
        }

        public class Range
        {
            public long Start { get; private set; }
            public long RRange { get; private set; }
            public long End => Start + RRange - 1;

            public Range(long start, long rRange)
            {
                Start = start;
                RRange = rRange;
            }
        }

        [Fact]
        public void Part1()
        {
            var lines = File.ReadLines("./inputs/day05/input.txt").ToList();

            var seeds = lines[0].Split(':', ' ').Where(x => long.TryParse(x, out var _)).Select(x => long.Parse(x)).ToList();

            var seedTransformersGroups = new List<List<SeedTransformer>>
            {
                GetSeedTransformers(lines, 3, 47),
                GetSeedTransformers(lines, 50, 69),
                GetSeedTransformers(lines, 72, 97),
                GetSeedTransformers(lines, 100, 138),
                GetSeedTransformers(lines, 141, 184),
                GetSeedTransformers(lines, 187, 222),
                GetSeedTransformers(lines, 225, 235),
            };

            var lowestLocation = long.MaxValue;

            foreach (var seed in seeds)
            {
                var seedDestination = seed;

                foreach (var seedGroup in seedTransformersGroups)
                {
                    var seedTransformer = seedGroup.FirstOrDefault(s => s.GetDestination(seedDestination) > -1);
                    seedDestination = seedTransformer == null ? seedDestination : seedTransformer.GetDestination(seedDestination);
                }

                lowestLocation = Math.Min(lowestLocation, seedDestination);
            }

            Assert.Equal(278755257, lowestLocation);
        }

        [Fact(Skip = "Too long")]
        public void Part2()
        {
            var lines = File.ReadLines("./inputs/day05/input.txt").ToList();

            var seeds = lines[0].Split(':', ' ')
                .Where(x => long.TryParse(x, out var _))
                .Select(x => long.Parse(x))
                .ToList();

            var seedRanges = new List<Range>();

            for (int i = 0; i < seeds.Count; i += 2)
            {
                var start = seeds[i];
                var count = seeds[i + 1];

                seedRanges.Add(new Range(start, count));
            }

            var seedTransformersGroups = new List<List<SeedTransformer>>
            {
                GetSeedTransformers(lines, 3, 47),
                GetSeedTransformers(lines, 50, 69),
                GetSeedTransformers(lines, 72, 97),
                GetSeedTransformers(lines, 100, 138),
                GetSeedTransformers(lines, 141, 184),
                GetSeedTransformers(lines, 187, 222),
                GetSeedTransformers(lines, 225, 235),
            };
            seedTransformersGroups.Reverse();

            var result = Parallel.For(0, int.MaxValue, (i, state) =>
            {
                if (state.ShouldExitCurrentIteration)
                {
                    if (state.LowestBreakIteration < i)
                        return;
                }

                var seedSource = i;

                foreach (var seedGroup in seedTransformersGroups)
                {
                    var seedTransformer = seedGroup.FirstOrDefault(s => s.GetSource(seedSource) > -1);
                    seedSource = seedTransformer == null ? seedSource : seedTransformer.GetSource(seedSource);
                }

                if (seedRanges.Any(s => s.Start <= seedSource && seedSource <= s.End))
                {
                    state.Break();
                }
            });

            Assert.Equal(26829166, result.LowestBreakIteration);
        }

        private static List<SeedTransformer> GetSeedTransformers(List<string> lines, int startRange, int endRange)
        {
            return Enumerable.Range(startRange, endRange - startRange + 1)
                .Select(x => lines[x]
                    .Split(' ')
                    .Select(n => long.Parse(n))
                    .ToList()
                )
                .ToList()
                .Select(x => new SeedTransformer(x.ElementAt(1), x.ElementAt(0), x.ElementAt(2)))
                .ToList();
        }
    }
}
