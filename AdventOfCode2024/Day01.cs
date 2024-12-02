using AdventOfCode2024.SharedKernel;

namespace AdventOfCode2024;

public class Day01
{
    private readonly List<long> leftSide = new();
    private readonly List<long> rightSide = new();
    
    public Day01()
    {
        var inputs = InputHelpers.GetInput("day01")
            .Split("\n")
            .Where(s => !string.IsNullOrWhiteSpace(s));

        foreach (var input in inputs)
        {
            var pair = input.Split("   ");
            leftSide.Add(long.Parse(pair[0]));
            rightSide.Add(long.Parse(pair[1]));
        }
    }
    
    [Fact]
    public void Part1()
    {
        leftSide.Sort();
        rightSide.Sort();
        
        var result = 0L;
        
        for (var i = 0; i < leftSide.Count; i++)
        {
            var left = leftSide[i];
            var right = rightSide[i];
            result += Math.Abs(left - right);
        }
        
        Assert.Equal(1941353, result);
    }
    
    [Fact]
    public void Part2()
    {
        var result = 0L;
        
        var groupedRight = rightSide
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());

        foreach (var left in leftSide)
        {
            var rightCount = groupedRight.GetValueOrDefault(left, 0);
            result += rightCount * left;
        }
        
        Assert.Equal(22539317, result);
    }
}