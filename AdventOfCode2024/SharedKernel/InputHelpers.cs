namespace AdventOfCode2024.SharedKernel;

public class InputHelpers
{
    public static string GetInput(string day)
    {
        var path = Path.Combine(Directory.GetCurrentDirectory(), "inputs", $"{day}.txt");
        return File.ReadAllText(path);
    }
}