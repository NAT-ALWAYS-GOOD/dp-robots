using DPRobots.Logging;
using DPRobots.Robots;
using DPRobots.Stock;

namespace DPRobots.UserInstructions;

public record VerifyUserInstruction : IUserInstruction
{
    public const string CommandName = "VERIFY";

    public override string ToString() => CommandName;

    public static void Execute(Dictionary<string, int> verifyArgs)
    {
        var request = verifyArgs
            .ToDictionary(kvp => Robot.FromName(kvp.Key)!, kvp => kvp.Value);
        Logger.Log(StockManager.VerifyRequestedQuantitiesAreAvailable(request)
            ? LogType.AVAILABLE
            : LogType.UNAVAILABLE);
    }
}