/// <summary>
/// Outputs text to the console in the given color.
/// </summary>
public static class ColoredConsole
{
    public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    public static void Write(string text, ConsoleColor color = ConsoleColor.White)
    {
        Console.ForegroundColor = color;
        Console.Write(text);
        Console.ResetColor();
    }

    public static string PromptUser(string question, ConsoleColor color = ConsoleColor.White)
    {
        while (true)
        {
            Write(question, color);
            Console.ForegroundColor = ConsoleColor.Cyan;

            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) continue;

            Console.ResetColor();
            return input;
        }
    }
}
