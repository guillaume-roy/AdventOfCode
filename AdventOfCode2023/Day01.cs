using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023
{
    public class Day01
    {
        [Fact]
        public void Part1()
        {
            var lines = File.ReadLines("./inputs/day01/input.txt").ToList();

            var result = 0;

            foreach (var line in lines)
            {
                var lineNumberStr = "";

                foreach (var character in line)
                {
                    if (int.TryParse(character.ToString(), out var number))
                    {
                        lineNumberStr += $"{character}";
                    }
                }

                if (lineNumberStr.Length == 1)
                {
                    lineNumberStr += lineNumberStr;
                }

                result += int.Parse($"{lineNumberStr[0]}{lineNumberStr[lineNumberStr.Length - 1]}");
            }

            Assert.Equal(55607, result);
        }

        [Fact]
        public void Part2()
        {
            var numbers = new Dictionary<string, string>
            {
                { "1", "1" },
                { "2", "2" },
                { "3", "3" },
                { "4", "4" },
                { "5", "5" },
                { "6", "6" },
                { "7", "7" },
                { "8", "8" },
                { "9", "9" },
                { "one", "1" },
                { "two", "2" },
                { "three", "3" },
                { "four", "4" },
                { "five", "5" },
                { "six", "6" },
                { "seven", "7" },
                { "eight", "8" },
                { "nine", "9" },
            };

            var lines = File.ReadLines("./inputs/day01/input.txt").ToList();

            var result = 0;

            foreach (var line in lines)
            {
                var startStr = Regex.Replace(line, $"{string.Join('|', numbers.Keys.ToList())}", e =>
                {
                    return numbers[e.Value].ToString();
                });

                startStr = Regex.Replace(startStr, @"[^\d]", "");

                var endStr = Regex.Replace(line, $"{string.Join('|', numbers.Keys.ToList())}", e =>
                {
                    return numbers[e.Value].ToString();
                }, RegexOptions.RightToLeft);

                endStr = Regex.Replace(endStr, @"[^\d]", "");


                var res = int.Parse($"{startStr[0]}{endStr[endStr.Length - 1]}");

                result += res;
            }

            Assert.Equal(55291, result);
        }
    }
}
