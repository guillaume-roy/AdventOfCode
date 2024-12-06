using AdventOfCode2024.SharedKernel;

namespace AdventOfCode2024;

public class Day06
{
    [Fact]
    public void Part1()
    {
        var rows = InputHelpers.GetInput("day06")
            .Trim()
            .Split("\n")
            .Where(c => string.IsNullOrWhiteSpace(c) is false)
            .Select(c => c.ToCharArray())
            .ToArray();

        var position = FindStartingPosition(rows);

        var visitedPositions = Move(position, rows, [position]);

        Assert.Equal(5461, visitedPositions.DistinctBy(p => (p.Row, p.Column)).Count());
    }

    [Fact]
    public void Part2()
    {
        var rows = InputHelpers.GetInput("day06")
            .Trim()
            .Split("\n")
            .Where(c => string.IsNullOrWhiteSpace(c) is false)
            .Select(c => c.ToCharArray())
            .ToArray();

        var position = FindStartingPosition(rows);

        var visitedPositions = Move(position, rows, [position]).DistinctBy(p => (p.Row, p.Column));

        var count = 0;
        Parallel.ForEach(visitedPositions, visitedPosition =>
        {
            var updatedRows = rows.Select(r => r.ToArray()).ToArray();

            updatedRows![visitedPosition.Row][visitedPosition.Column] = '#';

            if (HasCycle(position, updatedRows, [position]))
                Interlocked.Increment(ref count);
        });

        Assert.Equal(1836, count);
    }

    public record Position(int Row, int Column, Direction Direction);

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    private List<Position> Move(Position position, char[][] rows, List<Position> visitedPositions)
    {
        while (true)
        {
            var nextPosition = FindNextPosition(position, rows);

            if (IsFinalPosition(nextPosition, rows))
                return visitedPositions;

            var tries = 4;
            while (IsValidPosition(nextPosition, rows) is false && tries > 0)
            {
                position = ChangeDirection(position);
                nextPosition = FindNextPosition(position, rows);
                tries--;
            }

            if (tries == 0) throw new Exception($"Infinite loop - {nextPosition}");

            position = nextPosition;
            visitedPositions = visitedPositions.Append(nextPosition).ToList();
        }
    }

    private bool HasCycle(Position position, char[][] rows, List<Position> visitedPositions)
    {
        while (true)
        {
            var nextPosition = FindNextPosition(position, rows);

            if (IsFinalPosition(nextPosition, rows))
                return false;

            var tries = 4;
            while (IsValidPosition(nextPosition, rows) is false && tries > 0)
            {
                position = ChangeDirection(position);
                nextPosition = FindNextPosition(position, rows);
                tries--;
            }

            if (tries == 0) return false;

            if (HasAlreadyBeenVisited(nextPosition, visitedPositions))
                return true;

            position = nextPosition;
            visitedPositions.Add(nextPosition);
        }
    }

    private Position ChangeDirection(Position position)
    {
        return position.Direction switch
        {
            Direction.Up => position with { Direction = Direction.Right },
            Direction.Right => position with { Direction = Direction.Down },
            Direction.Down => position with { Direction = Direction.Left },
            Direction.Left => position with { Direction = Direction.Up },
            _ => throw new Exception("Invalid direction")
        };
    }

    private Direction FindDirection(char position)
    {
        return position switch
        {
            '^' => Direction.Up,
            '>' => Direction.Right,
            'v' => Direction.Down,
            '<' => Direction.Left,
            _ => throw new Exception("Invalid direction")
        };
    }

    private Position FindStartingPosition(char[][] rows)
    {
        for (var row = 0; row < rows.Length; row++)
        {
            for (var column = 0; column < rows[row].Length; column++)
            {
                if (!new[] { '^', '>', '<', 'v' }.Contains(rows[row][column]))
                    continue;

                var direction = FindDirection(rows[row][column]);
                return new Position(row, column, direction);
            }
        }

        throw new Exception("Position not found");
    }

    private Position FindNextPosition(Position position, char[][] rows)
    {
        return position.Direction switch
        {
            Direction.Up => position with { Row = position.Row - 1 },
            Direction.Right => position with { Column = position.Column + 1 },
            Direction.Down => position with { Row = position.Row + 1 },
            Direction.Left => position with { Column = position.Column - 1 },
            _ => throw new Exception("Invalid direction")
        };
    }

    private bool IsValidPosition(Position position, char[][] rows)
    {
        return rows[position.Row][position.Column] != '#';
    }

    private bool IsFinalPosition(Position position, char[][] rows)
    {
        if (position.Row < 0 || position.Row >= rows.Length)
            return true;
        if (position.Column < 0 || position.Column >= rows[position.Row].Length)
            return true;
        return false;
    }

    private bool HasAlreadyBeenVisited(Position position, List<Position> visitedPositions)
    {
        return visitedPositions.Any(p =>
            p.Row == position.Row && p.Column == position.Column && p.Direction == position.Direction);
    }
}