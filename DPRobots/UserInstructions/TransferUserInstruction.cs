using DPRobots.Logging;
using DPRobots.RobotFactories;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record TransferUserInstruction(
    RobotFactory SourceFactory,
    RobotFactory TargetFactory,
    List<StockItem> PiecesToTransfer) : IUserInstruction
{
    public const string CommandName = "TRANSFER";

    public override string ToString() => $"{CommandName} {GivenArgs}";

    private static string? GivenArgs { get; set; }
    
    public static IUserInstruction? TryParse(string args)
    {
        try
        {
            var parts = args.Split(',', 3, StringSplitOptions.TrimEntries);
            if (parts.Length != 3) return null;

            var source = FactoryManager.GetInstance().GetFactory(parts[0]);
            var target = FactoryManager.GetInstance().GetFactory(parts[1]);
            var pieces = UserInstructionArgumentParser.ParseStockItems(parts[2]);

            GivenArgs = args;
            return new TransferUserInstruction(source, target, pieces);
        }
        catch (Exception e)
        {
            Logger.Log(LogType.ERROR, $"TRANSFER parsing error: {e.Message}");
            return null;
        }
    }

    public void Execute()
    {
        foreach (var item in PiecesToTransfer)
        {
            if (!SourceFactory.Stock.Has(item))
            {
                Logger.Log(LogType.ERROR, $"Not enough `{item.Prototype}` in `{SourceFactory.Name}`");
                continue;
            }

            SourceFactory.Stock.RemoveStockItem(item, ToString());
            TargetFactory.Stock.AddStockItem(item, ToString());
        }

        Logger.Log(LogType.STOCK_UPDATED);
    }
}
