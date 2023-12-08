using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023
{
    public class Day03
    {
        class StarPosition
        {
            public int X { get; set; }
            public int Y { get; set; }

            public override bool Equals(object? obj)
            {
                return obj is StarPosition position &&
                       X == position.X &&
                       Y == position.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }

        class NumberPosition
        {
            public int X { get; set; }
            public int Y { get; set; }
            public string ValueStr { get; set; }
            public int Length => ValueStr.Length;
            public int Value => int.Parse(ValueStr);
            public StarPosition AdjacentStar { get; set; }

            public bool IsValidPart1(List<List<string>> grid)
            {
                var startY = Math.Max(0, Y - 1);
                var endY = Math.Min(grid.Count - 1, Y + 1);

                var startX = Math.Max(0, X - 1);
                var endX = Math.Min(grid[0].Count - 1, X + Length);

                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        var character = grid[y][x];
                        if (Regex.IsMatch(character, @"[^\d\.]"))
                        {
                            return true;
                        }
                    }
                }

                return false;
            }

            public void FindAdjacentStar(List<List<string>> grid)
            {
                var startY = Math.Max(0, Y - 1);
                var endY = Math.Min(grid.Count - 1, Y + 1);

                var startX = Math.Max(0, X - 1);
                var endX = Math.Min(grid[0].Count - 1, X + Length);

                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        var character = grid[y][x];
                        if (Regex.IsMatch(character, @"[*]"))
                        {
                            AdjacentStar = new StarPosition
                            {
                                X = x,
                                Y = y,
                            };
                            return;
                        }
                    }
                }
            }

            public override bool Equals(object? obj)
            {
                return obj is NumberPosition position &&
                       X == position.X &&
                       Y == position.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }

        [Fact]
        public void Part1()
        {
            var grid = File.ReadLines("./inputs/day03/input.txt").ToList().Select(s => s.ToCharArray().Select(c => c.ToString()).ToList()).ToList();
            var numbers = new List<NumberPosition>();

            for (int y = 0; y < grid.Count; y++)
            {
                var number = "";

                for (int x = 0; x < grid[y].Count; x++)
                {
                    if (Regex.IsMatch(grid[y][x], @"\d"))
                    {
                        number += grid[y][x];

                        if (!string.IsNullOrWhiteSpace(number) && x == 139)
                        {
                            numbers.Add(new NumberPosition
                            {
                                X = x - number.Length,
                                Y = y,
                                ValueStr = number,
                            });
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(number) || x == 139)
                        {
                            numbers.Add(new NumberPosition
                            {
                                X = x - number.Length,
                                Y = y,
                                ValueStr = number,
                            });
                        }

                        number = "";
                    }
                }
            }

            var validNumbers = numbers.Where(n => n.IsValidPart1(grid));
            var total = validNumbers.Sum(n => n.Value);

            Assert.Equal(539433, total);
        }

        [Fact]
        public void Part2()
        {
            var grid = File.ReadLines("./inputs/day03/input.txt").ToList().Select(s => s.ToCharArray().Select(c => c.ToString()).ToList()).ToList();
            var numbers = new List<NumberPosition>();

            for (int y = 0; y < grid.Count; y++)
            {
                var number = "";

                for (int x = 0; x < grid[y].Count; x++)
                {
                    if (Regex.IsMatch(grid[y][x], @"\d"))
                    {
                        number += grid[y][x];

                        if (!string.IsNullOrWhiteSpace(number) && x == 139)
                        {
                            numbers.Add(new NumberPosition
                            {
                                X = x - number.Length,
                                Y = y,
                                ValueStr = number,
                            });
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(number) || x == 139)
                        {
                            numbers.Add(new NumberPosition
                            {
                                X = x - number.Length,
                                Y = y,
                                ValueStr = number,
                            });
                        }

                        number = "";
                    }
                }
            }

            var total = 0;

            numbers.ForEach(n => n.FindAdjacentStar(grid));
            var numbersWithAdjacentStars = numbers.Where(n => n.AdjacentStar != null).ToList();

            foreach (var n in numbersWithAdjacentStars)
            {
                foreach (var n2 in numbersWithAdjacentStars)
                {
                    if (!n.Equals(n2) && n.AdjacentStar.Equals(n2.AdjacentStar))
                    {
                        total += n.Value * n2.Value;
                    }
                }
            }

            Assert.Equal(75847567, total / 2);
        }
    }
}
