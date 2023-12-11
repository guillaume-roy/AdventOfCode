using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023
{
    public class Sequence
    {
        private Lazy<bool> _isZeros => new(() => Values.All(x => x == 0));
        public bool IsZeros => _isZeros.Value;
        public List<long> Values { get; set; } = new List<long>();
        public Sequence SubSequence { get; set; }

        public Sequence(List<long> values) => Values = values;

        public void Derive()
        {
            if (IsZeros)
            {
                return;
            }

            var list = new List<long>();

            for (int i = 1; i < Values.Count; i++)
            {
                list.Add(Values[i] - Values[i - 1]);
            }

            SubSequence = new Sequence(list);
            SubSequence.Derive();
            Values.Add(Values.Last() + SubSequence.Values.Last());
        }

        public void ReverseDerive()
        {
            if (IsZeros)
            {
                return;
            }

            var list = new List<long>();

            for (int i = 1; i < Values.Count; i++)
            {
                list.Add(Values[i] - Values[i - 1]);
            }

            SubSequence = new Sequence(list);
            SubSequence.ReverseDerive();
            Values.Insert(0, Values.First() - SubSequence.Values.First());
        }
    }

    public class Day09
    {
        [Fact]
        public void Part1()
        {
            var lines = File.ReadLines("./inputs/day09/input.txt").ToList().Select(x => x.Split(" ").Select(x => long.Parse(x)).ToList()).ToList();

            var res = 0L;
            foreach (var line in lines)
            {
                var seq = new Sequence(line);
                seq.Derive();
                res += seq.Values.Last();
            }

            Assert.Equal(1980437560, res);
        }

        [Fact]
        public void Part2()
        {
            var lines = File.ReadLines("./inputs/day09/input.txt").ToList().Select(x => x.Split(" ").Select(x => long.Parse(x)).ToList()).ToList();

            var res = 0L;
            foreach (var line in lines)
            {
                var seq = new Sequence(line);
                seq.ReverseDerive();
                res += seq.Values.First();
            }

            Assert.Equal(977, res);
        }
    }
}
