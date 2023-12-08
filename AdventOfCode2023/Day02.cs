using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023
{
    public class Day02
    {
        [Fact]
        public void Part1()
        {
            var total = 0;

            var maxRed = 12;
            var maxGreen = 13;
            var maxBlue = 14;

            var lines = File.ReadLines("./inputs/day02/input.txt").ToList();

            foreach (var line in lines)
            {
                var isValid = true;

                var topMatch = Regex.Match(line, @"Game (\d+):(.*)");
                var gameId = topMatch.Groups[1].Value;
                var game = topMatch.Groups[2].Value;

                var sets = game.Split(";");
                foreach (var set in sets)
                {
                    var cubes = set.Split(",");

                    foreach (var cube in cubes)
                    {
                        var info = cube.Trim().Split(" ");
                        var quantity = int.Parse(info[0]);
                        var color = info[1];

                        switch (color)
                        {
                            case "blue":
                                if (quantity > maxBlue)
                                {
                                    isValid &= false;
                                }
                                break;
                            case "red":
                                if (quantity > maxRed)
                                {
                                    isValid &= false;
                                }
                                break;
                            case "green":
                                if (quantity > maxGreen)
                                {
                                    isValid &= false;
                                }
                                break;
                        }
                    }
                }

                if (isValid)
                {
                    total += int.Parse(gameId);
                }
            }

            Assert.Equal(3059, total);
        }

        [Fact]
        public void Part2()
        {
            var total = 0;

            var lines = File.ReadLines("./inputs/day02/input.txt").ToList();

            foreach (var line in lines)
            {
                var lineMaxRed = 0;
                var lineMaxBlue = 0;
                var lineMaxGreen = 0;

                var topMatch = Regex.Match(line, @"Game (\d+):(.*)");
                var gameId = topMatch.Groups[1].Value;
                var game = topMatch.Groups[2].Value;

                var sets = game.Split(";");
                foreach (var set in sets)
                {
                    var cubes = set.Split(",");

                    foreach (var cube in cubes)
                    {
                        var info = cube.Trim().Split(" ");
                        var quantity = int.Parse(info[0]);
                        var color = info[1];

                        switch (color)
                        {
                            case "blue":
                                lineMaxBlue = Math.Max(lineMaxBlue, quantity);
                                break;
                            case "red":
                                lineMaxRed = Math.Max(lineMaxRed, quantity);
                                break;
                            case "green":
                                lineMaxGreen = Math.Max(lineMaxGreen, quantity);
                                break;
                        }
                    }
                }

                total += lineMaxBlue * lineMaxRed * lineMaxGreen;
            }

            Assert.Equal(65371, total);
        }
    }
}
