using DPRobots.Logging;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record StocksUserInstruction : IUserInstruction
{
    public const string CommandName = "STOCKS";

    public override string ToString() => CommandName;
    
    public static IUserInstruction? TryParse(string args)
    {
        try
        {
            UserInstructionArgumentParser.EnsureEmpty(args, CommandName);
            return new StocksUserInstruction();
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }
    public void Execute()
    {
        var stockManager = StockManager.GetInstance();
        var pieceStock = stockManager.GetStock;
        var robotStock = stockManager.GetStock;
        foreach (var stockItem in robotStock)
            Console.WriteLine($"{stockItem.Quantity} {stockItem.Prototype}");

        foreach (var stockItem in pieceStock)
            Console.WriteLine($"{stockItem.Quantity} {stockItem.Prototype}");
    }
}