using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AdventOfCode2023
{
    public class Day10
    {
        private static List<List<Coordinate>> _coordinatesPart1 = new();
        private static readonly ConcurrentBag<long> _pointsPart1 = new();

        class Coordinate
        {
            public int X { get; set; }
            public int Y { get; set; }
            public char Value { get; set; }

            public Coordinate(int x, int y, char value)
            {
                X = x;
                Y = y;
                Value = value;
            }

            public void Move(long points, List<Coordinate> visited, string from)
            {
                if (Value == 'S')
                {
                    if (visited.Count == 0)
                    {
                        if (CanGoUp(visited, out var top))
                        {
                            top.Move(points + 1, visited, "down");
                        }

                        if (CanGoDown(visited, out var down))
                        {
                            down.Move(points + 1, visited, "top");
                        }

                        if (CanGoLeft(visited, out var left))
                        {
                            left.Move(points + 1, visited, "right");
                        }

                        if (CanGoRight(visited, out var right))
                        {
                            right.Move(points + 1, visited, "left");
                        }
                    }
                    else
                    {
                        // Finish
                        _pointsPart1.Add(points);
                        return;
                    }
                }
                else
                {
                    visited.Add(this);
                    switch (Value)
                    {
                        case '|':
                            if (from == "top" && CanGoDown(visited, out var down))
                            {
                                down.Move(points + 1, visited, "top");
                            }
                            else if (from == "down" && CanGoUp(visited, out var top))
                            {
                                top.Move(points + 1, visited, "down");
                            }
                            break;
                        case '-':
                            if (from == "left" && CanGoRight(visited, out var right))
                            {
                                right.Move(points + 1, visited, "left");
                            }
                            else if (from == "right" && CanGoLeft(visited, out var left))
                            {
                                left.Move(points + 1, visited, "right");
                            }
                            break;
                        case 'L':
                            if (from == "top" && CanGoRight(visited, out var right1))
                            {
                                right1.Move(points + 1, visited, "left");
                            }
                            else if (from == "right" && CanGoUp(visited, out var top1))
                            {
                                top1.Move(points + 1, visited, "down");
                            }
                            break;
                        case 'J':
                            if (from == "top" && CanGoLeft(visited, out var left1))
                            {
                                left1.Move(points + 1, visited, "right");
                            }
                            else if (from == "left" && CanGoUp(visited, out var top2))
                            {
                                top2.Move(points + 1, visited, "down");
                            }
                            break;
                        case '7':
                            if (from == "left" && CanGoDown(visited, out var down1))
                            {
                                down1.Move(points + 1, visited, "top");
                            }
                            else if (from == "down" && CanGoLeft(visited, out var left2))
                            {
                                left2.Move(points + 1, visited, "right");
                            }
                            break;
                        case 'F':
                            if (from == "right" && CanGoDown(visited, out var down2))
                            {
                                down2.Move(points + 1, visited, "top");
                            }
                            else if (from == "down" && CanGoRight(visited, out var right2))
                            {
                                right2.Move(points + 1, visited, "left");
                            }
                            break;
                        case '.':
                            break;
                    }
                }
                //throw new Exception("Pas normal lol");
            }

            private Coordinate GetTop()
            {
                return _coordinatesPart1[Math.Max(0, Y - 1)][X];
            }

            private Coordinate GetDown()
            {
                return _coordinatesPart1[Math.Min(_coordinatesPart1.Count - 1, Y + 1)][X];
            }

            private Coordinate GetLeft()
            {
                return _coordinatesPart1[Y][Math.Max(0, X - 1)];
            }

            private Coordinate GetRight()
            {
                return _coordinatesPart1[Y][Math.Min(_coordinatesPart1[0].Count - 1, X + 1)];
            }

            private bool CanGoUp(List<Coordinate> visited, out Coordinate res)
            {
                var top = GetTop();
                res = top;
                if (visited.Any(v => v.Equals(top)))
                {
                    return false;
                }

                return new char[] { '|', '7', 'F', 'S' }.Contains(top.Value);
            }

            private bool CanGoDown(List<Coordinate> visited, out Coordinate res)
            {
                var down = GetDown();
                res = down;
                if (visited.Any(v => v.Equals(down)))
                {
                    return false;
                }

                return new char[] { '|', 'L', 'J', 'S' }.Contains(down.Value);
            }

            private bool CanGoLeft(List<Coordinate> visited, out Coordinate res)
            {
                var left = GetLeft();
                res = left;
                if (visited.Any(v => v.Equals(left)))
                {
                    return false;
                }

                return new char[] { '-', 'L', 'F', 'S' }.Contains(left.Value);
            }

            private bool CanGoRight(List<Coordinate> visited, out Coordinate res)
            {
                var right = GetRight();
                res = right;
                if (visited.Any(v => v.Equals(right)))
                {
                    return false;
                }

                return new char[] { '-', '7', 'J', 'S' }.Contains(right.Value);
            }

            public override bool Equals(object? obj)
            {
                return obj is Coordinate coordinate &&
                       X == coordinate.X &&
                       Y == coordinate.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }


        [Fact]
        public void Part1()
        {
            _coordinatesPart1 = File.ReadLines("./inputs/day10/input.txt")
                .Select((y, yi) => y
                    .ToCharArray()
                    .Select((x, xi) => new Coordinate(xi, yi, x))
                    .ToList())
                .ToList();

            var startingPoint = GetStartingPoint();

            startingPoint.Move(0, new List<Coordinate>(), string.Empty);

            Assert.Equal(42, _pointsPart1.Max());
        }

        private Coordinate GetStartingPoint()
        {
            for (int y = 0; y < _coordinatesPart1.Count; y++)
            {
                for (int x = 0; x < _coordinatesPart1[y].Count; x++)
                {
                    if (_coordinatesPart1[y][x].Value == 'S')
                    {
                        return _coordinatesPart1[y][x];
                    }
                }
            }
            throw new Exception();
        }
    }
}
