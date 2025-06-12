using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record NeededStocksUserInstruction : IUserInstruction
{
    public override string ToString() => "NEEDED_STOCKS";

    public static void Execute(Dictionary<string, int> args)
    {
        var request = args
            .ToDictionary(kvp => Robot.FromName(kvp.Key)!, kvp => kvp.Value);
        var overallTotals = StockManager.CalculateOverallNeededStocks(request, true);

        if (overallTotals.Count == 0) return;

        Console.WriteLine("Total:");
        foreach (var total in overallTotals)
            Console.WriteLine($"{total.Value} {total.Key}");
    }
}