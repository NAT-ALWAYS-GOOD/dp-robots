namespace DPRobots.Logging;

public enum LogType
{
    ERROR,
    AVAILABLE,
    UNAVAILABLE,
    STOCK_UPDATED,
    INSTRUCTION,
}

public static class Logger
{
    public static void Log(LogType logType, string? message = null)
    {
        SetConsoleColor(logType);
        Console.Write(logType);
        Console.ResetColor();
        
        if (!string.IsNullOrEmpty(message))
            Console.WriteLine(" " + message);
        else
            Console.WriteLine();
    }

    private static void SetConsoleColor(LogType logType)
    {
        Console.ForegroundColor = logType switch
        {
            LogType.ERROR => ConsoleColor.Red,
            LogType.AVAILABLE or LogType.UNAVAILABLE or LogType.STOCK_UPDATED => ConsoleColor.DarkYellow,
            LogType.INSTRUCTION => ConsoleColor.Green,
            _ => ConsoleColor.White
        };
    }
}