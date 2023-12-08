using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023
{
    public class Day04
    {
        class Card
        {
            public int Id { get; set; }
        }

        [Fact]
        public void Part1()
        {
            var total = 0;

            var lines = File.ReadLines("./inputs/day04/input.txt").ToList();

            foreach (var line in lines)
            {
                var subTotal = 0;

                var cardContent = line.Split(':', '|');
                var winningNumbers = cardContent[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n));
                var numbers = cardContent[2].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n));

                foreach (var winningNumber in winningNumbers)
                {
                    if (numbers.Contains(winningNumber))
                    {
                        if (subTotal == 0)
                        {
                            subTotal = 1;
                        }
                        else
                        {
                            subTotal *= 2;
                        }
                    }
                }

                total += subTotal;
            }

            Assert.Equal(25004, total);
        }

        [Fact]
        public void Part2()
        {
            var total = 0;

            var lines = File.ReadLines("./inputs/day04/input.txt").ToList();

            var cardProcessorCount = Enumerable.Range(1, lines.Count).ToDictionary(n => n, n => 1);

            foreach (var line in lines)
            {
                var cardContent = line.Split(':', '|');
                var cardId = int.Parse(Regex.Match(cardContent[0], @"Card\s+(\d+)").Groups[1].Value);
                var winningNumbers = cardContent[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n));
                var numbers = cardContent[2].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n));

                var foundWinningNumbers = winningNumbers.Intersect(numbers).Count();

                for (int cardProcessorIndex = 0; cardProcessorIndex < cardProcessorCount[cardId]; cardProcessorIndex++)
                {
                    for (int i = 1; i <= foundWinningNumbers; i++)
                    {
                        cardProcessorCount[cardId + i]++;
                    }
                }
            }

            total = cardProcessorCount.Sum(x => x.Value);

            Assert.Equal(14427616, total);
        }
    }
}
