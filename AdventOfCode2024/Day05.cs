using System.Collections.Concurrent;
using AdventOfCode2024.SharedKernel;

namespace AdventOfCode2024;

public class Day05
{
    [Fact]
    public void Part1()
    {
        var inputs = InputHelpers.GetInput("day05")
            .Trim()
            .Split("\n")
            .Where(c => string.IsNullOrWhiteSpace(c) is false)
            .GroupBy(c => c.Contains("|"))
            .ToArray();

        var forwardPageOrders = inputs[0]
            .Select(c => c.Trim().Split("|").Select(int.Parse).ToList())
            .ToLookup(c => c[0], c => c[1]);

        var backPageOrders = inputs[0]
            .Select(c => c.Trim().Split("|").Select(int.Parse).ToList())
            .ToLookup(c => c[1], c => c[0]);

        var pageUpdates = inputs[1]
            .Select(c => c.Trim().Split(",").Select(int.Parse).ToList())
            .ToArray();

        var count = 0;
        foreach (var pageUpdate in pageUpdates)
        {
            var isValid = true;
            for (var i = 0; i < pageUpdate.Count; i++)
            {
                var page = pageUpdate[i];

                var forwardPages = pageUpdate.Skip(i + 1).ToArray();
                var backPages = pageUpdate.Take(i).ToArray();

                if (IsPageOrderValid(page, backPages, forwardPages, forwardPageOrders, backPageOrders) is false)
                {
                    isValid = false;
                }
            }

            if (!isValid) continue;

            var middleValue = pageUpdate[pageUpdate.Count / 2];
            count += middleValue;
        }

        Assert.Equal(6951, count);
    }

    [Fact]
    public void Part2()
    {
        var inputs = InputHelpers.GetInput("day05")
            .Trim()
            .Split("\n")
            .Where(c => string.IsNullOrWhiteSpace(c) is false)
            .GroupBy(c => c.Contains("|"))
            .ToArray();

        var forwardPageOrders = inputs[0]
            .Select(c => c.Trim().Split("|").Select(int.Parse).ToList())
            .ToLookup(c => c[0], c => c[1]);

        var backPageOrders = inputs[0]
            .Select(c => c.Trim().Split("|").Select(int.Parse).ToList())
            .ToLookup(c => c[1], c => c[0]);

        var pageUpdates = inputs[1]
            .Select(c => c.Trim().Split(",").Select(int.Parse).ToList())
            .ToArray();

        var incorrectPageUpdates = new List<List<int>>();

        foreach (var pageUpdate in pageUpdates)
        {
            var isValid = true;
            for (var i = 0; i < pageUpdate.Count; i++)
            {
                var page = pageUpdate[i];

                var forwardPages = pageUpdate.Skip(i + 1).ToArray();
                var backPages = pageUpdate.Take(i).ToArray();

                if (IsPageOrderValid(page, backPages, forwardPages, forwardPageOrders, backPageOrders) is false)
                {
                    isValid = false;
                }
            }

            if (!isValid)
                incorrectPageUpdates.Add(pageUpdate);
        }

        var count = 0;
        Parallel.ForEach(incorrectPageUpdates, pageUpdate =>
        {
            var newPageUpdate = new List<int>();

            var specificForwardPageOrders = inputs[0]
                .Select(c => c.Trim().Split("|").Select(int.Parse).ToList())
                .Where(x => pageUpdate.Contains(x[0]) && pageUpdate.Contains(x[1]))
                .ToLookup(x => x[0], x => x[1]);

            var currentPage = pageUpdate
                .First(p => specificForwardPageOrders.Contains(p) is false);
            newPageUpdate.Add(currentPage);

            var result = FindAncestors(newPageUpdate, pageUpdate.Count - 1, specificForwardPageOrders);
            var middleValue = result![result.Count / 2];
            Interlocked.Add(ref count, middleValue);
        });

        Assert.Equal(4121, count);
    }

    public List<int>? FindAncestors(List<int> buffer, int currentIndex, ILookup<int, int> lookup)
    {
        if (currentIndex == 0)
            return buffer;

        var possibleAncestors = lookup
            .Where(p => p.Contains(buffer.Last()))
            .ToList();

        foreach (var possibleAncestor in possibleAncestors)
        {
            var newBuffer = buffer.Append(possibleAncestor.Key).ToList();
            var found = FindAncestors(newBuffer, currentIndex - 1, lookup);
            if (found is not null)
                return found;
        }

        return null;
    }

    public bool IsPageOrderValid(int currentPage, int[] backPages, int[] forwardPages,
        ILookup<int, int> forwardPageOrders, ILookup<int, int> backPageOrders)
    {
        foreach (var backPage in backPages)
        {
            if (forwardPageOrders.Contains(currentPage) && forwardPageOrders[currentPage].Any(c => c == backPage))
            {
                return false;
            }
        }

        foreach (var forwardPage in forwardPages)
        {
            if (backPageOrders.Contains(currentPage) && backPageOrders[currentPage].Any(c => c == forwardPage))
            {
                return false;
            }
        }

        return true;
    }
}