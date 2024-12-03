using System.Text.RegularExpressions;
using AdventOfCode2024.SharedKernel;

namespace AdventOfCode2024;

public class Day03
{
    [Fact]
    public void Part1()
    {
        var inputs = InputHelpers.GetInput("day03");
        var result = Compute(inputs);
        Assert.Equal(161085926, result);
    }

    [Fact]
    public void Part2()
    {
        var inputs = InputHelpers.GetInput("day03");

        var buffer = "";
        var escape = false;

        for (var j = 0; j < inputs.Length; j++)
        {
            var inputSlice = inputs[..j];

            if (inputSlice.EndsWith("don't()"))
                escape = true;

            if (inputSlice.EndsWith("do()"))
                escape = false;

            if (escape is false)
                buffer += inputs[j];
        }
        
        var result = Compute(buffer);
        Assert.Equal(82045421, result);
    }

    private long Compute(string str)
    {
        var matches = Regex.Matches(str, @"mul\((\d+),(\d+)\)");
        var result = matches.Sum(m => long.Parse(m.Groups[1].Value) * long.Parse(m.Groups[2].Value));
        return result;
    }
}