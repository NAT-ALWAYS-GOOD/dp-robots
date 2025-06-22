using DPRobots.Logging;
using DPRobots.RobotFactories;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record StocksUserInstruction(RobotFactory? Factory = null) : IUserInstruction
{
    public const string CommandName = "STOCKS";

    public override string ToString() => CommandName;

    public static IUserInstruction? TryParse(string args)
    {
        var (stockArgs, factory) = UserInstructionArgumentParser.SplitArgsAndFactory(args);

        try
        {
            UserInstructionArgumentParser.EnsureEmpty(stockArgs, CommandName);
            return new StocksUserInstruction(factory);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }

    public void Execute()
    {
        IReadOnlyList<StockItem> stock;
        if (Factory is null) stock = FactoryManager.GetInstance().GetTotalStockItems();
        else stock = Factory.Stock.GetStock;

        foreach (var stockItem in stock)
            Console.WriteLine($"{stockItem.Quantity} {stockItem.Prototype}");
    }
}