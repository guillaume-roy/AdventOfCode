using AdventOfCode2024.SharedKernel;

namespace AdventOfCode2024;

public class Day04
{
    public enum Direction
    {
        Top,
        Right,
        Bottom,
        Left,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight
    }

    [Fact]
    public void Part1()
    {
        var inputs = InputHelpers.GetInput("day04")
            .Trim()
            .Split("\n")
            .ToArray();

        var directions = Enum.GetValues<Direction>();

        var count = 0;
        for (var i = 0; i < inputs.Length; i++)
        {
            for (var j = 0; j < inputs[i].Length; j++)
            {
                foreach (var direction in directions)
                {
                    if (FindXmas(j, i, inputs, "" + inputs[i][j], direction))
                    {
                        count++;
                    }
                }
            }
        }

        Assert.Equal(2358, count);
    }

    [Fact]
    public void Part2()
    {
        var inputs = InputHelpers.GetInput("day04")
            .Trim()
            .Split("\n")
            .ToArray();

        var blocks = BuildBlocks(inputs);

        var count = 0;
        foreach (var block in blocks)
        {
            var blockCount = 0;

            if ($"{block[0][0]}{block[1][1]}{block[2][2]}" == "MAS")
                blockCount++;

            if ($"{block[0][2]}{block[1][1]}{block[2][0]}" == "MAS")
                blockCount++;

            if ($"{block[2][0]}{block[1][1]}{block[0][2]}" == "MAS")
                blockCount++;

            if ($"{block[2][2]}{block[1][1]}{block[0][0]}" == "MAS")
                blockCount++;

            if (blockCount == 2)
                count++;
        }

        Assert.Equal(1737, count);
    }

    private List<string[]> BuildBlocks(string[] inputs)
    {
        var blocks = new List<string[]>();
        for (var i = 0; i < inputs.Length - 2; i++)
        {
            for (var j = 0; j < inputs[i].Length - 2; j++)
            {
                var block = new string[3];
                for (var k = 0; k < 3; k++)
                {
                    block[k] = inputs[i + k].Substring(j, 3);
                }

                blocks.Add(block);
            }
        }

        return blocks;
    }

    private bool FindXmas(int x, int y, string[] inputs, string buffer, Direction direction)
    {
        if (buffer == "XMAS")
            return true;

        if (buffer.Length == 4)
            return false;

        var (newX, newY) = GetNewCoordinates(x, y, direction);
        if (newX < 0 || newX >= inputs[0].Length || newY < 0 || newY >= inputs.Length)
            return false;

        buffer += inputs[newY][newX];
        return FindXmas(newX, newY, inputs, buffer, direction);
    }

    private (int newX, int newY) GetNewCoordinates(int x, int y, Direction direction)
    {
        return direction switch
        {
            Direction.Top => (x, y - 1),
            Direction.Right => (x + 1, y),
            Direction.Bottom => (x, y + 1),
            Direction.Left => (x - 1, y),
            Direction.TopLeft => (x - 1, y - 1),
            Direction.TopRight => (x + 1, y - 1),
            Direction.BottomLeft => (x - 1, y + 1),
            Direction.BottomRight => (x + 1, y + 1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }
}