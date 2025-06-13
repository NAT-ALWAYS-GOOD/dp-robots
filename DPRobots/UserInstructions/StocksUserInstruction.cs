using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record StocksUserInstruction : IUserInstruction
{
    public const string CommandName = "STOCKS";

    public override string ToString() => CommandName;

    public static void Execute()
    {
        var stockManager = StockManager.GetInstance();
        var pieceStock = stockManager.GetPieceStocks;
        var robotStock = stockManager.GetRobotStocks;
        foreach (var stockItem in robotStock)
            Console.WriteLine($"{stockItem.Quantity} {stockItem.RobotPrototype}");

        foreach (var stockItem in pieceStock)
            Console.WriteLine($"{stockItem.Quantity} {stockItem.Prototype}");
    }
}