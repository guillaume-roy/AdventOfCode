using AdventOfCode2024.SharedKernel;
using Xunit.Abstractions;

namespace AdventOfCode2024;

public class Day02(ITestOutputHelper output)
{
    [Fact]
    public void Part1()
    {
        var inputs = InputHelpers.GetInput("day02")
            .Split("\n")
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Split(" ").Select(long.Parse).ToList())
            .ToList();

        var count = inputs.Count(IsSafe);

        Assert.Equal(257, count);
    }

    [Fact]
    public void Part2()
    {
        var inputs = InputHelpers.GetInput("day02")
            .Split("\n")
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s.Split(" ").Select(long.Parse).ToList())
            .ToList();

        var count = inputs.Count(IsSafeWithDampener);

        Assert.Equal(328, count);
    }

    private bool IsSafe(List<long> inputs)
    {
        // If already safe, return true
        if (IsIncreasing(inputs) && HasAdjacentLevels(inputs))
            return true;

        if (IsDecreasing(inputs) && HasAdjacentLevels(inputs))
            return true;

        return false;
    }

    private bool IsSafeWithDampener(List<long> inputs)
    {
        // If already safe, return true
        if (IsIncreasing(inputs) && HasAdjacentLevels(inputs))
            return true;

        if (IsDecreasing(inputs) && HasAdjacentLevels(inputs))
            return true;

        // Try removing each level and check if it makes the sequence safe
        for (int i = 0; i < inputs.Count; i++)
        {
            var tempList = inputs.Where((_, index) => index != i).ToList();

            if (IsIncreasing(tempList) && HasAdjacentLevels(tempList))
                return true;

            if (IsDecreasing(tempList) && HasAdjacentLevels(tempList))
                return true;
        }

        return false;
    }

    private bool HasAdjacentLevels(List<long> inputs)
    {
        for (int i = 1; i < inputs.Count; i++)
        {
            var first = inputs[i - 1];
            var second = inputs[i];

            var diff = Math.Abs(first - second);
            if (diff is < 1 or > 3)
            {
                return false;
            }
        }

        return true;
    }

    private bool HasInnerAdjacentLevels(List<long> inputs)
    {
        for (var i = 0; i < inputs.Count; i++)
        {
            var temp = inputs.Where((_, j) => i != j).ToList();
            if (HasAdjacentLevels(temp))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsIncreasing(List<long> inputs)
    {
        for (int i = 1; i < inputs.Count; i++)
        {
            var first = inputs[i - 1];
            var second = inputs[i];

            if (first > second)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsInnerIncreasing(List<long> inputs)
    {
        for (var i = 0; i < inputs.Count; i++)
        {
            var temp = inputs.Where((_, j) => i != j).ToList();
            if (IsIncreasing(temp))
            {
                return true;
            }
        }

        return false;
    }

    private bool IsDecreasing(List<long> inputs)
    {
        for (int i = 1; i < inputs.Count; i++)
        {
            var first = inputs[i - 1];
            var second = inputs[i];

            if (first < second)
            {
                return false;
            }
        }

        return true;
    }

    private bool IsInnerDecreasing(List<long> inputs)
    {
        for (var i = 0; i < inputs.Count; i++)
        {
            var temp = inputs.Where((_, j) => i != j).ToList();
            if (IsDecreasing(temp))
            {
                return true;
            }
        }

        return false;
    }
}