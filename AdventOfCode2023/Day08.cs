using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023
{
    public class Day08
    {
        public static Dictionary<string, (string source, string left, string right)> _paths;
        private static List<char> _directions;

        private long Lcm(long a, long b) => a / Gcd(a, b) * b;

        private long Gcd(long a, long b) => b == 0 ? a : Gcd(b, a % b);

        private long FindIterationToZ(string source)
        {
            var iteration = 0L;
            var directionIndex = 0;

            while (!source.EndsWith('Z'))
            {
                var direction = _directions[directionIndex];
                var (_, left, right) = _paths[source];
                source = direction == 'L' ? left : right;

                directionIndex = (directionIndex + 1) % _directions.Count;
                iteration++;

                if (source.EndsWith("Z"))
                {
                    break;
                }
            }
            return iteration;
        }

        [Fact]
        public void Part1()
        {
            var lines = File.ReadLines("./inputs/day08/input.txt").ToList();
            var pathRegex = new Regex(@"([A-Z]+) = \(([A-Z]+), ([A-Z]+)\)");

            _directions = lines[0].ToCharArray().ToList();
            _paths = Enumerable.Range(2, lines.Count - 2)
                .Select(x => lines[x])
                .Select(x => pathRegex.Match(x))
                .Select(m => (source: m.Groups[1].Value, left: m.Groups[2].Value, right: m.Groups[3].Value))
                .GroupBy(x => x.source)
                .ToDictionary(x => x.Key, x => x.Single());

            var iteration = 0;
            var source = "AAA";

            while (source != "ZZZ")
            {
                var direction = _directions[iteration % _directions.Count];
                source = direction == 'L' ? _paths[source].left : _paths[source].right;
                iteration++;
            }

            Assert.Equal(13301, iteration);
        }

        [Fact]
        public void Part2()
        {
            var lines = File.ReadLines("./inputs/day08/input.txt").ToList();
            var pathRegex = new Regex(@"([A-Z|\d]+) = \(([A-Z|\d]+), ([A-Z|\d]+)\)");

            _directions = lines[0].ToCharArray().ToList();
            _paths = Enumerable.Range(2, lines.Count - 2)
                .Select(x => lines[x])
                .Select(x => pathRegex.Match(x))
                .Select(m => (source: m.Groups[1].Value, left: m.Groups[2].Value, right: m.Groups[3].Value))
                .GroupBy(x => x.source)
                .ToDictionary(x => x.Key, x => x.Single());

            var result = _paths.Keys
                .Where(x => x.EndsWith('A'))
                .Select(x => FindIterationToZ(x))
                .Aggregate(Lcm);

            Assert.Equal(7309459565207, result);
        }
    }
}
