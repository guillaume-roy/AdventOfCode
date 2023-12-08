using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023
{
    public class Day06
    {
        [Fact]
        public void Part1()
        {
            var lines = File.ReadLines("./inputs/day06/input.txt").ToList();
            var times = lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => int.Parse(x)).ToList();
            var distances = lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1).Select(x => int.Parse(x)).ToList();

            var total = 1;

            for (int i = 0; i < times.Count; i++)
            {
                var ways = 0;

                for (int buttonHoldTime = 0; buttonHoldTime <= times[i]; buttonHoldTime++)
                {
                    var distance = buttonHoldTime * (times[i] - buttonHoldTime);

                    if (distance > distances[i])
                    {
                        ways++;
                    }
                }

                total *= ways;
            }

            Assert.Equal(3317888, total);
        }

        [Fact]
        public void Part2()
        {
            var lines = File.ReadLines("./inputs/day06/input.txt").ToList();
            var time = long.Parse(string.Join("", lines[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)));
            var distance = long.Parse(string.Join("", lines[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Skip(1)));

            var total = 0;

            for (int buttonHoldTime = 0; buttonHoldTime <= time; buttonHoldTime++)
            {
                var currentDistance = buttonHoldTime * (time - buttonHoldTime);

                if (currentDistance > distance)
                {
                    total++;
                }
            }

            Assert.Equal(24655068, total);
        }
    }
}
