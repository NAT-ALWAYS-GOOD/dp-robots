using DPRobots.Logging;
using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record NeededStocksUserInstruction(Dictionary<RobotBlueprint, int> RobotsWithQuantities) : IUserInstruction
{
    public const string CommandName = "NEEDED_STOCKS";

    public override string ToString() => $"{CommandName} {GivenArgs}";
    
    private static string? GivenArgs { get; set; }
    
    public static IUserInstruction? TryParse(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return null;

        try
        {
            var robotsWithQuantities = UserInstructionArgumentParser.ParseRobotsWithQuantities(args);
            GivenArgs = args;
            return new NeededStocksUserInstruction(robotsWithQuantities);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }

    public void Execute()
    {
        var overallTotals = StockManager.CalculateOverallNeededStocks(RobotsWithQuantities, true);

        if (overallTotals.Count == 0) return;

        Console.WriteLine("Total:");
        foreach (var total in overallTotals)
            Console.WriteLine($"{total.Value} {total.Key}");
    }
}