using DPRobots.Logging;
using DPRobots.RobotFactories;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record ReceiveUserInstruction(List<StockItem> ItemsToAdd, RobotFactory Factory) : IUserInstruction
{
    public const string CommandName = "RECEIVE";

    public override string ToString() => $"{CommandName} {GivenArgs}";
    
    private static string? GivenArgs { get; set; }

    public static IUserInstruction? TryParse(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return null;
        
        var (stockArgs, factory) = UserInstructionArgumentParser.SplitArgsAndFactory(args);
        if (factory is null)
        {
            Logger.Log(LogType.ERROR,
                $"Missing target factory. Available factory for this instruction are {string.Join(", ", FactoryManager.GetInstance().Factories.Select(f => f.Name))}.");
            return null;
        }

        try
        {
            var itemsToAdd = UserInstructionArgumentParser.ParseStockItems(stockArgs, factory);
            GivenArgs = args;
            return new ReceiveUserInstruction(itemsToAdd, factory);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, e.Message);
            return null;
        }
    }

    public void Execute()
    {
        foreach (var item in ItemsToAdd)
        {
            Factory.Stock.AddStockItem(item);
        }
        Logger.Log(LogType.STOCK_UPDATED);
    }
}