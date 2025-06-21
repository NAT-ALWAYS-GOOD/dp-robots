using DPRobots.Logging;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record ReceiveUserInstruction(List<StockItem> ItemsToAdd) : IUserInstruction
{
    public const string CommandName = "RECEIVE";

    public override string ToString() => CommandName;

    public static IUserInstruction? TryParse(string args)
    {
        if (string.IsNullOrWhiteSpace(args))
            return null;

        try
        {
            var itemsToAdd = UserInstructionArgumentParser.ParseStockItems(args);
            return new ReceiveUserInstruction(itemsToAdd);
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
            StockManager.GetInstance().AddStockItem(item);
        }
        Logger.Log(LogType.STOCK_UPDATED);
    }
}